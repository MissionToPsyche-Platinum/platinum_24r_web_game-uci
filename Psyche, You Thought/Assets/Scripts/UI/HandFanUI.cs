using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// Cards are evenly spaced across [−fanAngle/2, +fanAngle/2] degrees.
/// Each card's pivot is at the bottom-centre (0.5, 0) so rotation fans naturally.
/// The arc radius pushes cards outward so they don't overlap at the pivot.

/// <summary>
/// Lays out answer cards in a fan/arc arrangement mimicking holding cards in hand.
/// </summary>
public class HandFanUI : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject cardPrefab;

    [Header("Fan Settings")]
    [SerializeField] private float fanAngleDegrees = 40f; // total spread of the fan
    [SerializeField] private float arcRadius = 320f; // distance from pivot to card origin
    [SerializeField] private float hoverLift = 40f; // Y lift when hovering a card
    [SerializeField] private float cardScale = 1;

    [Header("Animation")]
    [SerializeField] private float dealDuration = 0.08f; // stagger delay per card
    [SerializeField] private float liftDuration = 0.12f; // hover animation time


    private readonly List<GameObject> _cardObjects = new List<GameObject>();
    private readonly List<Vector3> _restPositions = new List<Vector3>();
    private readonly List<float> _restAngles = new List<float>();
    private int _hoveredIndex = -1;

    // Callback: fired when player confirms a card (passes hand index)
    public System.Action<int> OnCardSelected;



    /// <summary>Populate and animate the fan with the current hand.</summary>
    public void BuildHand(IReadOnlyList<AnswerCard> hand)
    {
        ClearHand();
        if (hand == null || hand.Count == 0) return;

        int n = hand.Count;

        for (int i = 0; i < n; i++)
        {
            int capturedIndex = i;
            AnswerCard card = hand[i];

            GameObject go = Instantiate(cardPrefab, transform);
            go.SetActive(false); // hidden until deal animation

            RectTransform rt = go.GetComponent<RectTransform>();

            // Fan geometry
            float t = n == 1 ? 0f : (float)i / (n - 1);   // 0..1
            float angle = Mathf.Lerp(-fanAngleDegrees / 2f, fanAngleDegrees / 2f, t);
            float radAngle = angle * Mathf.Deg2Rad;

            // Arc position: cards spread along a circular arc below the container
            float x = Mathf.Sin(radAngle) * arcRadius;
            float y = (Mathf.Cos(radAngle) - 1f) * arcRadius * 0.18f; // slight vertical curve

            Vector3 restPos = new Vector3(x, y, 0f);

            rt.pivot = new Vector2(0.5f, 0f); // pivot at bottom-center

            rt.anchorMin = new Vector2(0.5f, 0f);
            rt.anchorMax = new Vector2(0.5f, 0f);

            rt.anchoredPosition = restPos;
            rt.localRotation = Quaternion.Euler(0f, 0f, -angle);
            rt.localScale = Vector3.one * cardScale;

            _restPositions.Add(restPos);
            _restAngles.Add(-angle);

            // Populate card text
            PopulateCardVisuals(go, card);

            // Wire button
            Button btn = go.GetComponent<Button>();
            if (btn)
            {
                btn.onClick.AddListener(() => SelectCard(capturedIndex));

                // Hover events via EventTrigger or PointerEnter/Exit
                WireHoverEvents(go, capturedIndex);
            }

            _cardObjects.Add(go);
        }

        // Staggered deal animation
        StartCoroutine(DealAnimation());
    }

    /// <summary>Discard a card at handIndex (called after selection confirmed).</summary>
    public void RemoveCard(int handIndex)
    {
        if (handIndex < 0 || handIndex >= _cardObjects.Count) return;
        Destroy(_cardObjects[handIndex]);
        _cardObjects.RemoveAt(handIndex);
        _restPositions.RemoveAt(handIndex);
        _restAngles.RemoveAt(handIndex);
    }

    public void ClearHand()
    {
        foreach (var go in _cardObjects) if (go) Destroy(go);
        _cardObjects.Clear();
        _restPositions.Clear();
        _restAngles.Clear();
        _hoveredIndex = -1;
    }

    // ── Card Visuals ──────────────────────────────────────────────────────────

    private static string MinigameLabel(MinigameType minigameType)
    {
        switch (minigameType)
        {
            case MinigameType.None : return "";
            case MinigameType.FlappyBird : return "Flappy Propulsion";
            case MinigameType.MatchCards : return "Match Mach";
            case MinigameType.WhackAMole : return "Whack An Alien";
            case MinigameType.SpaceInvaders : return "Invaders of Space";
            case MinigameType.RhythmQTE : return "Starry Beats";
            case MinigameType.RollingDice : return "Meteor Dice";
            default: return "";
        }
    }

    private void PopulateCardVisuals(GameObject go, AnswerCard card)
    {
        // Find named child TMP fields (set up in the prefab)
        TextMeshProUGUI titleTMP = FindChildTMP(go, "Title");
        TextMeshProUGUI descTMP = FindChildTMP(go, "Body");
        TextMeshProUGUI statsTMP = FindChildTMP(go, "Stats");
        TextMeshProUGUI tagTMP = FindChildTMP(go, "Tag");

        if (titleTMP) titleTMP.text = card.title;
        if (descTMP) descTMP.text = card.description;

        if (statsTMP)
        {
            string effStars = BuildStars(card.effectiveness, "★", "☆", 5);
            string chaosStars = BuildStars(card.chaos, "★", "☆", 5);
            string accStars = BuildStars(card.scientificAccuracy, "★", "☆", 5);
            statsTMP.text = $"Effectiveness: {effStars}\nChaos: {chaosStars}\nScientific Accuracy: {accStars}";
        }

        if (tagTMP)
        {
            tagTMP.gameObject.SetActive(card.IsSpecial());
            if (card.IsSpecial()) tagTMP.text = $"✦ {MinigameLabel(card.associatedMinigame)}";
        }
    }

    private static string BuildStars(int value, string filled, string empty, int max)
    {
        string s = "";
        for (int i = 0; i < max; i++) s += i < value ? filled : empty;
        return s;
    }

    private static TextMeshProUGUI FindChildTMP(GameObject root, string childName)
    {
        Transform t = root.transform.Find(childName);
        return t ? t.GetComponent<TextMeshProUGUI>() : null;
    }

    // ── Hover ─────────────────────────────────────────────────────────────────

    private void WireHoverEvents(GameObject go, int index)
    {
        // Add a PointerEnterExit component — simpler than EventTrigger boilerplate
        var hover = go.AddComponent<CardHoverHandler>();
        hover.Init(
            index,
            onEnter: (i) => StartCoroutine(LiftCard(i, true)),
            onExit:  (i) => StartCoroutine(LiftCard(i, false))
        );
    }

    private IEnumerator LiftCard(int index, bool up)
    {
        if (index < 0 || index >= _cardObjects.Count) yield break;

        GameObject go = _cardObjects[index];
        if (!go) yield break;

        if (up)
            go.transform.SetAsLastSibling();
        else
            go.transform.SetSiblingIndex(index);

        RectTransform rt = go.GetComponent<RectTransform>();
        if (!rt) yield break;

        Vector3 from = rt.anchoredPosition;
        Vector3 target = _restPositions[index] + (up ? Vector3.up * hoverLift : Vector3.zero);

        float elapsed = 0f;
        while (elapsed < liftDuration)
        {
            elapsed += Time.deltaTime;
            rt.anchoredPosition = Vector3.Lerp(from, target, elapsed / liftDuration);
            yield return null;
        }
        rt.anchoredPosition = target;
    }

    // ── Deal Animation ────────────────────────────────────────────────────────

    private IEnumerator DealAnimation()
    {
        for (int i = 0; i < _cardObjects.Count; i++)
        {
            yield return new WaitForSeconds(dealDuration);
            if (_cardObjects[i]) _cardObjects[i].SetActive(true);
            // TODO: add a slide-in tween
        }
    }

    // ── Selection ─────────────────────────────────────────────────────────────

    private void SelectCard(int index)
    {
        OnCardSelected?.Invoke(index);
    }
}

/// <summary>
/// Lightweight pointer-enter/exit forwarder.
/// Auto-added by HandFanUI.WireHoverEvents(); no manual setup needed.
/// </summary>
public class CardHoverHandler : MonoBehaviour,
    UnityEngine.EventSystems.IPointerEnterHandler,
    UnityEngine.EventSystems.IPointerExitHandler
{
    private int _index;
    private System.Action<int> _onEnter;
    private System.Action<int> _onExit;

    public void Init(int index, System.Action<int> onEnter, System.Action<int> onExit)
    {
        _index = index;
        _onEnter = onEnter;
        _onExit = onExit;
    }

    public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData _) => _onEnter?.Invoke(_index);
    public void OnPointerExit(UnityEngine.EventSystems.PointerEventData _) => _onExit?.Invoke(_index);
}
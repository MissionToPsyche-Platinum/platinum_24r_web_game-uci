using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Overlay panel shown during the judging phase.
/// </summary>
public class JudgingPanelUI : MonoBehaviour
{
    [Header("Panel Refs")]
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private CanvasGroup backgroundOverlay;

    [Header("Alien Section")]
    [SerializeField] private Image alienImage;
    [SerializeField] private TextMeshProUGUI alienNameText;
    [SerializeField] private TextMeshProUGUI speechText;

    [Header("Prompt Card")]
    [SerializeField] private TextMeshProUGUI promptTitleText;
    // [SerializeField] private TextMeshProUGUI promptDescText;

    [Header("Hand")]
    [SerializeField] private HandFanUI handFan;

    [Header("Alien Sprites")]
    [SerializeField] private Sprite[] alienSprites;

    [Header("Animate")]
    [SerializeField] private float openDuration  = 0.35f;
    [SerializeField] private float closeDuration = 0.25f;

    private PromptCard _currentPrompt;
    private bool _isOpen = false;


    private void Awake()
    {
        // Start hidden
        if (panelRoot) panelRoot.SetActive(false);
        if (backgroundOverlay) backgroundOverlay.alpha = 0f;
    }

    private void OnEnable()
    {
        GameManager.OnJudgingOpen += Open;
        GameManager.OnJudgingClose += Close;
    }

    private void OnDisable()
    {
        GameManager.OnJudgingOpen -= Open;
        GameManager.OnJudgingClose -= Close;
    }

    private void Open(PromptCard prompt)
    {
        StopAllCoroutines();
        _currentPrompt = prompt;

        if (panelRoot) panelRoot.SetActive(true);

        PopulateAlien();

        if (promptTitleText) promptTitleText.text = prompt.title;
        // if (promptDescText)  promptDescText.text  = "";

        if (speechText)
            speechText.text = GetAlienIntro();

        if (handFan != null && GameManager.Instance?.humanPlayer != null)
        {
            handFan.OnCardSelected = OnCardPicked;
            handFan.BuildHand(GameManager.Instance.humanPlayer.CurrentHand);
        }
        
        StartCoroutine(AnimateOpen());
    }

    private void Close()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateClose());
    }

    // ── Card Selection ────────────────────────────────────────────────────────

    private void OnCardPicked(int handIndex)
    {
        Debug.Log($"[JudgingPanel] OnCardPicked({handIndex})");
        StartCoroutine(PlayCardAndConfirm(handIndex));
    }

    private IEnumerator PlayCardAndConfirm(int handIndex)
    {
        if (speechText) speechText.text = GetAlienReaction();

        yield return new WaitForSeconds(0.8f);

        if (handFan) handFan.enabled = false;

        GameManager.Instance?.HumanConfirmCard(handIndex);
    }

    // ── Alien Flavour ─────────────────────────────────────────────────────────

    private static readonly string[] _alienNames =
    {
        "Zyx-9 of Vor'kaleth",
        "High Arbiter Kluu",
        "Sensor-Bloom Vrex",
        "The Mmm Collective",
        "Adjudicator Tharr",
    };

    private static readonly string[] _introLines =
    {
        "Earthling... impress me. Or don't. Either way I'm writing a report.",
        "Your biometrics suggest anxiety. Good. Now pick a card.",
        "We have judged 4,000 civilisations. Most of them no longer exist. Choose wisely.",
        "The prompt has been issued. The cosmos waits. Also, hurry up.",
        "I have read all of your planet's literature. Your card better be funnier.",
    };

    private static readonly string[] _reactionLines =
    {
        "Mmm. Interesting choice. Our analysts are... divided.",
        "I did not expect that. My species finds surprise distressing. Well done.",
        "Calculating... yes. This will be considered.",
        "Bold. Reckless. Possibly brilliant. We will see.",
        "Your logic is... uniquely human. I mean that as a compliment. Mostly.",
    };

    private int _alienIndex;

    private void PopulateAlien()
    {
        _alienIndex = Random.Range(0, _alienNames.Length);

        if (alienNameText) alienNameText.text = _alienNames[_alienIndex];

        if (alienImage && alienSprites != null && alienSprites.Length > 0)
            alienImage.sprite = alienSprites[Random.Range(0, alienSprites.Length)];
    }

    private string GetAlienIntro() => _introLines[Random.Range(0, _introLines.Length)];
    private string GetAlienReaction() => _reactionLines[Random.Range(0, _reactionLines.Length)];

    // ── Animations ────────────────────────────────────────────────────────────

    private IEnumerator AnimateOpen()
    {
        if (backgroundOverlay == null || panelRoot == null) yield break;

        RectTransform rt = panelRoot.GetComponent<RectTransform>();
        Vector3 startScale = Vector3.one * 0.85f;
        Vector3 endScale = Vector3.one;

        if (rt) rt.localScale = startScale;
        backgroundOverlay.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / openDuration);
            backgroundOverlay.alpha = Mathf.Lerp(0f, 0.6f, t);
            if (rt) rt.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        backgroundOverlay.alpha = 1f;
        if (rt) rt.localScale = endScale;
    }

    private IEnumerator AnimateClose()
    {
        if (backgroundOverlay == null) yield break;

        float elapsed = 0f;
        while (elapsed < closeDuration)
        {
            elapsed += Time.deltaTime;
            backgroundOverlay.alpha = Mathf.Lerp(0.6f, 0f, elapsed / closeDuration);
            yield return null;
        }

        handFan?.ClearHand();
        if (panelRoot)  panelRoot.SetActive(false);
        GameManager.Instance?.NotifyJudgingClosed();
    }
}
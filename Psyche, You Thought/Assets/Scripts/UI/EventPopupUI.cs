using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Random event popup that appears after judging rounds or story beats.
/// </summary>
public class EventPopupUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject popupRoot;
    [SerializeField] private TextMeshProUGUI iconText;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Button dismissButton;
    [SerializeField] private TextMeshProUGUI dismissLabel;

    [Header("Animation")]
    [SerializeField] private float openDuration  = 0.3f;
    [SerializeField] private float closeDuration = 0.2f;
    [SerializeField] private float bounceScale   = 1.08f;

    private void Awake()
    {
        if (popupRoot) popupRoot.SetActive(false);
        if (canvasGroup) canvasGroup.alpha = 0f;
    }

    private void OnEnable() => GameManager.OnRandomEvent += ShowEvent;
    private void OnDisable() => GameManager.OnRandomEvent -= ShowEvent;

    private void ShowEvent(RandomEvent evt)
    {
        StartCoroutine(RunPopup(evt));
    }

    private IEnumerator RunPopup(RandomEvent evt)
    {
        // Populate
        if (iconText) iconText.text = evt.iconText;
        if (titleText) titleText.text = evt.title;
        if (descText) descText.text = evt.description;
        if (dismissLabel)dismissLabel.text = evt.buttonLabel;

        // Show
        if (popupRoot) popupRoot.SetActive(true);
        yield return StartCoroutine(AnimateOpen());

        // Wait for dismiss
        bool dismissed = false;
        dismissButton.onClick.AddListener(() => dismissed = true);
        while (!dismissed) yield return null;
        dismissButton.onClick.RemoveAllListeners();

        // Hide
        yield return StartCoroutine(AnimateClose());
        if (popupRoot) popupRoot.SetActive(false);

        // Signal GameManager
        GameManager.Instance?.NotifyEventDismissed();
    }

    private IEnumerator AnimateOpen()
    {
        if (canvasGroup == null) yield break;

        RectTransform rt = popupRoot?.GetComponent<RectTransform>();
        float elapsed = 0f;

        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / openDuration);
            canvasGroup.alpha = t;

            // Bounce scale: overshoot slightly then settle
            float scaleT = t < 0.6f
                ? Mathf.Lerp(0.7f, bounceScale, t / 0.6f)
                : Mathf.Lerp(bounceScale, 1f, (t - 0.6f) / 0.4f);
            if (rt) rt.localScale = Vector3.one * scaleT;

            yield return null;
        }
        canvasGroup.alpha = 1f;
        if (rt) rt.localScale = Vector3.one;
    }

    private IEnumerator AnimateClose()
    {
        if (canvasGroup == null) yield break;

        float elapsed = 0f;
        while (elapsed < closeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / closeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
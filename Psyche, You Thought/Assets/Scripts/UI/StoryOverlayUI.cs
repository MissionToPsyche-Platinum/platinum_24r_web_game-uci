using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Visual-novel-style dialogue overlay for story beats.
/// </summary>
public class StoryOverlayUI : MonoBehaviour
{
    [Header("Overlay")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject  overlayRoot;

    [Header("Dialogue Box")]
    [SerializeField] private Image portraitImage;
    [SerializeField] private GameObject nameBadge;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject continuePrompt;

    [Header("Portraits")]
    [SerializeField] private Sprite portraitAria;
    [SerializeField] private Sprite portraitAlien;
    [SerializeField] private Sprite portraitPlayer;
    [SerializeField] private Sprite portraitNarrator; // null = no portrait shown

    [Header("Typewriter")]
    [SerializeField] private float charsPerSecond = 40f;

    [Header("Buttons")]
    [SerializeField] private Button skipButton;
    [SerializeField] private Button continueButton; // invisible fullscreen tap area 
    private List<StoryBeat> _beats;
    private int _beatIndex;
    private bool _beatReady; // player tapped "continue"
    private bool _skipAll;
    private bool _typewriterDone;
 

    private void Awake()
    {
        if (overlayRoot) overlayRoot.SetActive(false);
        if (canvasGroup) canvasGroup.alpha = 0f;
    }

    private void OnEnable()
    {
        GameManager.OnStoryBeats += ShowStory;
    }

    private void OnDisable()
    {
        GameManager.OnStoryBeats -= ShowStory;
    }

    // ── Entry Point ───────────────────────────────────────────────────────────

    private void ShowStory(List<StoryBeat> beats)
    {
        _beats     = beats;
        _beatIndex = 0;
        _skipAll   = false;
        StartCoroutine(RunStory());
    }

    // ── Story Coroutine ───────────────────────────────────────────────────────

    private IEnumerator RunStory()
    {
        // Fade in
        if (overlayRoot) overlayRoot.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(0f, 1f, 0.25f));

        // Wire buttons
        if (skipButton) skipButton.onClick.AddListener(OnSkip);
        if (continueButton) continueButton.onClick.AddListener(OnContinue);

        // Play each beat
        while (_beatIndex < _beats.Count && !_skipAll)
        {
            yield return StartCoroutine(PlayBeat(_beats[_beatIndex]));
            _beatIndex++;
        }

        // Fade out
        if (skipButton) skipButton.onClick.RemoveListener(OnSkip);
        if (continueButton) continueButton.onClick.RemoveListener(OnContinue);

        yield return StartCoroutine(FadeCanvasGroup(1f, 0f, 0.25f));
        if (overlayRoot) overlayRoot.SetActive(false);

        GameManager.Instance?.NotifyStoryDismissed();
    }

    private IEnumerator PlayBeat(StoryBeat beat)
    {
        _beatReady = false;
        _typewriterDone = false;

        if (nameBadge) nameBadge.SetActive(false);
        if (speakerNameText) speakerNameText.text = "";

        // Portrait
        bool hasPortrait = !string.IsNullOrEmpty(beat.portraitKey);
        if (portraitImage)
        {
            portraitImage.gameObject.SetActive(hasPortrait);
            if (hasPortrait)
                portraitImage.sprite = GetPortrait(beat.portraitKey);
        }

        // Speaker name
        if (!string.IsNullOrEmpty(beat.speakerName))
        {
            if (nameBadge) nameBadge.SetActive(true);
            if (speakerNameText) speakerNameText.text = beat.speakerName;
        }

        // Typewriter
        if (continuePrompt) continuePrompt.SetActive(false);
        yield return StartCoroutine(Typewrite(beat.dialogueLine));
        _typewriterDone = true;
        if (continuePrompt) continuePrompt.SetActive(true);

        // Wait for tap
        while (!_beatReady && !_skipAll) yield return null;
    }

    // ── Typewriter ────────────────────────────────────────────────────────────

    private IEnumerator Typewrite(string fullText)
    {
        if (dialogueText == null) yield break;

        dialogueText.text = "";
        float delay = 1f / charsPerSecond;

        for (int i = 0; i <= fullText.Length; i++)
        {
            if (_skipAll) { dialogueText.text = fullText; yield break; }

            // If player taps mid-typewriter, snap to full text
            if (_beatReady && !_typewriterDone)
            {
                dialogueText.text = fullText;
                _beatReady = false;
                yield break;
            }

            dialogueText.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }
    }

    // ── Button Handlers ───────────────────────────────────────────────────────

    private void OnContinue()
    {
        _beatReady = true;
    }

    private void OnSkip()
    {
        _skipAll = true;
        _beatReady = true;
    }

    // ── Portrait Lookup ───────────────────────────────────────────────────────

    private Sprite GetPortrait(string key)
    {
        return key?.ToLower() switch
        {
            "aria" => portraitAria,
            "alien" => portraitAlien,
            "player" => portraitPlayer,
            _ => portraitNarrator
        };
    }

    // ── Utility ───────────────────────────────────────────────────────────────

    private IEnumerator FadeCanvasGroup(float from, float to, float duration)
    {
        if (canvasGroup == null) yield break;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
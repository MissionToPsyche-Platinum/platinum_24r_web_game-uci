using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Root controller for the Main scene canvas.
/// Manages the persistent HUD (score, round counter) and
/// visibility of the judging panel, story overlay,
/// and event popup.
/// </summary>
public class MainGameUI : MonoBehaviour
{
    [Header("HUD Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI phaseText;

    [Header("Round Result Banner")]
    [SerializeField] private GameObject resultBanner;
    [SerializeField] private TextMeshProUGUI resultBannerText;
    [SerializeField] private float bannerDisplayTime = 2f;


    private void OnEnable()
    {
        GameManager.OnScoreAwarded += HandleScoreAwarded;
        GameManager.OnRoundEnd += HandleRoundEnd;
        GameManager.OnPhaseChanged += HandlePhaseChanged;
        GameManager.OnRoundResultReady += HandleRoundResult;
    }

    private void OnDisable()
    {
        GameManager.OnScoreAwarded -= HandleScoreAwarded;
        GameManager.OnRoundEnd -= HandleRoundEnd;
        GameManager.OnPhaseChanged -= HandlePhaseChanged;
        GameManager.OnRoundResultReady -= HandleRoundResult;
    }

    private void Start()
    {
        if (resultBanner) resultBanner.SetActive(false);
        RefreshScore();
        RefreshRound();

        GameManager.Instance?.BeginLoop();
    }

    // ── Event Handlers ────────────────────────────────────────────────────────

    private void HandleScoreAwarded(Player winner, int newScore)
    {
        // Only update HUD score if the human player won
        if (GameManager.Instance != null && winner == GameManager.Instance.humanPlayer)
            RefreshScore();

        // Banner shows winner regardless
        if (resultBannerText)
            resultBannerText.text = GameManager.Instance?.unlimitedRounds == true 
            ? "Onwards!" 
            : winner == GameManager.Instance?.humanPlayer
                ? "★ You won the round! ★"
                : $"{winner.playerName} won the round.";
    }

    private void HandleRoundEnd(int roundNumber)
    {
        RefreshRound();
    }

    private void HandlePhaseChanged(RoundPhase phase)
    {
        if (phaseText) phaseText.text = PhaseLabel(phase);
    }

    private void HandleRoundResult()
    {
        if (resultBanner == null) return;
        StopCoroutine(nameof(ShowBannerCoroutine));
        StartCoroutine(nameof(ShowBannerCoroutine));
    }

    // ── Internal ──────────────────────────────────────────────────────────────

    private void RefreshScore()
    {
        if (scoreText && GameManager.Instance?.humanPlayer != null)
            scoreText.text = $"{GameManager.Instance.humanPlayer.CurrentScore} pts";
    }

    private void RefreshRound()
    {
        if (roundText && GameManager.Instance != null) {
            roundText.text = GameManager.Instance?.unlimitedRounds == true
            ? $"{Math.Max(GameManager.Instance.missionGoal - GameManager.Instance.missionProgress, 0)} Gm away"
            : $"Round {GameManager.Instance.currentRound} / {GameManager.Instance.totalRounds}";
        }
    }

    private System.Collections.IEnumerator ShowBannerCoroutine()
    {
        if (resultBanner == null) yield break;
        resultBanner.SetActive(true);
        yield return new WaitForSeconds(bannerDisplayTime);
        resultBanner.SetActive(false);
    }

    private static string PhaseLabel(RoundPhase phase)
    {
        switch (phase)
        {
            case RoundPhase.DealCards: return "Dealing cards...";
            case RoundPhase.DisplayScenario: return "A new challenge appears!";
            case RoundPhase.PickCard: return "Choose your response";
            case RoundPhase.SubmitCard: return "Submitting...";
            case RoundPhase.RunMinigame: return "Minigame!";
            case RoundPhase.JudgeCards: return "The judge deliberates...";
            case RoundPhase.AwardPoints: return "Verdict!";
            case RoundPhase.NextRound: return "Next round incoming...";
            default: return "";
        }
    }
}
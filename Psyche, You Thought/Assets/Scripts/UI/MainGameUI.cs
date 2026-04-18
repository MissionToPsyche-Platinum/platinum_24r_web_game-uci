using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    }

    // ── Event Handlers ────────────────────────────────────────────────────────

    private void HandleScoreAwarded(Player winner, int newScore)
    {
        // Only update HUD score if the human player won
        if (GameManager.Instance != null && winner == GameManager.Instance.humanPlayer)
            RefreshScore();

        // Banner shows winner regardless
        if (resultBannerText)
            resultBannerText.text = winner == GameManager.Instance?.humanPlayer
                ? "✨ You won the round!"
                : $"{winner.playerName} won the round.";
    }

    private void HandleRoundEnd(int roundNumber)
    {
        RefreshRound();
    }

    private void HandlePhaseChanged(RoundPhase phase)
    {
        if (phaseText) phaseText.text = phase.ToString();
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
            scoreText.text = $"Score: {GameManager.Instance.humanPlayer.CurrentScore}";
    }

    private void RefreshRound()
    {
        if (roundText && GameManager.Instance != null)
            roundText.text = $"Round {GameManager.Instance.currentRound} / {GameManager.Instance.totalRounds}";
    }

    private System.Collections.IEnumerator ShowBannerCoroutine()
    {
        if (resultBanner == null) yield break;
        resultBanner.SetActive(true);
        yield return new WaitForSeconds(bannerDisplayTime);
        resultBanner.SetActive(false);
    }
}
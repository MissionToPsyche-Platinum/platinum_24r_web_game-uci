using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Attach to the root Canvas GameObject in the End scene.
/// </summary>
public class EndMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        int score = GameManager.Instance?.humanPlayer?.CurrentScore ?? 0;
        if (finalScoreText) finalScoreText.text = $"Final Score: {score}";

        // Simple win/lose message
        if (winnerText)
        {
            int aiTop = 0;
            if (GameManager.Instance != null)
                foreach (Player ai in GameManager.Instance.aiPlayers)
                    if (ai.CurrentScore > aiTop) aiTop = ai.CurrentScore;

            winnerText.text = score > aiTop ? "You won! 🎉" : score == aiTop ? "It's a tie!" : "Better luck next time.";
        }

        if (playAgainButton) playAgainButton.onClick.AddListener(OnPlayAgain);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(OnMainMenu);
    }

    private void OnPlayAgain() => GameManager.Instance?.StartGame();
    private void OnMainMenu() => SceneNavigator.GoToStart();
}

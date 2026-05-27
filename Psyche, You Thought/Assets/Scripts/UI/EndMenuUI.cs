using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Attach to the root Canvas GameObject in the End scene.
/// </summary>
public class EndMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        int score = GameManager.Instance?.humanPlayer?.CurrentScore ?? 0;
        if (finalScoreText) finalScoreText.text = $"You travelled {GameManager.Instance?.missionGoal} Gm!";

        if (playAgainButton) playAgainButton.onClick.AddListener(OnPlayAgain);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(OnMainMenu);
    }

    private void OnPlayAgain() => GameManager.Instance?.StartGame();
    private void OnMainMenu() => SceneNavigator.GoToStart();
}

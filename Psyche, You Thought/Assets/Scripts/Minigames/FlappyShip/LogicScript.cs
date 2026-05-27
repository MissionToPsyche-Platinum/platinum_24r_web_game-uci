using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int playerScore;
    public Text scoreText;
    public GameObject gameOverScreen;


    [ContextMenu("Increase Score")]
    public void addScore()
    {
        playerScore++;
        scoreText.text = playerScore.ToString();
        FlappyBirdController controller = FindObjectOfType<FlappyBirdController>();
        controller.AddScore(playerScore);
    }
    public void EndGame()
    {
        FlappyBirdController controller = FindObjectOfType<FlappyBirdController>();
        controller.ReturnToGame();
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }
}

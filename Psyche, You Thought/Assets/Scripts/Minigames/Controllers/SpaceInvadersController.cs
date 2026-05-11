using UnityEngine;
using UnityEngine.UI;

public class SpaceInvadersController : MinigameController
{
    private bool _running;
    private int pointsPerEnemy = 10;
    private int winBonus = 500;
    private int lossPenalty = 20;
    public Text scoreText;

    public override void StartMinigame()
    {
        minigameType = MinigameType.SpaceInvaders;
        minigameScore = 0;
        progress = 0;
        _running = true;
        scoreText.text = "Score: " + minigameScore;

        Debug.Log("[SpaceInvadersController] Minigame started.");
        // TODO
    }

    public void EnemyDestroyed()
    {
        if (!_running) return;

        AddPoints(pointsPerEnemy);
        Debug.Log($"[SpaceInvadersController] Enemy destroyed. Score: {minigameScore}");
        scoreText.text = "Score: " + minigameScore;

        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 1)
        {
            Win();
        }
    }

    public void PlayerHit()
    {
        if (!_running) return;
        scoreText.text = "Score: " + minigameScore;

        Lose();
    }

    private void Win()
    {
        _running = false;
        Debug.Log($"[SpaceInvadersController] Completed. Score: {minigameScore}");
        ReturnToGame(bonus: winBonus, penalty: 0);
    }

    private void Lose()
    {
        _running = false;
        Debug.Log($"[SpaceInvadersController] Failed. Score: {minigameScore}");
        ReturnToGame(bonus: minigameScore, penalty: lossPenalty);
    }


    public void Finish()
    {
        _running = false;

        // TODO

        Debug.Log("[SpaceInvadersController] Completed with [stats here].");
        ReturnToGame(bonus: minigameScore, penalty: 0); // temp
    }
}

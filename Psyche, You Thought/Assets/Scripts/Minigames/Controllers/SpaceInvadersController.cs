using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SpaceInvadersController : MinigameController
{
    private bool _running;
    private int pointsPerEnemy = 10;
    private int winBonus = 50;
    private int lossPenalty = 20;
    private int finalBonus = 0;
    private int finalPenalty = 0;
    public Text scoreText;
    public Text instructionText;
    public Text endText;
    private bool _ending = false;
    private float returnCountdown = 0f;

    public override void StartMinigame()
    {
        minigameType = MinigameType.SpaceInvaders;
        minigameScore = 0;
        progress = 0;
        _running = false;

        scoreText.text = "Score: " + minigameScore;

        if (instructionText != null)
        {
            instructionText.text = "Use arrows to move\nPress Space to shoot\nEliminate aliens and avoid enemy projectiles\n\nMove to start!";
        }

        if (endText != null)
        {
            endText.text = "";
        }

        Time.timeScale = 0f;

        Debug.Log("[SpaceInvadersController] Minigame loaded. Waiting for player movement.");
    }

    void Update()
    {
        if (!_running && !_ending)
        {
            if (Keyboard.current.upArrowKey.wasPressedThisFrame ||
                Keyboard.current.rightArrowKey.wasPressedThisFrame ||
                Keyboard.current.leftArrowKey.wasPressedThisFrame ||
                Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                BeginGameplay();
            }
        }
        if (_ending)
        {
            returnCountdown -= Time.unscaledDeltaTime;

            if (returnCountdown <= 0f)
            {
                _ending = false;
                Time.timeScale = 1f;
                ReturnToGame(bonus: finalBonus, penalty: finalPenalty);
            }
        }
    }

    private void BeginGameplay()
    {
        _running = true;
        Time.timeScale = 1f;

        if (instructionText != null)
        {
            instructionText.text = "";
        }

        Debug.Log("[SpaceInvadersController] Minigame started.");
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
        if (!_running) return;

        _running = false;

        Debug.Log($"[SpaceInvadersController] Completed. Score: {minigameScore}");

        StartEndCountdown("You Win!", winBonus, 0);
    }

    private void Lose()
    {
        if (!_running) return;

        _running = false;

        Debug.Log($"[SpaceInvadersController] Failed. Score: {minigameScore}");

        StartEndCountdown("Game Over!", minigameScore, lossPenalty);
    }

    private void StartEndCountdown(string message, int bonus, int penalty)
    {
        _ending = true;
        returnCountdown = 2f;
        finalBonus = bonus;
        finalPenalty = penalty;

        Time.timeScale = 0f;

        if (endText != null)
        {
            endText.text = message + "\nReturning to game...";
        }
    }


    // public void Finish()
    // {
    //     _running = false;
    //     Time.timeScale = 1f;

    //     Debug.Log("[SpaceInvadersController] Completed with [stats here].");
    //     ReturnToGame(bonus: minigameScore, penalty: 0); // temp
    // }
}

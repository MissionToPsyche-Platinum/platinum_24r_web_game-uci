using UnityEngine;

public class FlappyBirdController : MinigameController
{
    private bool _running;

    public override void StartMinigame()
    {
        minigameType = MinigameType.FlappyBird;
        minigameScore = 0;
        progress = 0;
        _running = true;

        Debug.Log("[FlappyBirdController] Minigame started.");
        // TODO
    }

    private void Update()
    {
        if (!_running) return;

        // TODO

        // Finish();
    }
    public void AddScore(int score)
    {
        minigameScore = score;
    }



    public void Finish()
    {
        _running = false;

        // TODO
        if (minigameScore >= 20) // temp win condition
        {
            Debug.Log("[FlappyBirdController] Completed. Score: " + minigameScore);
            ReturnToGame(bonus: minigameScore, penalty: 0);
        }
        else
        {
            Debug.Log("[FlappyBirdController] Failed. Score: " + minigameScore);
            ReturnToGame(bonus: 0, penalty: minigameScore);
        }
    }
}

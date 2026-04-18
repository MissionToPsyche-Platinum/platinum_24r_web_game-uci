using UnityEngine;

public class FlappyBirdController : MinigameController
{
    private bool  _running;

    public override void StartMinigame()
    {
        minigameType   = MinigameType.FlappyBird;
        minigameScore  = 0;
        progress       = 0;
        _running       = true;

        Debug.Log("[FlappyBirdController] Minigame started.");
        // TODO
    }

    private void Update()
    {
        if (!_running) return;

        // TODO

        Finish();
    }

    
    private void Finish()
    {
        _running = false;
        
        // TODO

        Debug.Log("[FlappyBirdController] Completed with [stats here].");
        ReturnToGame(bonus: minigameScore, penalty: 0); // temp
    }
}

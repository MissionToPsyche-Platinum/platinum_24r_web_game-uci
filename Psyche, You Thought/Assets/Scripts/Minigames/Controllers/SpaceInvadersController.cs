using UnityEngine;

public class SpaceInvadersController : MinigameController
{
    private bool  _running;

    public override void StartMinigame()
    {
        minigameType   = MinigameType.SpaceInvaders;
        minigameScore  = 0;
        progress       = 0;
        _running       = true;

        Debug.Log("[SpaceInvadersController] Minigame started.");
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

        Debug.Log("[SpaceInvadersController] Completed with [stats here].");
        ReturnToGame(bonus: minigameScore, penalty: 0); // temp
    }
}

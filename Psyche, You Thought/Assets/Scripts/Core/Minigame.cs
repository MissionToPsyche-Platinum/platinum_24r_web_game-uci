using System;
using UnityEngine;

/// <summary>
/// Data container written by a minigame scene before returning to Main.
/// GameManager reads and applies results to the active player.
/// </summary>
[Serializable]
public class MinigameResult
{
    public MinigameType minigameType;
    public int minigameScore;
    public int bonusPoints;
    public int penaltyPoints;
    public int cardIdToAdd = -1; // -1 = no reward card
    public int cardIdToRemove = -1; // -1 = no penalty card
    public bool completed;

    public MinigameResult() { }

    public MinigameResult(MinigameType type, int score, int bonus, int penalty,
                          bool completed = true, int addCard = -1, int removeCard = -1)
    {
        minigameType = type;
        minigameScore = score;
        bonusPoints = bonus;
        penaltyPoints = penalty;
        this.completed = completed;
        cardIdToAdd = addCard;
        cardIdToRemove = removeCard;
    }

    public int NetPoints => bonusPoints - penaltyPoints;
}

/// <summary>
/// Base class for all minigame scene controllers.
/// </summary>
public abstract class MinigameController : MonoBehaviour
{
    protected MinigameType minigameType;
    protected int minigameScore;
    protected int progress;

    protected virtual void Start()
    {
        StartMinigame();
    }

    /// <summary>Override to implement minigame startup and logic.</summary>
    public abstract void StartMinigame();

    /// <summary>
    /// Call when the minigame ends. Stores results and returns to the Main scene.
    /// </summary>
    public virtual void ReturnToGame(int bonus = 0, int penalty = 0,
                                     int cardReward = -1, int cardPenalty = -1)
    {
        MinigameResult result = new MinigameResult(
            minigameType,
            minigameScore,
            bonus,
            penalty,
            completed: true,
            addCard: cardReward,
            removeCard: cardPenalty
        );

        Debug.Log($"[MinigameController] '{minigameType}' done. Score:{minigameScore} Net:{result.NetPoints}");
        GameManager.Instance?.OnMinigameComplete(result);
    }

    /// <summary>Convenience: add points during gameplay and increment progress counter.</summary>
    protected void AddPoints(int pts)
    {
        minigameScore += pts;
        progress++;
    }
}

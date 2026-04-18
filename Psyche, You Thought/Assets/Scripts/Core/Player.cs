using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player
{
    public int id;
    public string playerName;
    public PlayerType playerType;

    private int _currentScore;
    public int CurrentScore => _currentScore;

    private List<AnswerCard> _currentHand = new List<AnswerCard>();
    public IReadOnlyList<AnswerCard> CurrentHand => _currentHand;

    public Player(int id, string name, PlayerType type = PlayerType.Human)
    {
        this.id = id;
        playerName = name;
        playerType = type;
    }

    // ── Hand Management ───────────────────────────────────────────────────────

    public void DrawCard(AnswerCard card)
    {
        if (card == null) { Debug.LogWarning("[Player] Tried to draw a null card."); return; }
        _currentHand.Add(card);
    }

    /// <summary>Picks the card at handIndex, removes it from hand, and returns it.</summary>
    public AnswerCard PickCard(int handIndex)
    {
        if (handIndex < 0 || handIndex >= _currentHand.Count)
        {
            Debug.LogError($"[Player:{playerName}] PickCard index {handIndex} out of range.");
            return null;
        }
        AnswerCard card = _currentHand[handIndex];
        _currentHand.RemoveAt(handIndex);
        Debug.Log($"[Player:{playerName}] Picked '{card.title}'.");
        return card;
    }

    public AnswerCard PickCardById(int cardId)
    {
        int idx = _currentHand.FindIndex(c => c.id == cardId);
        if (idx < 0)
        {
            Debug.LogError($"[Player:{playerName}] Card ID {cardId} not in hand.");
            return null;
        }
        return PickCard(idx);
    }

    // ── Scoring ───────────────────────────────────────────────────────────────

    public void AddScore(int points)
    {
        _currentScore += points;
        Debug.Log($"[Player:{playerName}] Score +{points} → {_currentScore}");
    }

    public void ResetScore() => _currentScore = 0;

    // ── AI ────────────────────────────────────────────────────────────────────

    /// <summary>Naïve AI: picks the card with the highest baseScore.</summary>
    public AnswerCard AIPickBestCard()
    {
        if (_currentHand.Count == 0) return null;
        int bestIdx = 0;
        for (int i = 1; i < _currentHand.Count; i++)
            if (_currentHand[i].baseScore > _currentHand[bestIdx].baseScore)
                bestIdx = i;
        return PickCard(bestIdx);
    }

    // ── Minigame Hooks ────────────────────────────────────────────────────────

    /// <summary>Apply score bonus/penalty and optional card changes after a minigame.</summary>
    public void ApplyMinigameResults(MinigameResult result)
    {
        if (result == null) return;
        AddScore(result.NetPoints);

        if (result.cardIdToAdd >= 0)
        {
            AnswerCard reward = CardDatabase.GetAnswerCardById(result.cardIdToAdd);
            if (reward != null) AddCardToDeck(reward);
        }
        if (result.cardIdToRemove >= 0)
            RemoveCardById(result.cardIdToRemove);

        Debug.Log($"[Player:{playerName}] Minigame results applied. Net: {result.NetPoints}");
    }

    public void AddCardToDeck(AnswerCard card)
    {
        if (card == null) return;
        _currentHand.Add(card);
        Debug.Log($"[Player:{playerName}] Card '{card.title}' added from minigame reward.");
    }

    public bool RemoveCardById(int cardId)
    {
        int idx = _currentHand.FindIndex(c => c.id == cardId);
        if (idx < 0) return false;
        _currentHand.RemoveAt(idx);
        return true;
    }
}

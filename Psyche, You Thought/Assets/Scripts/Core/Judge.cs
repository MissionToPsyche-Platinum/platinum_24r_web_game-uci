using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Judge
{
    public Preferences preferences;
    public List<PromptCard> promptCards = new List<PromptCard>();

    public Judge()
    {
        preferences = new Preferences(1, 1, 1);
    }

    public Judge(Preferences prefs, List<PromptCard> deck)
    {
        preferences = prefs;
        promptCards = deck ?? new List<PromptCard>();
    }

    /// <summary>Randomise judge preferences for this round.</summary>
    public Preferences SetPreferences()
    {
        preferences = Preferences.Random();
        Debug.Log($"[Judge] New preferences — eff:{preferences.effectivenessMultiplier} " +
                  $"chaos:{preferences.chaosMultiplier} acc:{preferences.accuracyMultiplier}");
        return preferences;
    }

    /// <summary>
    /// Score an AnswerCard against a PromptCard using judge preferences.
    /// Formula: baseScore + (eff * effMul) + (chaos * chaosMul) + (acc * accMul)
    /// </summary>
    public int CalculateScore(AnswerCard card, PromptCard prompt)
    {
        if (card == null || prompt == null)
        {
            Debug.LogError("[Judge] CalculateScore: null card or prompt.");
            return 0;
        }

        int score = card.baseScore
                  + card.effectiveness * preferences.effectivenessMultiplier
                  + card.chaos * preferences.chaosMultiplier
                  + card.scientificAccuracy * preferences.accuracyMultiplier;

        Debug.Log($"[Judge] '{card.title}' scored {score}.");
        return score;
    }

    /// <summary>Draw a random prompt card from the deck, removing it so it won't repeat.</summary>
    public PromptCard PresentPrompt()
    {
        if (promptCards == null || promptCards.Count == 0)
        {
            Debug.LogWarning("[Judge] Prompt deck exhausted.");
            return null;
        }
        int idx = Random.Range(0, promptCards.Count);
        PromptCard card = promptCards[idx];
        promptCards.RemoveAt(idx);
        Debug.Log($"[Judge] Presenting: '{card.title}'.");
        return card;
    }

    /// <summary>Given scored submissions, return the winning Player (ties broken randomly).</summary>
    public Player DetermineWinner(List<(Player player, AnswerCard card, int score)> submissions)
    {
        if (submissions == null || submissions.Count == 0) return null;

        int maxScore = int.MinValue;
        foreach (var s in submissions)
            if (s.score > maxScore) maxScore = s.score;

        List<(Player player, AnswerCard card, int score)> winners =
            submissions.FindAll(s => s.score == maxScore);

        var winner = winners[Random.Range(0, winners.Count)];
        Debug.Log($"[Judge] Winner: {winner.player.playerName} with {maxScore} pts.");
        return winner.player;
    }
}

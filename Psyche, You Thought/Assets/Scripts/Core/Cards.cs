using System;
using UnityEngine;

/// <summary>Base class for all cards.</summary>
[Serializable]
public abstract class Card
{
    public string title;
    public int id;
}

/// <summary>Scenario / prompt card dealt to the Judge and shown to players.</summary>
[Serializable]
public class PromptCard : Card
{
    public Preferences preferences;
    public int baseEffectiveness;
    public int baseChaos;
    public int baseAccuracy;

    public PromptCard() { }

    public PromptCard(int id, string title, Preferences prefs, int eff, int chaos, int acc)
    {
        this.id = id;
        this.title = title;
        preferences = prefs;
        baseEffectiveness = eff;
        baseChaos = chaos;
        baseAccuracy = acc;
    }
}

/// <summary>Answer card held by players; may trigger a minigame.</summary>
[Serializable]
public class AnswerCard : Card
{
    public string description;
    public int effectiveness;
    public int chaos;
    public int scientificAccuracy;
    public int baseScore;
    public MinigameType associatedMinigame = MinigameType.None;

    public AnswerCard() { }

    public AnswerCard(int id, string title, string desc,
                      int eff, int chaos, int acc, int baseScore,
                      MinigameType minigame = MinigameType.None)
    {
        this.id = id;
        this.title = title;
        description = desc;
        effectiveness = eff;
        this.chaos = chaos;
        scientificAccuracy = acc;
        this.baseScore = baseScore;
        associatedMinigame = minigame;
    }

    /// <summary>Returns true if playing this card triggers a minigame.</summary>
    public bool IsSpecial() => associatedMinigame != MinigameType.None;

    /// <summary>Asks the GameManager to launch the associated minigame.</summary>
    public void TriggerMinigame()
    {
        if (!IsSpecial())
        {
            Debug.LogWarning($"[AnswerCard] '{title}' has no minigame.");
            return;
        }
        GameManager.Instance?.StartMinigameForCard(this);
    }
}

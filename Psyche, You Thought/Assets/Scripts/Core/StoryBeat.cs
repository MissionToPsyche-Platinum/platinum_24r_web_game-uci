using System;
using UnityEngine;

/// <summary>
/// A single story beat shown in the visual-novel overlay.
/// Create instances via StoryDatabase.GetBeatsForProgress().
/// </summary>
[Serializable]
public class StoryBeat
{
    [TextArea(3, 6)]
    public string dialogueLine;

    /// <summary>
    /// Speaker name displayed above the dialogue box.
    /// Leave empty for narration (no name tag shown).
    /// </summary>
    public string speakerName;

    /// <summary>
    /// Optional sprite key (matched to StoryManager.characterSprites dict).
    /// Leave null/empty for narration beats with no portrait.
    /// </summary>
    public string portraitKey;
}

/// <summary>
/// A random event that can pop up after a judging round or story beat.
/// Events add flavour and may modify storyProgress.
/// </summary>
[Serializable]
public class RandomEvent
{
    public string title;
    [TextArea(2, 5)]
    public string description;

    /// <summary>Icon emoji or short string shown in the popup header.</summary>
    public string iconText = "!";

    /// <summary>Label on the dismiss button.</summary>
    public string buttonLabel = "Got it";

    /// <summary>Extra storyProgress added when the event fires (can be 0 or negative).</summary>
    public int progressDelta = 0;
}
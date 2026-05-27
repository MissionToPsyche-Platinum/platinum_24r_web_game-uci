using System.Collections.Generic;

/// <summary>
/// Static database of all story beats and random events.
/// Story beats are keyed by the storyProgress value at which they fire.
/// </summary>
public static class StoryDatabase
{
    // ── Story Beats ───────────────────────────────────────────────────────────
    // Key = storyProgress value that triggers this batch of beats.

    private static readonly Dictionary<int, List<StoryBeat>> _beatsByProgress
        = new Dictionary<int, List<StoryBeat>>
    {
        {
            0, new List<StoryBeat>
            {
                new StoryBeat
                {
                    speakerName  = "",
                    portraitKey  = "",
                    dialogueLine = "Year 2026. NASA's Psyche spacecraft hurtles through the asteroid belt, 280 million miles from home."
                },
                new StoryBeat
                {
                    speakerName  = "",
                    portraitKey  = "",
                    dialogueLine = "Its mission: reach asteroid 16 Psyche — a metal world the size of Massachusetts — and unlock the secrets of planetary cores."
                },
                new StoryBeat
                {
                    speakerName  = "ARIA",
                    portraitKey  = "aria",
                    dialogueLine = "Good morning, Mission Specialist. All systems nominal. We are on course for Psyche orbital insertion in... well, a while."
                },
                new StoryBeat
                {
                    speakerName  = "Mission Specialist",
                    portraitKey  = "player",
                    dialogueLine = "ARIA, what's that blip on the long-range sensor?"
                },
                new StoryBeat
                {
                    speakerName  = "ARIA",
                    portraitKey  = "aria",
                    dialogueLine = "Unknown contact. It appears to be... waving? I recommend we engage diplomatic protocols. Or, you know, improvise."
                },
            }
        },
        {
            2, new List<StoryBeat>
            {
                new StoryBeat
                {
                    speakerName  = "",
                    portraitKey  = "",
                    dialogueLine = "The alien ship matches Psyche's velocity with unsettling precision. A transmission crackles through."
                },
                new StoryBeat
                {
                    speakerName  = "Zyx-9",
                    portraitKey  = "alien",
                    dialogueLine = "Greetings, carbon-based unit. We are the Vor'kaleth. We have been watching your world for 12,000 of your years. You are... acceptable."
                },
                new StoryBeat
                {
                    speakerName  = "Mission Specialist",
                    portraitKey  = "player",
                    dialogueLine = "...Acceptable. Great. That's great."
                },
            }
        },
        {
            4, new List<StoryBeat>
            {
                new StoryBeat
                {
                    speakerName  = "ARIA",
                    portraitKey  = "aria",
                    dialogueLine = "We are 60% of the way to Psyche. The Vor'kaleth seem genuinely curious about our methods of problem-solving."
                },
                new StoryBeat
                {
                    speakerName  = "Zyx-9",
                    portraitKey  = "alien",
                    dialogueLine = "Your species chooses... interesting solutions. We find this amusing. And occasionally terrifying."
                },
            }
        },
        {
            6, new List<StoryBeat>
            {
                new StoryBeat
                {
                    speakerName  = "",
                    portraitKey  = "",
                    dialogueLine = "Asteroid Psyche looms large on the forward cameras. The metallic surface shimmers in the faint sunlight."
                },
                new StoryBeat
                {
                    speakerName  = "Mission Specialist",
                    portraitKey  = "player",
                    dialogueLine = "We're almost there. Whatever the Vor'kaleth think of us... this is what we came for."
                },
                new StoryBeat
                {
                    speakerName  = "Zyx-9",
                    portraitKey  = "alien",
                    dialogueLine = "One final challenge remains. Impress us, and we shall grant you safe passage. Fail, and... we will also grant you safe passage. But we will be disappointed."
                },
            }
        },
    };

    // ── Random Events ─────────────────────────────────────────────────────────

    private static readonly List<RandomEvent> _randomEvents = new List<RandomEvent>
    {
        new RandomEvent
        {
            title        = "Solar Flare Warning",
            iconText     = "!",
            description  = "A moderate solar flare has erupted. Psyche's sensors are briefly scrambled. Mission Control says: 'Don't panic. We're panicking for you.'",
            buttonLabel  = "Acknowledged",
            progressDelta = -1
        },
        new RandomEvent
        {
            title        = "Micrometeoroid Hit",
            iconText     = "!",
            description  = "A tiny rock the size of a grape just put a hole in the thermal blanket. The spacecraft is fine. The blanket is having a rough day.",
            buttonLabel  = "Patch it",
            progressDelta = -1
        },
        new RandomEvent
        {
            title        = "Cosmic Shortcut",
            iconText     = "?",
            description  = "The Vor'kaleth have nudged Psyche's trajectory with a gravitational assist. You're ahead of schedule!",
            buttonLabel  = "Thank them",
            progressDelta = 1
        },
        new RandomEvent
        {
            title        = "Signal from JPL",
            iconText     = "?",
            description  = "Mission Control wants to know if you remembered the lucky peanuts. You did, right?",
            buttonLabel  = "...Of course",
            progressDelta = 0
        },
        new RandomEvent
        {
            title        = "Alien Fan Mail",
            iconText     = "?",
            description  = "The Vor'kaleth have transmitted 47 terabytes of what appears to be fan art of the Psyche spacecraft. ARIA is uploading it to NASA's server.",
            buttonLabel  = "Wholesome",
            progressDelta = 0
        },
        new RandomEvent
        {
            title        = "Navigation Check",
            iconText     = "!",
            description  = "Star trackers are nominal. Trajectory confirmed. Psyche orbital insertion approaching. Time to make it count.",
            buttonLabel  = "Let's go",
            progressDelta = 0
        },
        new RandomEvent
        {
            title        = "Power Surge",
            iconText     = "!",
            description  = "The solar arrays produced 120% expected power this orbit. The extra energy went directly into ARIA's 'philosophical subroutines.'",
            buttonLabel  = "That explains it",
            progressDelta = 0
        },
        new RandomEvent
        {
            title        = "Deep Space Detection",
            iconText     = "?",
            description  = "The gamma-ray spectrometer picked up something unusual. Probably nothing. Definitely not a second Psyche. Probably.",
            buttonLabel  = "Log it",
            progressDelta = 0
        },
    };

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns story beats for the given progress milestone, or null if none.
    /// </summary>
    public static List<StoryBeat> GetBeatsForProgress(int progress)
    {
        if (_beatsByProgress.TryGetValue(progress, out List<StoryBeat> beats))
            return beats;
        return null;
    }

    /// <summary>Returns a random event from the pool.</summary>
    public static RandomEvent GetRandomEvent()
    {
        return _randomEvents[UnityEngine.Random.Range(0, _randomEvents.Count)];
    }

    /// <summary>Whether there are story beats at this progress milestone.</summary>
    public static bool HasBeatsAt(int progress)
        => _beatsByProgress.ContainsKey(progress);
}
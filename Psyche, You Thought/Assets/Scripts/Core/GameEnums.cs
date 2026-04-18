public enum GameMode { Solo, Multiplayer }

public enum MinigameType
{
    None,
    FlappyBird,
    MatchCards,
    WhackAMole,
    SpaceInvaders,
    RhythmQTE,
    RollingDice
}

public enum PlayerType { Human, AI }

public enum RoundPhase
{
    DealCards,
    DisplayScenario,
    PickCard,
    SubmitCard,
    RunMinigame,
    JudgeCards,
    AwardPoints,
    NextRound
}

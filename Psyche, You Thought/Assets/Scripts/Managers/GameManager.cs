using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Extended flow per round:
///   1. Check for story beat → show VN overlay (waits for player dismiss)
///   2. Show judging overlay (alien + prompt + fan hand)
///   3. Wait for player to pick card (judging overlay drives this)
///   4. AI picks, judge scores, winner awarded
///   5. Show result briefly
///   6. Maybe fire a random event popup
///   7. Advance storyProgress, loop

/// <summary>
/// Central singleton orchestrating the full gameplay loop.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Config")]
    public int totalRounds = 8;

    /// <summary>
    /// Max mission progress points. Progress bar fills from 0 → missionGoal.
    /// Each judging round adds pointsPerRound; random events can add bonuses.
    /// </summary>
    public int missionGoal = 100;
    public int pointsPerRound = 10;

    // ── State ─────────────────────────────────────────────────────────────────
    [HideInInspector] public Settings currentSettings;
    [HideInInspector] public int storyProgress; // which story milestone we're at
    [HideInInspector] public int currentRound;
    [HideInInspector] public int missionProgress; // 0 → missionGoal (drives progress bar)

    // ── Participants ──────────────────────────────────────────────────────────
    [HideInInspector] public Player humanPlayer;
    [HideInInspector] public List<Player> aiPlayers = new List<Player>();
    [HideInInspector] public Judge judge;
    [HideInInspector] public Dealer dealer;

    // ── Round state ───────────────────────────────────────────────────────────
    private PromptCard _currentPrompt;
    private List<(Player player, AnswerCard card, int score)> _submissions
        = new List<(Player, AnswerCard, int)>();

    // Human pick handshake
    private bool _humanPickReady;
    private AnswerCard _humanPickedCard;

    // Popup/story handshakes
    private bool _storyDismissed;
    private bool _eventDismissed;

    // ── Events ────────────────────────────────────────────────────────────────
    public delegate void PromptDealtHandler(PromptCard prompt);
    public delegate void CardSubmittedHandler(Player player, AnswerCard card);
    public delegate void ScoreAwardedHandler(Player winner, int newScore);
    public delegate void RoundEndHandler(int roundNumber);
    public delegate void PhaseChangedHandler(RoundPhase phase);
    public delegate void StoryBeatHandler(List<StoryBeat> beats);
    public delegate void RandomEventHandler(RandomEvent evt);
    public delegate void MissionProgressHandler(int current, int goal);
    public delegate void JudgingOpenHandler(PromptCard prompt);

    public static event PromptDealtHandler OnPromptDealt;
    public static event CardSubmittedHandler OnCardSubmitted;
    public static event ScoreAwardedHandler OnScoreAwarded;
    public static event RoundEndHandler OnRoundEnd;
    public static event System.Action OnGameEnd;
    public static event PhaseChangedHandler OnPhaseChanged;
    public static event StoryBeatHandler OnStoryBeats; // show VN overlay
    public static event RandomEventHandler OnRandomEvent; // show random popup
    public static event MissionProgressHandler OnMissionProgress; // drive progress bar
    public static event JudgingOpenHandler OnJudgingOpen; // open judging overlay
    public static event System.Action OnJudgingClose; // close judging overlay
    public static event System.Action OnRoundResultReady; // brief result flash

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentSettings = Settings.Load();
    }

    public void StartGame()
    {
        Debug.Log("[GameManager] StartGame()");
        currentRound = 0;
        storyProgress = 0;
        missionProgress = 0;

        if (humanPlayer == null) BuildDefaultPlayers();
        if (dealer == null) BuildDefaultDealer();
        if (judge == null) BuildDefaultJudge();

        SceneNavigator.GoToMain();
        StartCoroutine(RunGameLoop());
    }

    public void ChangeSettings(Settings newSettings)
    {
        currentSettings = newSettings;
        currentSettings.Save();
    }

    /// <summary>Called by StoryOverlayUI when all beats are dismissed.</summary>
    public void NotifyStoryDismissed() => _storyDismissed  = true;

    /// <summary>Called by EventPopupUI when the player taps the button.</summary>
    public void NotifyEventDismissed() => _eventDismissed  = true;

    /// <summary>Called by JudgingPanelUI when the player confirms a card.</summary>
    public void HumanConfirmCard(int handIndex)
    {
        if (humanPlayer == null) return;
        _humanPickedCard = humanPlayer.PickCard(handIndex);
        _humanPickReady = true;
    }

    // ── Main Loop ─────────────────────────────────────────────────────────────

    private IEnumerator RunGameLoop()
    {
        // Story intro (progress = 0)
        yield return StartCoroutine(MaybeShowStory());

        while (currentRound < totalRounds)
        {
            yield return StartCoroutine(RunRound());
            currentRound++;
            storyProgress++;
            OnRoundEnd?.Invoke(currentRound);

            // Advance mission progress
            missionProgress = Mathf.Clamp(missionProgress + pointsPerRound, 0, missionGoal);
            OnMissionProgress?.Invoke(missionProgress, missionGoal);

            // Story beat if milestone hit
            yield return StartCoroutine(MaybeShowStory());

            // Random event (50% chance after each round)
            if (Random.value < 0.5f)
                yield return StartCoroutine(ShowRandomEvent());

            if (missionProgress >= missionGoal) break;
        }

        EndGame();
    }

    private IEnumerator RunRound()
    {
        _submissions.Clear();
        SetPhase(RoundPhase.DealCards);
        dealer.DealCardsToAllPlayers(AllPlayers());

        // Set up prompt
        SetPhase(RoundPhase.DisplayScenario);
        _currentPrompt = judge.PresentPrompt();
        if (_currentPrompt == null) { Debug.LogError("[GameManager] Prompt deck empty!"); yield break; }
        judge.SetPreferences();
        OnPromptDealt?.Invoke(_currentPrompt);

        // Open the judging overlay — human picks from within it
        SetPhase(RoundPhase.PickCard);
        OnJudgingOpen?.Invoke(_currentPrompt);
        yield return StartCoroutine(WaitForHumanPick());
        OnJudgingClose?.Invoke();

        // AI picks
        foreach (Player ai in aiPlayers)
        {
            AnswerCard aiCard = ai.AIPickBestCard();
            if (aiCard != null)
            {
                int aiScore = judge.CalculateScore(aiCard, _currentPrompt);
                _submissions.Add((ai, aiCard, aiScore));
                OnCardSubmitted?.Invoke(ai, aiCard);
            }
        }

        // Judge
        SetPhase(RoundPhase.JudgeCards);
        SetPhase(RoundPhase.AwardPoints);
        Player winner = judge.DetermineWinner(_submissions);
        if (winner != null)
        {
            winner.AddScore(1);
            OnScoreAwarded?.Invoke(winner, winner.CurrentScore);
        }

        // Brief result flash
        OnRoundResultReady?.Invoke();
        yield return new WaitForSeconds(2.5f);

        // Discard + top up
        foreach (var entry in _submissions) dealer.Discard(entry.card);
        foreach (Player p in AllPlayers()) dealer.TopUpHand(p);

        SetPhase(RoundPhase.NextRound);
    }

    // ── Human Pick ────────────────────────────────────────────────────────────

    private IEnumerator WaitForHumanPick()
    {
        _humanPickReady = false;
        _humanPickedCard = null;

        while (!_humanPickReady) yield return null;

        if (_humanPickedCard.IsSpecial())
            yield return StartCoroutine(HandleMinigame(humanPlayer, _humanPickedCard));

        SetPhase(RoundPhase.SubmitCard);
        int baseScore = judge.CalculateScore(_humanPickedCard, _currentPrompt);
        int mgBonus = GameSessionData.LastMinigameResult?.NetPoints ?? 0;
        GameSessionData.LastMinigameResult = null;
        _submissions.Add((humanPlayer, _humanPickedCard, baseScore + mgBonus));
        OnCardSubmitted?.Invoke(humanPlayer, _humanPickedCard);
    }

    // ── Story / Events ────────────────────────────────────────────────────────

    private IEnumerator MaybeShowStory()
    {
        List<StoryBeat> beats = StoryDatabase.GetBeatsForProgress(storyProgress);
        if (beats == null || beats.Count == 0) yield break;

        _storyDismissed = false;
        OnStoryBeats?.Invoke(beats);
        while (!_storyDismissed) yield return null;
    }

    private IEnumerator ShowRandomEvent()
    {
        RandomEvent evt = StoryDatabase.GetRandomEvent();
        _eventDismissed = false;
        OnRandomEvent?.Invoke(evt);
        while (!_eventDismissed) yield return null;

        // Apply any progress delta from the event
        if (evt.progressDelta != 0)
        {
            missionProgress = Mathf.Clamp(missionProgress + evt.progressDelta * pointsPerRound, 0, missionGoal);
            OnMissionProgress?.Invoke(missionProgress, missionGoal);
        }
    }

    // ── Minigames ─────────────────────────────────────────────────────────────

    public void StartMinigameForCard(AnswerCard card)
    {
        GameSessionData.LastMinigameResult = null;
        SceneNavigator.GoToMinigame(card.associatedMinigame);
    }

    private IEnumerator HandleMinigame(Player player, AnswerCard card)
    {
        SetPhase(RoundPhase.RunMinigame);
        GameSessionData.LastMinigameResult = null;
        SceneNavigator.GoToMinigame(card.associatedMinigame);
        while (GameSessionData.LastMinigameResult == null) yield return null;
        player.ApplyMinigameResults(GameSessionData.LastMinigameResult);
        SceneNavigator.GoToMain();
    }

    public void OnMinigameComplete(MinigameResult result)
    {
        Debug.Log($"[GameManager] Minigame complete: {result.minigameType}, net {result.NetPoints}");
        GameSessionData.LastMinigameResult = result;
    }

    // ── End ───────────────────────────────────────────────────────────────────

    private void EndGame()
    {
        Debug.Log("[GameManager] Game over.");
        OnGameEnd?.Invoke();
        SceneNavigator.GoToEnd();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void SetPhase(RoundPhase phase)
    {
        Debug.Log($"[GameManager] Phase → {phase}");
        OnPhaseChanged?.Invoke(phase);
    }

    private List<Player> AllPlayers()
    {
        var all = new List<Player> { humanPlayer };
        all.AddRange(aiPlayers);
        return all;
    }

    private void BuildDefaultPlayers()
    {
        humanPlayer = new Player(0, "Player 1", PlayerType.Human);
        aiPlayers = new List<Player>
        {
            new Player(1, "AI Zara",  PlayerType.AI),
            new Player(2, "AI Blaze", PlayerType.AI),
        };
    }

    private void BuildDefaultDealer() => dealer = new Dealer(CardDatabase.GetAllAnswerCards());
    private void BuildDefaultJudge() => judge  = new Judge(Preferences.Random(), CardDatabase.GetAllPromptCards());
}

public static class GameSessionData
{
    public static MinigameResult LastMinigameResult;
}
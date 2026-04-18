using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// All scene name constants live here. Update these to match your Build Settings exactly.
/// </summary>
public static class SceneNavigator
{
    public const string SCENE_START = "Start";
    public const string SCENE_MAIN = "Main";
    public const string SCENE_END = "End";
    public const string SCENE_FLAPPY_BIRD = "MinigameFlappyBird";
    public const string SCENE_MATCH_CARDS = "MinigameMatchCards";
    public const string SCENE_WHACK_A_MOLE = "MinigameWhackAMole";
    public const string SCENE_SPACE_INV = "MinigameSpaceInvaders";
    public const string SCENE_RHYTHM_QTE = "MinigameRhythmQTE";
    public const string SCENE_ROLLING_DICE = "MinigameRollingDice";

    public static void GoToStart() => Load(SCENE_START);
    public static void GoToMain() => Load(SCENE_MAIN);
    public static void GoToEnd() => Load(SCENE_END);

    public static void GoToMinigame(MinigameType type)
    {
        string sceneName;
        switch (type)
        {
            case MinigameType.FlappyBird: sceneName = SCENE_FLAPPY_BIRD;  break;
            case MinigameType.MatchCards: sceneName = SCENE_MATCH_CARDS;  break;
            case MinigameType.WhackAMole: sceneName = SCENE_WHACK_A_MOLE; break;
            case MinigameType.SpaceInvaders:sceneName = SCENE_SPACE_INV;    break;
            case MinigameType.RhythmQTE: sceneName = SCENE_RHYTHM_QTE;   break;
            case MinigameType.RollingDice: sceneName = SCENE_ROLLING_DICE; break;
            default:
                Debug.LogError($"[SceneNavigator] No scene for MinigameType: {type}");
                return;
        }
        Load(sceneName);
    }

    private static void Load(string sceneName)
    {
        Debug.Log($"[SceneNavigator] → {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}

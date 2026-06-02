using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RhythmGameManager : MinigameController
{
    public TMP_Text scoreText;
    public TMP_Text resultText;

    private int score = 0;
    private float gameTimer = 0f;

    public override void StartMinigame()
    {
        minigameType = MinigameType.RhythmQTE;
        minigameScore = 0;
        progress = 0;

        resultText.text = "";
    }

    void Update()
    {
        if (Keyboard.current.aKey.isPressed)
            Hit("A");

        if (Keyboard.current.sKey.isPressed)
            Hit("S");

        if (Keyboard.current.dKey.isPressed)
            Hit("D");

        if (Keyboard.current.fKey.isPressed)
            Hit("F");

        scoreText.text = "Score: " + score;

        gameTimer += Time.deltaTime;

        if (score >= 1000)
        {
            resultText.text = "Mission Success!";
            ReturnToGame(bonus: 10);
        }
    }

    void Hit(string keyPressed)
    {
        NoteMovement[] notes = FindObjectsByType<NoteMovement>(
            FindObjectsSortMode.None
        );

        foreach (NoteMovement note in notes)
        {
            if (note.lane == keyPressed)
            {
                Destroy(note.gameObject);

                score += 100;
                minigameScore = score;

                return;
            }
        }
    }

    // bool foundMatch = false;

    // foreach(NoteMovement note in notes)
    // {
    //     if(note.lane == keyPressed)
    //     {
    //         Destroy(note.gameObject);

    //         score += 100;
    //         minigameScore = score;

    //         foundMatch = true;
    //         break;
    //     }
    // }

    // if(!foundMatch)
    // {
    //     score -= 50;

    //     if(score < 0)
    //         score = 0;
    // }
}
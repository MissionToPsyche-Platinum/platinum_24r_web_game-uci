using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DiceBattleGame : MinigameController
{
    public int playerHP = 20;
    public int enemyHP = 20;

    public TMP_Text playerHPText;
    public TMP_Text enemyHPText;
    public TMP_Text diceResultText;
    public TMP_Text battleLogText;

    public Button rollButton;
    public GameObject[] dicePips;

    public override void StartMinigame()
    {
        minigameType = MinigameType.RollingDice;
        minigameScore = 0;
        progress = 0;

        UpdateUI();
        battleLogText.text = "Roll the dice to attack!";
    }

    public void RollDice()
    {
        StartCoroutine(RollDiceAnimation());
    }

    IEnumerator RollDiceAnimation()
    {
        rollButton.interactable = false;

        for (int i = 0; i < 15; i++)
        {
            int fakeRoll = Random.Range(1, 7);
            ShowDiceFace(fakeRoll);
            yield return new WaitForSeconds(0.05f);
        }

        int roll = Random.Range(1, 7);
        ShowDiceFace(roll);
        HandleRollResult(roll);

        if (playerHP > 0 && enemyHP > 0)
            rollButton.interactable = true;
    }

    void HandleRollResult(int roll)
    {
        diceResultText.text = "Dice Roll: " + roll;

        int damage;
        string attackName;

        if (roll == 1)
        {
            damage = 0;
            attackName = "Miss";
        }
        else if (roll <= 3)
        {
            damage = 3;
            attackName = "Small Collision";
        }
        else if (roll <= 5)
        {
            damage = 5;
            attackName = "Meteor Shower";
        }
        else
        {
            damage = 6;
            attackName = "Planet Destroyer";
        }

        enemyHP -= damage;
        StartCoroutine(EnemyDamageEffect());

        if (enemyHP <= 0)
        {
            enemyHP = 0;
            minigameScore += 100;
            battleLogText.text = "You used " + attackName + " and won! Returning to main game...";
            UpdateUI();
            StartCoroutine(ReturnAfterDelay());
            return;
        }

        int enemyDamage = Random.Range(0, 4);
        playerHP -= enemyDamage;

        if (playerHP <= 0)
        {
            playerHP = 0;
            battleLogText.text = "You used " + attackName + ". Enemy hit back for " + enemyDamage + ". You lost!";
            UpdateUI();
            return;
        }

        battleLogText.text = "You used " + attackName + " for " + damage +
                             " damage. Enemy hit back for " + enemyDamage + " damage.";
        UpdateUI();
    }

    IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        ReturnToGame(bonus: 10);
    }

    void UpdateUI()
    {
        playerHPText.text = "Player HP: " + playerHP;
        enemyHPText.text = "Enemy HP: " + enemyHP;
    }

    void ShowDiceFace(int roll)
    {
        foreach (GameObject pip in dicePips)
            pip.SetActive(false);

        if (roll == 1)
        {
            dicePips[2].SetActive(true);
        }
        else if (roll == 2)
        {
            dicePips[0].SetActive(true);
            dicePips[5].SetActive(true);
        }
        else if (roll == 3)
        {
            dicePips[0].SetActive(true);
            dicePips[2].SetActive(true);
            dicePips[5].SetActive(true);
        }
        else if (roll == 4)
        {
            dicePips[0].SetActive(true);
            dicePips[1].SetActive(true);
            dicePips[4].SetActive(true);
            dicePips[5].SetActive(true);
        }
        else if (roll == 5)
        {
            dicePips[0].SetActive(true);
            dicePips[1].SetActive(true);
            dicePips[2].SetActive(true);
            dicePips[4].SetActive(true);
            dicePips[5].SetActive(true);
        }
        else if (roll == 6)
        {
            foreach (GameObject pip in dicePips)
                pip.SetActive(true);
        }
    }

    IEnumerator EnemyDamageEffect()
    {
        Color originalColor = enemyHPText.color;
        Vector3 originalPosition = enemyHPText.transform.localPosition;

        enemyHPText.color = Color.red;

        for (int i = 0; i < 8; i++)
        {
            enemyHPText.transform.localPosition =
                originalPosition + new Vector3(Random.Range(-8, 8), Random.Range(-8, 8), 0);

            yield return new WaitForSeconds(0.03f);
        }

        enemyHPText.transform.localPosition = originalPosition;
        enemyHPText.color = originalColor;
    }
}
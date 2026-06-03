using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class MatchCardsController : MinigameController
{
    private bool _running;
    private bool _win;

    [SerializeField]
    private Sprite cardBack;


    public Sprite lostLife;
    public Sprite[] frontImages;
    public List<Sprite> cardFronts = new List<Sprite>();
    public List<Button> cards = new List<Button>();

    public List<Image> lives = new List<Image>();

    private bool firstGuess, secondGuess;
    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;
    private int lifeCount;

    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessCard, secondGuessCard;

    private void Awake() {
        frontImages = Resources.LoadAll<Sprite>("match_card_game/match_card_fronts");
        lostLife = Resources.LoadAll<Sprite>("match_card_game")[0];
    }

    public override void StartMinigame()
    {
        minigameType   = MinigameType.MatchCards;
        minigameScore  = 0;
        progress       = 0;
        _running       = true;

        AudioManager.Instance?.PlayMusic(AudioManager.Instance?.matchCardsMusic);

        Debug.Log("[MatchCardsController] Minigame started.");

        GetCards();
        GetLives();
        AddListeners();
        AddCardFronts();
        Shuffle(cardFronts);
        gameGuesses = 6;
        lifeCount = 5;
    }

    void GetLives() {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("MatchCardLife");

        for (int i = 0; i < objects.Length; i++)
        {
            lives.Add(objects[i].GetComponent<Image>());
        }
    }

    void DeductLife() {
        lives[lifeCount - 1].sprite = lostLife;
        lifeCount--;
        AudioManager.Instance?.PlaySfx(AudioManager.Instance?.lifeLostSFX);

        if (lifeCount == 0) {
            Finish();
        }
    }

    void GetCards() {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("MatchCard");

        for (int i = 0; i < objects.Length; i++) {
            cards.Add(objects[i].GetComponent<Button>());
            cards[i].image.sprite = cardBack;
        }

    }

    void AddCardFronts() {
        int index = 0;
        for (int i = 0; i < cards.Count; i++) {
            if (index == cards.Count/2) {
                index = 0;
            }
            cardFronts.Add(frontImages[index]);
            index++;
        }
    }

    void AddListeners() {
        foreach (Button card in cards) {
            card.onClick.AddListener(() => PickACard());
        }
    }

    public void PickACard() {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        AudioManager.Instance?.PlaySfx(AudioManager.Instance?.cardFlipSFX);

        if (!firstGuess) {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGuessCard = cardFronts[firstGuessIndex].name;
            cards[firstGuessIndex].image.sprite = cardFronts[firstGuessIndex];

        } else if (!secondGuess) {
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            
            if (secondGuessIndex != firstGuessIndex) {
                secondGuess = true;
                secondGuessCard = cardFronts[secondGuessIndex].name;
                cards[secondGuessIndex].image.sprite = cardFronts[secondGuessIndex];
                countGuesses++;
                StartCoroutine(CheckIfCardsMatch());
            }
            
        }
    }

    IEnumerator CheckIfCardsMatch() {

        yield return new WaitForSeconds(1f);

        if (firstGuessCard == secondGuessCard) {

            cards[firstGuessIndex].interactable = false;
            cards[secondGuessIndex].interactable = false;

            AudioManager.Instance?.PlaySfx(AudioManager.Instance?.matchMadeSFX);

            IsGameFinished();

        } else {
            yield return new WaitForSeconds(0.2f);

            cards[firstGuessIndex].image.sprite = cardBack;
            cards[secondGuessIndex].image.sprite = cardBack;

            DeductLife();

        }

        firstGuess = secondGuess = false;
    }

    void IsGameFinished() {
        countCorrectGuesses++;

        if (countCorrectGuesses == gameGuesses) {
            _win = true;
            Finish();
        }
    }

    void Shuffle(List<Sprite> cardList) {
        for (int i = 0; i < cards.Count; i++) {
            Sprite temp = cardList[i];
            int randomIndex = Random.Range(i, cards.Count);
            cardList[i] = cardList[randomIndex];
            cardList[randomIndex] = temp;
        }
    }

    
    private void Finish()
    {
        _running = false;
        int losePenalty = 0;

        if (_win) {
            minigameScore = 100 * lifeCount;
        } else {
            minigameScore = 0;
            losePenalty = -100;
        }

        Debug.Log("[MatchCardsController] Completed with minigameScore: " + minigameScore + " and losePenalty: " + losePenalty);
        ReturnToGame(bonus: minigameScore, penalty: losePenalty);
    }
}

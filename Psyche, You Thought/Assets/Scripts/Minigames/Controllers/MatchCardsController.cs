using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MatchCardsController : MinigameController
{
    private bool _running;

    [SerializeField]
    private Sprite cardBack;

    public Sprite[] frontImages;
    public List<Sprite> cardFronts = new List<Sprite>();

    public List<Button> cards = new List<Button>();

    private bool firstGuess, secondGuess;
    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;

    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessCard, secondGuessCard;

    private void Awake() {
        frontImages = Resources.LoadAll<Sprite>("match_card_fronts");
    }

    public override void StartMinigame()
    {
        minigameType   = MinigameType.MatchCards;
        minigameScore  = 0;
        progress       = 0;
        _running       = true;

        Debug.Log("[MatchCardsController] Minigame started.");

        //generate cards?
        //fill in positions

        //for every card: pick a position. if it's taken, reroll. if not, claim the position

        // TODO

        GetCards();
        AddListeners();
        AddCardFronts();
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
        
        if (!firstGuess) {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGuessCard = cardFronts[firstGuessIndex].name;
            cards[firstGuessIndex].image.sprite = cardFronts[firstGuessIndex];
        } else if (!secondGuess) {
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondGuessCard = cardFronts[secondGuessIndex].name;
            cards[secondGuessIndex].image.sprite = cardFronts[secondGuessIndex];

            if (firstGuessCard == secondGuessCard) {
                Debug.Log("yay");
            } else {
                Debug.Log("nay");
            }

        }

            Debug.Log("haha jonathan you are clicking " + name);
    }

    private void Update()
    {
        if (!_running) return;

        // TODO

        Finish();
    }

    
    private void Finish()
    {
        _running = false;
        
        // TODO

        Debug.Log("[MatchCardsController] Completed with [stats here].");
        ReturnToGame(bonus: minigameScore, penalty: 0); // temp
    }
}

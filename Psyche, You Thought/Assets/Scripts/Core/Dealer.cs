using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the answer-card deck: shuffle, deal, and discard.
/// Corresponds to the "Dealer" swim lane.
/// </summary>
public class Dealer
{
    private List<AnswerCard> _deck = new List<AnswerCard>();
    private List<AnswerCard> _discard = new List<AnswerCard>();

    public int DeckCount => _deck.Count;
    public int HandSize  { get; set; } = 5;

    public Dealer(List<AnswerCard> fullDeck)
    {
        _deck = new List<AnswerCard>(fullDeck);
        Shuffle();
    }

    public void Shuffle()
    {
        for (int i = _deck.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            AnswerCard tmp = _deck[i];
            _deck[i] = _deck[j];
            _deck[j] = tmp;
        }
        Debug.Log($"[Dealer] Shuffled. {_deck.Count} cards remaining.");
    }

    /// <summary>Deal HandSize cards to every player. (Swim lane: "Deal cards to all players")</summary>
    public void DealCardsToAllPlayers(List<Player> players)
    {
        foreach (Player p in players)
            DealCardsToPlayer(p, HandSize);
    }

    public void DealCardsToPlayer(Player player, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_deck.Count == 0) Reshuffle();
            if (_deck.Count == 0)
            {
                Debug.LogWarning("[Dealer] Deck and discard both exhausted.");
                return;
            }
            if (player.CurrentHand.Count == HandSize) return;
            
            AnswerCard card = _deck[0];
            _deck.RemoveAt(0);
            player.DrawCard(card);
        }
    }

    /// <summary>Refill a player's hand back to HandSize after they play a card.</summary>
    public void TopUpHand(Player player)
    {
        int deficit = HandSize - player.CurrentHand.Count;
        if (deficit > 0) DealCardsToPlayer(player, deficit);
    }

    public void Discard(AnswerCard card)
    {
        if (card != null) _discard.Add(card);
    }

    private void Reshuffle()
    {
        Debug.Log("[Dealer] Reshuffling discard into deck.");
        _deck.AddRange(_discard);
        _discard.Clear();
        Shuffle();
    }
}

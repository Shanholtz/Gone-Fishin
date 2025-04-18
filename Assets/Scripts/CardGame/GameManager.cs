using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public PlayerManager PlayerHand;
    public AIManager aiHand;
    public StatManager stats;
    public float displayTime = 5f;

    public void Match(List<Card> hand)
    {
        StartCoroutine(ProcessMatching(hand));
    }

    private IEnumerator ProcessMatching(List<Card> hand)
    {
        List<Card> CardsToRemove = GetMatchingPairs(hand);

        foreach (Card card in CardsToRemove)
        {
            card.FlipCard(true);
        }

        foreach (Card card in CardsToRemove)
        {
            if (hand == PlayerHand.hand)
            {
                stats.ChangeStat(card.suit, card.rank, "player");
            }
            if (hand == aiHand.hand)
            {
                stats.ChangeStat(card.suit, card.rank, "ai");
            }
            
        }

        yield return new WaitForSeconds(displayTime); // Wait for display time before removal

        foreach (Card card in CardsToRemove)
        {
            hand.Remove(card);
            Destroy(card.gameObject);
        }

        yield return new WaitForEndOfFrame(); // Ensure objects are destroyed before repositioning

        PlayerHand.PositionCards();
        aiHand.PositionCards();

        if (PlayerHand.hand.Count == 0)
        {
            PlayerHand.DrawHand();
        }

        if (aiHand.hand.Count == 0)
        {
            aiHand.DrawHand();
        }
    }

    private List<Card> GetMatchingPairs(List<Card> cardList)
    {
        HashSet<Card> cardsToRemove = new HashSet<Card>();
        HashSet<int> countedRanks = new HashSet<int>(); // Track counted ranks

        for (int i = 0; i < cardList.Count; i++)
        {
            for (int j = i + 1; j < cardList.Count; j++)
            {
                if (cardList[i].rank == cardList[j].rank && cardList[i].suit != cardList[j].suit)
                {
                    if (!countedRanks.Contains(cardList[i].rank)) // Only count this rank once
                    {
                        Debug.Log($"{(cardList == PlayerHand.hand ? "Player" : "AI")} has a Pair of: {cardList[i].rank}'s!");
                        
                        cardsToRemove.Add(cardList[i]);
                        cardsToRemove.Add(cardList[j]);

                        if (cardList == PlayerHand.hand) PlayerHand.pairs++;
                        if (cardList == aiHand.hand) aiHand.pairs++;

                        countedRanks.Add(cardList[i].rank); // Mark this rank as counted
                    }
                }
            }
        }
        return new List<Card>(cardsToRemove);
    }
}

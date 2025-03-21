using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerManager PlayerHand;
    public AIManager aiHand;
    public float displayTime = 5f;
    public bool Changeturn = true;

    public void Match()
    {
        StartCoroutine(ProcessMatching());
    }

    private IEnumerator ProcessMatching()
    {
        List<Card> playerCardsToRemove = GetMatchingPairs(PlayerHand.hand);
        List<Card> aiCardsToRemove = GetMatchingPairs(aiHand.hand);

        foreach (Card card in playerCardsToRemove)
        {
            card.FlipCard(true);
        }

        foreach (Card card in aiCardsToRemove)
        {
            card.FlipCard(true);
        }

        yield return new WaitForSeconds(displayTime); // Wait for display time before removal

        foreach (Card card in playerCardsToRemove)
        {
            PlayerHand.hand.Remove(card);
            Destroy(card.gameObject);
        }

        foreach (Card card in aiCardsToRemove)
        {
            aiHand.hand.Remove(card);
            Destroy(card.gameObject);
        }

        yield return new WaitForEndOfFrame(); // Ensure objects are destroyed before repositioning

        PlayerHand.PositionCards();
        aiHand.PositionCards();

        ChangeTurn();
    }

    private List<Card> GetMatchingPairs(List<Card> cardList)
    {
        HashSet<Card> cardsToRemove = new HashSet<Card>();

        for (int i = 0; i < cardList.Count; i++)
        {
            for (int j = i + 1; j < cardList.Count; j++) // Start from i+1 to avoid duplicate checks
            {
                if (cardList[i].rank == cardList[j].rank && cardList[i].suit != cardList[j].suit)
                {
                    Debug.Log($"{(cardList == PlayerHand.hand ? "Player" : "AI")} has a Pair of: {cardList[i].rank}'s!");

                    cardsToRemove.Add(cardList[i]);
                    cardsToRemove.Add(cardList[j]);

                    if (cardList == PlayerHand.hand)
                    {
                        PlayerHand.pairs++;
                    }
                    if (cardList == aiHand.hand)
                    {
                        aiHand.pairs++;
                    }
                }
            }
        }

        return new List<Card>(cardsToRemove);
    }

    private void ChangeTurn()
    {
        Changeturn = !Changeturn;
    }
}

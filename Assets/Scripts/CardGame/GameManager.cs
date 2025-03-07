using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HandManager PlayerHand;
    public float displayTime = 5f;
    private int handCount;

    public void Match()
    {
        List<Card> cardsToRemove = new List<Card>();

        handCount = PlayerHand.playerCards.Count;

        for(int i = 0; i < handCount; i++)
        {
            Card card1 = PlayerHand.playerCards[i];

            for (int j=0; j<handCount; j++)
            {
                Card card2 = PlayerHand.playerCards[j];
                if (card1.rank == card2.rank && card1.suit != card2.suit)
                {
                    Debug.Log($"You Have a Pair of: {card1.rank}'s!" );

                    cardsToRemove.Add(card1);
                    cardsToRemove.Add(card2);
                }
            }
        }

        foreach (Card card in cardsToRemove)
        {
            PlayerHand.playerCards.Remove(card);
            Destroy(card.gameObject, displayTime);
        }

        StartCoroutine(Delay(displayTime));
    }

    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerHand.PositionCards();
    }
}

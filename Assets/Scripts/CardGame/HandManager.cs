using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public GameManager game;
    public DeckManager deckManager;
    public List<Card> hand = new List<Card>();
    public int startingHandSize = 5;
    public float spacing = 1.5f; // Space between cards
    protected virtual float yOffset => 4f;

    public int pairs = 0;

    protected virtual void Awake()
    {
        deckManager.onDeckReady += DrawAndPositionHand;
    }

    protected void DrawAndPositionHand()
    {
        DrawHand();
    }

    public virtual void AddCard()
    {
        Card drawnCard = deckManager.DrawCard(transform);
        if (drawnCard != null)
        {
            hand.Add(drawnCard);
            drawnCard.gameObject.SetActive(true);
            drawnCard.FlipCard(true); // Show face-up
        }
        
        PositionCards();
        game.Match();

    }

    public virtual void DrawHand()
    {
        for (int i = 0; i < startingHandSize; i++)
        {
            Card drawnCard = deckManager.DrawCard(transform);
            if (drawnCard != null)
            {
                hand.Add(drawnCard);
                drawnCard.gameObject.SetActive(true);
                drawnCard.FlipCard(true); // Show face-up
            }
            
            PositionCards();
        }
        game.Match();
    }

    public virtual void PositionCards()
    {
        float startX = -(spacing * (hand.Count- 1)) / 2; // Center the hand

        for (int i = 0; i < hand.Count; i++)
        {
            Vector3 newPosition = new Vector3(startX + (i * spacing), yOffset, 0);
            hand[i].SetPosition(newPosition);
        }
    }

    protected virtual void OnDestroy()
    {
        deckManager.onDeckReady -= DrawAndPositionHand; // Unsubscribe when destroyed
    }
}

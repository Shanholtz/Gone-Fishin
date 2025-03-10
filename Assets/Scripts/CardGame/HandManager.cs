using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameManager game;
    public List<Card> playerCards = new List<Card>();
    public int startingHandSize = 5;
    public float spacing = 1.25f; // Space between cards
    public float yOffset = -4f; // Adjust based on screen size

    public bool isTurn;


    void Update()
    {
        isTurn = game.Changeturn;

        if (isTurn)
        {
            enabled = true;
        }

        if (!isTurn)
        {
            enabled = false;
        }
    }
    void Awake()
    {
        deckManager.onDeckReady += DrawAndPositionHand;
    }

    void DrawAndPositionHand()
    {
        DrawHand();
    }

    public void AddCard()
    {
        Card drawnCard = deckManager.DrawCard(transform);
        if (drawnCard != null)
        {
            playerCards.Add(drawnCard);
            drawnCard.gameObject.SetActive(true);
            drawnCard.FlipCard(true); // Show face-up
        }

        PositionCards();
        game.Match();

    }

    void DrawHand()
    {
        for (int i = 0; i < startingHandSize; i++)
        {
            AddCard();
        }
    }

    public void PositionCards()
    {
        float startX = -(spacing * (playerCards.Count- 1)) / 2; // Center the hand

        for (int i = 0; i < playerCards.Count; i++)
        {
            Vector3 newPosition = new Vector3(startX + (i * spacing), yOffset, 0);
            playerCards[i].SetPosition(newPosition);
        }
    }

     void OnDestroy()
    {
        deckManager.onDeckReady -= DrawAndPositionHand; // Unsubscribe when destroyed
    }
}

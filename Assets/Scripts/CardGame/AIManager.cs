using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameManager game;
    public List<Card> AICards = new List<Card>();
    public int startingHandSize = 5;
    public float spacing = 1.25f; // Space between cards
    public float yOffset = 4f; // Adjust based on screen size
    public bool isTurn;

    void Update()
        {
            isTurn = game.Changeturn;

            if (isTurn)
            {
                enabled = false;
            }

            if (!isTurn)
            {
                enabled = true;
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
            AICards.Add(drawnCard);
            drawnCard.gameObject.SetActive(true);
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
        float startX = -(spacing * (AICards.Count- 1)) / 2; // Center the hand

        for (int i = 0; i < AICards.Count; i++)
        {
            Vector3 newPosition = new Vector3(startX + (i * spacing), yOffset, 0);
            AICards[i].SetPosition(newPosition);
        }
    }

     void OnDestroy()
    {
        deckManager.onDeckReady -= DrawAndPositionHand; // Unsubscribe when destroyed
    }
}

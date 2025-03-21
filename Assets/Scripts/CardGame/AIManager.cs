using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class AIManager : HandManager
{
    public TurnManager turnManager;
    public PlayerManager playerHand;
    public TextMeshProUGUI AIPairs;
    protected override float yOffset => 4f; // Adjust based on screen size
    public bool isTurn;

    protected override void Awake()
    {
        base.Awake();
    }

    public void AIRequestMatch()
    {
        if (hand.Count == 0)
        {
            Debug.Log("AI has no cards to request with.");
            AddCard();
            return;
        }

        // AI randomly selects a card to request
        Card selectedCard = hand[Random.Range(0, hand.Count)];
        Debug.Log($"AI is asking for {selectedCard.rank}");

        foreach (Card playerCard in playerHand.hand)
        {
            if (playerCard.rank == selectedCard.rank)
            {
                playerHand.hand.Remove(playerCard);
                hand.Add(playerCard);
                game.Match();
            }
        }

        turnManager.SwapTurn();
    }

    public override void AddCard()
    {
        Card drawnCard = deckManager.DrawCard(transform);
        if (drawnCard != null)
        {
            hand.Add(drawnCard);
            drawnCard.gameObject.SetActive(true);
            drawnCard.FlipCard(true); // Player sees their card

            PositionCards();
            game.Match();
        }
    }

    void Update()
    {
        AIPairs.text = pairs.ToString();
    }
}

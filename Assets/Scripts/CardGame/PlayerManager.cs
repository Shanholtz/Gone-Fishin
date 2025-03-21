using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : HandManager
{
    public TurnManager turnManager;
    public AIManager aiHand;
    public TextMeshProUGUI PlayerPairs;
    private Card selectedCard;
    protected override float yOffset => -4f; // Adjust based on screen size
    public bool isTurn;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SelectCard(Card card)
    {
        if (selectedCard == card) // Deselect if clicked again
        {
            selectedCard = null;
            Debug.Log("Deselected card.");
        }
        else
        {
            selectedCard = card;
            Debug.Log($"Selected {selectedCard.rank} to ask AI.");
            RequestMatch();
        }
    }

    public void RequestMatch()
    {
        if (selectedCard == null)
        {
            Debug.Log("No card selected!");
            return;
        }

        foreach (Card aiCard in aiHand.hand)
        {
            if (aiCard.rank == selectedCard.rank)
            {
                aiHand.hand.Remove(aiCard);
                hand.Add(aiCard);
                game.Match();
            }
        }
    }

    public override void AddCard()
    {
        base.AddCard();
    }

    void Update()
    {
        PlayerPairs.text = pairs.ToString();
    }
}

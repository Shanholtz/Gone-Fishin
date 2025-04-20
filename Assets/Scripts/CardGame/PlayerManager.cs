using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : HandManager
{
    public TurnManager turnManager;
    public AIManager aiHand;
    public TextMeshProUGUI PlayerPairs;
    public TextMeshProUGUI red;
    public TextMeshProUGUI blue;
    public SceneManager sceneManager;
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
            selectedCard.transform.position = selectedCard.assignedPosition;
            selectedCard = null;
            Debug.Log("Deselected card.");
        }
        if (selectedCard != card && selectedCard != null)
        {
            selectedCard.transform.position = selectedCard.assignedPosition;
            selectedCard.isSelected = false;
            selectedCard = card;
            Debug.Log($"Selected {selectedCard.rank} to ask AI.");
        }
        else
        {
            if (hand.Contains(card))
            {
                selectedCard = card;
                selectedCard.isSelected = true;
                Debug.Log($"Selected {selectedCard.rank} to ask AI."); 
            }
            
        }
    }

    public void RequestMatch()
    {
        if (selectedCard == null)
        {
            Debug.Log("No card selected!");
            return;
        }

        if (!isTurn)
        {
            Debug.Log("Not your turn!");
            return;
        }

        List<Card> matchingCards = new List<Card>();

        foreach (Card aiCard in aiHand.hand)
        {
            if (aiCard.rank == selectedCard.rank)
            {
                matchingCards.Add(aiCard);
                aiCard.transform.position = aiCard.assignedPosition - aiCard.hoverOffset;
            }
        }

        if (matchingCards.Count > 0)
        {
            red.text = "That was easy!";
            blue.text = "I'm sure it was...";

            foreach (Card card in matchingCards)
            {
                aiHand.hand.Remove(card);
                hand.Add(card);
            }

            game.Match(hand); // Process matching effects
            turnManager.SwapTurn();
        }
        else
        {
            blue.text = "Go Fish, kid";
            red.text = "WHAAAAAAT!";
            selectedCard.isSelected = false;
            selectedCard = null;

            //AddCard();
            StartCoroutine(sceneChange());
        }

        selectedCard = null;
    }

    IEnumerator sceneChange()
    {
        yield return new WaitForSeconds(3f);
        sceneManager.ChangeScene();
    }

    public override void AddCard()
    {
        base.AddCard();
    }
}

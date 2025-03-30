using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class AIManager : HandManager
{
    public TurnManager turnManager;
    public PlayerManager playerHand;
    public TextMeshProUGUI AIPairs;
    public SceneManager sceneManager;
    protected override float yOffset => 4f; // Adjust based on screen size
    public bool isTurn;

    protected override void Awake()
    {
        base.Awake();
    }

    public void AIRequestMatch()
    {
        if(isTurn)
        {
            StartCoroutine(AIThinkAndRequest());
        }
    }

    public IEnumerator AIThinkAndRequest()
    {
        yield return new WaitForSeconds (5f);

        if (hand.Count == 0)
        {
            Debug.Log("AI has no cards to request with.");
            AddCard();
            turnManager.SwapTurn();
            yield break;
        }

        // AI randomly selects a card to request
        Card selectedCard = hand[Random.Range(0, hand.Count)];
        Debug.Log($"AI is asking for {selectedCard.rank}");

        List<Card> matchingCards = new List<Card>();

        foreach (Card playerCard in playerHand.hand)
        {
            if (playerCard.rank == selectedCard.rank)
            {
                matchingCards.Add(playerCard);
                playerCard.transform.position = playerCard.assignedPosition + playerCard.hoverOffset;
            }
        }

        if (matchingCards.Count > 0)
        {
            Debug.Log("AI found a match! Taking the cards.");

            foreach (Card card in matchingCards)
            {
                playerHand.hand.Remove(card);
                hand.Add(card);
            }

            game.Match(); // Process matching effects
            
            yield return new WaitForSeconds(2f);
            turnManager.SwapTurn();
        }
        else
        {
            Debug.Log("No match found, AI draws a card.");
            AddCard();

            StartCoroutine(sceneChange());
            
        }
    }

    IEnumerator sceneChange()
    {
        yield return new WaitForSeconds(6f);
        sceneManager.ChangeScene();
    }

    public override void AddCard()
    {
        Card drawnCard = deckManager.DrawCard(transform);
        if (drawnCard != null)
        {
            hand.Add(drawnCard);
            drawnCard.gameObject.SetActive(true);
            drawnCard.FlipCard(false); 

            PositionCards();
            game.Match();
        }
    }

    public override void DrawHand()
    {
        for (int i = 0; i < startingHandSize; i++)
        {
            Card drawnCard = deckManager.DrawCard(transform);
            if (drawnCard != null)
            {
                hand.Add(drawnCard);
                drawnCard.gameObject.SetActive(true);
                drawnCard.FlipCard(false); // Show face-down
            }
            
            PositionCards();
        }
        game.Match();
    }

    void Update()
    {
        AIPairs.text = pairs.ToString();
    }
}

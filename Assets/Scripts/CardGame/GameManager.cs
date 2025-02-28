using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public DeckManager deckManager;
    public List<Card> playerHand = new List<Card>();
    public List<Card> opponentHand = new List<Card>();
    
    // UI Hand Panels
    public Transform playerHandPanel;
    public Transform opponentHandPanel;

    void Start()
    {
        DealInitialCards();
    }

    void DealInitialCards()
    {
        for (int i = 0; i < 5; i++) // Deal 5 cards to each player
        {
            DrawCard(playerHand, playerHandPanel);
            DrawCard(opponentHand, opponentHandPanel);
        }
    }

    public void DrawCard(List<Card> hand, Transform panel = null)
    {
        if (deckManager.deck.Count > 0)
        {
            Card drawnCard = deckManager.deck[0];
            deckManager.deck.RemoveAt(0);
            hand.Add(drawnCard);

            drawnCard.gameObject.SetActive(true);
            drawnCard.transform.SetParent(panel, false);
            drawnCard.transform.localScale = Vector2.one;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

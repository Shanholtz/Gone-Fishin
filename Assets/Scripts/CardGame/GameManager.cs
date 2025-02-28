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
        Debug.Log($"Cards in Deck: {deckManager.deck.Count}");
        DealInitialCards();
    }

    void DealInitialCards()
    {
        for (int i = 0; i < 5; i++) // Deal 5 cards to each player
        {
            DrawCard(playerHand);
            DrawCard(opponentHand);
        }
    }

    public void DrawCard(List<Card> hand)
    {
        
        if (deckManager.deck.Count > 0)
        {
            Card drawnCard = deckManager.deck[0];
            deckManager.deck.RemoveAt(0);
            hand.Add(drawnCard);

            drawnCard.gameObject.SetActive(true);
            drawnCard.transform.SetParent(playerHandPanel, false);
            drawnCard.transform.localScale = Vector3.one;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

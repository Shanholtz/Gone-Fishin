using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public List<Card> deck = new List<Card>();

    public Sprite[] cardSprites; // Assign in Inspector
    public Sprite cardBack;

    private string[] suits = { "Clubs", "Diamonds", "Hearts", "Spades" };
    private string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"  };

    void Start()
    {
        GenerateDeck();
        ShuffleDeck();
    }

    void GenerateDeck()
    {
        int spriteIndex = 0;
        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                GameObject newCard = Instantiate(cardPrefab);
                Card card = newCard.GetComponent<Card>();
                card.rank = rank;
                card.suit = suit;
                card.frontSprite = cardSprites[spriteIndex++];
                card.backSprite = cardBack;
                deck.Add(card);
                newCard.SetActive(false); // Hide initially
            }
        }
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

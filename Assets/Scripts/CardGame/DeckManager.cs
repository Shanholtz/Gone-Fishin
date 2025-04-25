using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public List<Card> deck = new List<Card>();

    public Sprite[] cardSprites; // Assign in Inspector
    public Sprite cardBack;
    public SceneManager scene;

    private string[] Testsuits = { "Hook", "Rod"};
    private int[] Testranks = { 1, 2, 3, 4, 5, 6};

    private string[] suits = { "Hook", "Rod", "String", "Bait" };
    private int[] ranks = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13  };

    public event Action onDeckReady;

    void Start()
    {
        StartCoroutine(InitializeDeck());
    }

    IEnumerator InitializeDeck()
    {
        GenerateDeck();
        yield return null;
        ShuffleDeck();

        onDeckReady?.Invoke();
    }

    void GenerateDeck()
    {
        int spriteIndex = 0;
        foreach (string suit in suits)
        {
            foreach (int rank in ranks)
            {
                GameObject newCard = Instantiate(cardPrefab);
               
                Card card = newCard.GetComponent<Card>();
                card.rank = rank;
                card.suit = suit;
                card.frontSprite = cardSprites[spriteIndex++];
                card.backSprite = cardBack;
                newCard.gameObject.name = "Card_" + card.frontSprite.name;
                newCard.transform.SetParent(transform);
                deck.Add(card);
            }
        }
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = UnityEngine.Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public Card DrawCard(Transform hand)
    {
        if (deck.Count == 0)
        {
            Debug.LogWarning("Deck is empty!");
            if (hand.childCount == 0)
            {
                scene.ChangeScene();
            }
            return null;
        }

        Card drawnCard = deck[0];
        drawnCard.transform.SetParent(hand);
        deck.RemoveAt(0);
        return drawnCard;
    }

    public void PlaceCard(Card card)
    {
        card.transform.SetParent(transform);
        deck.Insert(0, card);
    }
}

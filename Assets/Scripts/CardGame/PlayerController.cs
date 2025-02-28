using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AskForCard(string rank)
    {
        bool cardFound = false;
        List<Card> opponentHand = gameManager.opponentHand;
        List<Card> playerHand = gameManager.playerHand;

        for (int i = opponentHand.Count - 1; i >= 0; i--)
        {
            if (opponentHand[i].rank == rank)
            {
                Card card = opponentHand[i];
                opponentHand.RemoveAt(i);
                playerHand.Add(card);
                cardFound = true;
            }
        }

        if (!cardFound)
        {
            Debug.Log("Go Fish!");
            gameManager.DrawCard(playerHand);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public PlayerManager playerHand;
    public AIManager aiHand;

    public bool isPlayerTurn = true; // Player starts first

    void Start()
    {
        SwapTurn();
    }

    public void SwapTurn()
    {
        isPlayerTurn = !isPlayerTurn; // Toggle turn

        if (isPlayerTurn)
        {
            Debug.Log("Player's Turn");
            playerHand.isTurn = true;
            aiHand.isTurn = false;
        }
        else
        {
            Debug.Log("AI's Turn");
            playerHand.isTurn = false;
            aiHand.isTurn = true;
        }
    }
}

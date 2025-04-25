using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{

    public PlayerManager playerHand;
    public AIManager aiHand;
    public GameObject request;

    public bool isPlayerTurn = false; // Player starts first

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
            request.SetActive(true);
        }
        else
        {
            Debug.Log("AI's Turn");
            playerHand.isTurn = false;
            aiHand.isTurn = true;
            request.SetActive(false);
            aiHand.AIRequestMatch();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Hook : MonoBehaviour
{
    public GameObject gameArea;
    public StatManager stat;
    public TurnManager turn;

    public float hookedRadius = 0.25f;
    public bool isFishHooked = false; // New flag to track if a fish is hooked
    public float attractionRadius;

    public float AttractionRadius
    {
        get
        {
            float radius = stat.playerStats.radius;
            if (!turn.isPlayerTurn)
            {
                radius = stat.aiStats.radius;
            }

            return radius;
        }
    }

    public void attraction()
    {
        if (turn.isPlayerTurn)
        {
            attractionRadius = stat.playerStats.radius;
        }

        if (!turn.isPlayerTurn)
        {
            attractionRadius = stat.aiStats.radius;
        }
    }
    
}
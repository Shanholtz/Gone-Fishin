using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class Stats
{
    [Tooltip("Total number of fish that can be generated")]
    public float limit = 3f;
    public float radius = 2.5f;
    public float timer = 5f;
    public float rareTier;
    public int score = 0;
}

public class StatManager : MonoBehaviour
{
    public TextMeshProUGUI redScore;
    public TextMeshProUGUI blueScore;
    public Catch time;
    public SCR_FishSpawner spawner;
    public SCR_Hook hookRadius;
    public TurnManager turn;

    [Header("Stat Values")]
    // stats
    public Stats playerStats;
    public Stats aiStats;

    void Update()
    {
        redScore.text = $"{playerStats.score}";
        blueScore.text = $"{aiStats.score}";
    }

    public void ChangeStat(string suit, float rank, string player)
    {
        Stats stats = playerStats;

        if (player == "ai")
        {
            stats = aiStats;
        }

        if (suit == "Hook")
        {
            stats.radius += (float)(rank / 5.2); // a 13 rank card will double the current attraction radius of 2.5
        }

        if (suit == "Rod")
        {
            if (rank == 1) // if ace, just adds 1 fish
            {
                stats.limit += (int)(rank);
            }
            else // anything else
            {
                stats.limit += (int)(rank / 2.6); // a 13 rank card will add 5 fish to the pond
            }
        }

        if (suit == "String")
        {
            stats.timer += (float)(rank / 2.6); // a 13 rank card will double the time of 5 seconds to 10
        }

        if (suit == "Bait")
        {
            if (rank >= 0 && rank <= 3)
            {
                spawner.playerSpawnTable = spawner.GetTier1Table();
                stats.rareTier = 1f; // Tier 1 rarity
            }
            else if (rank >= 4 && rank <= 6)
            {
                spawner.playerSpawnTable = spawner.GetTier2Table();
                stats.rareTier = 2f; // Tier 2 rarity
            }
            else if (rank >= 7 && rank <= 10)
            {
                spawner.playerSpawnTable = spawner.GetTier3Table();
                stats.rareTier = 3f; // Tier 3 rarity
            }
            else if (rank >= 11 && rank <= 13)
            {
                spawner.playerSpawnTable = spawner.GetTier4Table();
                stats.rareTier = 4f; // Tier 4 rarity
            }
            else if (rank >= 14)
            {
                spawner.playerSpawnTable = spawner.GetTier5Table(); // anything above the 13 card rank, give the best table
                stats.rareTier = 5f; // Tier 5 rarity
            }

            spawner.ResetFish();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public Catch time;
    public SCR_FishSpawner spawner;
    public SCR_Hook hookRadius;
    public TurnManager turn;

    // stats
    public float playerLimit = 3f;
    public float playerRadius= 2.5f;
    public float playerTimer = 5f;
    public float playerRare;

    public float aiLimit = 3f;
    public float aiRadius = 2.5f;
    public float aiTimer = 5f;
    public float aiRare;

    public void ChangeStat(string suit, float rank)
    {
        if(turn.isPlayerTurn)
        {
            if (suit == "Hook")
            {
                playerRadius += (float)(rank/5.2); // a 13 rank card will double the current attraction radius of 2.5
            }

            if (suit == "Rod")
            {
                if (rank == 1) // if ace, just adds 1 fish
                {
                    playerLimit += (int)(rank);
                }
                else // anything else
                {
                    playerLimit += (int)(rank/2.6); // a 13 rank card will add 5 fish to the pond
                }
            }

            if (suit == "String")
            {
                playerTimer += (float)(rank/2.6); // a 13 rank card will double the time of 5 seconds to 10
            }

            if (suit == "Bait")
            {
                if (rank >= 1 && rank <= 3)
                    spawner.playerSpawnTable = spawner.GetTier1Table();
                else if (rank >= 4 && rank <= 6)
                    spawner.playerSpawnTable = spawner.GetTier2Table();
                else if (rank >= 7 && rank <= 10)
                    spawner.playerSpawnTable = spawner.GetTier3Table();
                else if (rank >= 11 && rank <= 13)
                    spawner.playerSpawnTable = spawner.GetTier4Table();
                else if (rank >= 14)
                    spawner.playerSpawnTable = spawner.GetTier5Table(); // anything above the 13 card rank, give the best table
            

                spawner.ResetFish();
            }

            if(!turn.isPlayerTurn)
            {
                if (suit == "Hook")
                {
                    aiRadius += (float)(rank/5.2); // a 13 rank card will double the current attraction radius of 2.5
                }

                if (suit == "Rod")
                {
                    if (rank == 1) // if ace, just adds 1 fish
                    {
                        aiLimit += (int)(rank);
                    }
                    else // anything else
                    {
                        aiLimit += (int)(rank/2.6); // a 13 rank card will add 5 fish to the pond
                    }
                }

                if (suit == "String")
                {
                    aiTimer += (float)(rank/2.6); // a 13 rank card will double the time of 5 seconds to 10
                }

                if (suit == "Bait")
                {
                    if (rank >= 1 && rank <= 3)
                        spawner.aiSpawnTable = spawner.GetTier1Table();
                    else if (rank >= 4 && rank <= 6)
                        spawner.aiSpawnTable = spawner.GetTier2Table();
                    else if (rank >= 7 && rank <= 10)
                        spawner.aiSpawnTable = spawner.GetTier3Table();
                    else if (rank >= 11 && rank <= 13)
                        spawner.aiSpawnTable = spawner.GetTier4Table();
                    else if (rank >= 14)
                        spawner.aiSpawnTable = spawner.GetTier5Table();// anything above the 13 card rank, give the best table


                    spawner.ResetFish();
                }
            }
        }
    }
}

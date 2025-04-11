using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public Catch time;
    public SCR_FishSpawner spawner;
    public SCR_Hook hookRadius;

    public void ChangeStat(string suit, float rank)
    {
        if (suit == "Hook")
        {
            hookRadius.attractionRadius += (float)(rank/5.2); // a 13 rank card will double the current attraction radius of 2.5
        }

        if (suit == "Rod")
        {
            if (rank == 1) // if ace, just adds 1 fish
            {
                spawner.fishLimit += (int)(rank);
            }
            else // anything else
            {
                spawner.fishLimit += (int)(rank/2.6); // a 13 rank card will add 5 fish to the pond
            }
        }

        if (suit == "String")
        {
            time.timerDuration += (float)(rank/2.6); // a 13 rank card will double the time of 5 seconds to 10
        }

        if (suit == "Bait")
        {
            if (rank >= 1 && rank <= 3)
            {
                spawner.spawnTable = spawner.GetTier1Table();
            }
            else if (rank >= 4 && rank <= 6)
            {
                spawner.spawnTable = spawner.GetTier2Table();
            }
            else if (rank >= 7 && rank <= 10)
            {
                spawner.spawnTable = spawner.GetTier3Table();
            }
            else if (rank >= 11 && rank <= 13)
            {
                spawner.spawnTable = spawner.GetTier4Table();
            }

            spawner.ResetFish();
        }
    }
}

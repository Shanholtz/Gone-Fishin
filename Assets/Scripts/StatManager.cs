using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public Catch time;
    public SCR_FishSpawner spawner;
    public SCR_Hook hookRadius;

    public void ChangeStat(string suit, int rank)
    {
        if (suit == "Hook")
        {
            hookRadius.attractionRadius += rank;
        }

        if (suit == "Rod")
        {
            spawner.fishLimit += rank;
        }

        if (suit == "String")
        {
            time.timerDuration += rank;
        }

        if (suit == "Bait")
        {
            spawner.rareFishCount += rank;
        }
    }
}

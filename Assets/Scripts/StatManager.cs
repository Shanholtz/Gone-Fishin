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
            Debug.Log("Hook has been matched");
        }

        if (suit == "Rod")
        {
            spawner.fishLimit += rank;
            Debug.Log("Rod has been matched");
        }

        if (suit == "String")
        {
            time.timerDuration += rank;
            Debug.Log("String has been matched");
        }

        if (suit == "Bait")
        {
            spawner.rareFishCount += rank;
            Debug.Log("Bait has been matched");
        }
    }
}

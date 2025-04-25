using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIStatButton : MonoBehaviour
{
    public GameObject aiStats;
    public TextMeshProUGUI stats;
    public StatManager Astats;

    public void showStats()
    {
        if (!aiStats.activeSelf)
        {
            aiStats.SetActive(true); 

            stats.text = $"Fish Amount: {Astats.aiStats.limit}\nFish Rarity: {Astats.aiStats.rareTier}\nHook Radius: {Astats.aiStats.radius}\nTimer Length: {Astats.aiStats.timer}";

        }
        
        else
        {
            aiStats.SetActive(false);
        }
    }
}

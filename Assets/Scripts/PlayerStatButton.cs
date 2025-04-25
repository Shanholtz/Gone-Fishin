using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatButton : MonoBehaviour
{
    public GameObject playerStats;
    public TextMeshProUGUI stats;
    public StatManager pstats;

    public void showStats()
    {
        if (!playerStats.activeSelf)
        {
            playerStats.SetActive(true); 

            stats.text = $"Fish Amount: {pstats.playerStats.limit}\nFish Rarity: {pstats.playerStats.rareTier}\nHook Radius: {pstats.playerStats.radius}\nTimer Length: {pstats.playerStats.timer}";

        }
        
        else
        {
            playerStats.SetActive(false);
        }
    }


}

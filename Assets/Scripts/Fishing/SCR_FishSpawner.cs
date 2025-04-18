using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class FishSpawnChance
{
    public int spriteIndex;  // Index of fish sprite
    public float chance;     // Percent out of 100
}

public class SCR_FishSpawner : MonoBehaviour
{
    public GameObject gameArea;
    public GameObject fishPrefab;
    public TurnManager turn;
    public StatManager stats;
    public List<FishSpawnChance> playerSpawnTable;
    public List<FishSpawnChance> aiSpawnTable;

    // List of fish sprites to choose from
    public Sprite[] fishSprites;

    // Controls amount of fish
    public int fishCount = 0;
    public float fishLimit = 3;
    public int fishPerFrame = 1;
    //public int rareFishCount = 0;

    // Controls game area
    public float spawnCircleRadius = 5.0f;
    public float gameBoundaryCircleRadius = 10.0f;

    // Control fish speed
    public float fastestSpeed = 12.0f;
    public float slowestSpeed = 0.75f;

    public bool fishHooked = false;


    public List<FishSpawnChance> spawnTable; // Active spawn table

    private List<FishSpawnChance> tier1SpawnTable;
    private List<FishSpawnChance> tier2SpawnTable;
    private List<FishSpawnChance> tier3SpawnTable;
    private List<FishSpawnChance> tier4SpawnTable;
    private List<FishSpawnChance> tier5SpawnTable;

    void Awake()
    {
        InitSpawnTables();

        // Set default spawn tables if they aren't already assigned
        if (playerSpawnTable == null || playerSpawnTable.Count == 0)
        {
            playerSpawnTable = GetTier1Table();
        }

        if (aiSpawnTable == null || aiSpawnTable.Count == 0)
        {
            aiSpawnTable = GetTier1Table();
        }

        spawnTable = tier1SpawnTable; // This will be overridden in InitialPopulation anyway
    }


// Start is called before the first frame update
void Start()
    {
        InitialPopulation();
    }

    void InitSpawnTables()
    {
        tier1SpawnTable = new List<FishSpawnChance>
        {
            new FishSpawnChance { spriteIndex = 1, chance = 50f }, // Bass
            new FishSpawnChance { spriteIndex = 2, chance = 50f }  // Can
        };

        tier2SpawnTable = new List<FishSpawnChance>
        {
            new FishSpawnChance { spriteIndex = 1, chance = 40f }, // Bass
            new FishSpawnChance { spriteIndex = 2, chance = 30f }, // Can
            new FishSpawnChance { spriteIndex = 3, chance = 30f }  // Shark
        };

        tier3SpawnTable = new List<FishSpawnChance>
        {
            new FishSpawnChance { spriteIndex = 1, chance = 35f }, // Bass
            new FishSpawnChance { spriteIndex = 3, chance = 30f }, // Shark
            new FishSpawnChance { spriteIndex = 4, chance = 25f }, // Angel
            new FishSpawnChance { spriteIndex = 0, chance = 10f }  // Golden
        };

        tier4SpawnTable = new List<FishSpawnChance>
        {
            new FishSpawnChance { spriteIndex = 0, chance = 20f }, // Golden
            new FishSpawnChance { spriteIndex = 3, chance = 32.5f }, // Shark
            new FishSpawnChance { spriteIndex = 4, chance = 32.5f }, // Angel
            new FishSpawnChance { spriteIndex = 5, chance = 10f }  // Fish Sticks
        };

        tier5SpawnTable = new List<FishSpawnChance>
        {
            new FishSpawnChance { spriteIndex = 0, chance = 50f }, // Golden
            new FishSpawnChance { spriteIndex = 3, chance = 20f }, // Shark
            new FishSpawnChance { spriteIndex = 4, chance = 20f }, // Angel
            new FishSpawnChance { spriteIndex = 5, chance = 10f }  // Fish Sticks
        };
    }

    public List<FishSpawnChance> GetTier1Table() => tier1SpawnTable;
    public List<FishSpawnChance> GetTier2Table() => tier2SpawnTable;
    public List<FishSpawnChance> GetTier3Table() => tier3SpawnTable;
    public List<FishSpawnChance> GetTier4Table() => tier4SpawnTable;
    public List<FishSpawnChance> GetTier5Table() => tier5SpawnTable;

    public void InitialPopulation()
    {
        // reset the fish count before spawning
        fishCount = 0;

        if (turn.isPlayerTurn)
        {
            fishLimit = stats.playerStats.limit;
            spawnTable = playerSpawnTable;

        }
        if (!turn.isPlayerTurn)
        {
            fishLimit = stats.aiStats.limit;
            spawnTable = aiSpawnTable;

        }
        // Spawns fish in random positions when the game starts.
        for (int i = 0; i < fishLimit; i++)
        {
            Vector3 position = GetRandomPosition(true);
            SCR_Fish fishScript = AddFish(position);
            fishScript.transform.Rotate(Vector3.forward * Random.Range(0.0f, 260f));
        }
    }

    Vector3 GetRandomPosition(bool withinCamera)
    {
        Vector3 position = Random.insideUnitCircle;

        // .normalized makes the fish spawn on the spawn radius edges
        if (!withinCamera)
        {
            position = position.normalized;
        }

        position *= spawnCircleRadius;
        position += gameArea.transform.position;

        return position;
    }

    SCR_Fish AddFish(Vector3 position)
    {
        fishCount++;
        GameObject newFish = Instantiate(fishPrefab, position, Quaternion.FromToRotation(Vector3.up, (gameArea.transform.position - position)), transform);
        SCR_Fish fishScript = newFish.GetComponent<SCR_Fish>();
        fishScript.fishSpawner = this;
        fishScript.gameArea = gameArea;
        SpriteRenderer spriteRenderer = newFish.GetComponent<SpriteRenderer>();

        if (fishSprites.Length > 0 && spriteRenderer != null && spawnTable != null)
        {
            float roll = Random.Range(0f, 100f);
            float cumulative = 0f;

            foreach (var entry in spawnTable)
            {
                cumulative += entry.chance;
                if (roll <= cumulative)
                {
                    spriteRenderer.sprite = fishSprites[entry.spriteIndex];
                    SetFishStats(fishScript, newFish, entry.spriteIndex);
                    break;
                }
            }
        }

        return fishScript;
    }

    void SetFishStats(SCR_Fish fishScript, GameObject fishObj, int spriteIndex)
    {
        switch (spriteIndex)
        {
            case 0: // Golden Fish
                fishScript.speed = fastestSpeed;
                fishObj.transform.localScale = Vector3.one * 0.75f;
                fishScript.scoreValue = 10;
                break;
            case 1: // Bass
                fishScript.speed = Random.Range(slowestSpeed, fastestSpeed);
                fishObj.transform.localScale = Vector3.one * Random.Range(1.0f, 1.5f);
                fishScript.scoreValue = 3;
                break;
            case 2: // Can
                fishScript.speed = slowestSpeed;
                fishObj.transform.localScale = Vector3.one * 0.75f;
                fishScript.scoreValue = 1;
                break;
            case 3: // Shark
                fishScript.speed = Random.Range(slowestSpeed, fastestSpeed);
                fishObj.transform.localScale = Vector3.one * 2f;
                fishScript.scoreValue = 5;
                break;
            case 4: // Angel
                fishScript.speed = Random.Range(slowestSpeed, fastestSpeed);
                fishObj.transform.localScale = Vector3.one * Random.Range(1.0f, 1.5f);
                fishScript.scoreValue = 7;
                break;
            case 5: // Fish Sticks
                fishScript.speed = fastestSpeed;
                fishObj.transform.localScale = Vector3.one;
                fishScript.scoreValue = 20;
                break;
        }
    }

    public void ResetFish()
    {
        // Destroy all fish
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        fishCount = 0;
        fishHooked = false;

        // Force update of the active spawn table and fish limit
        if (turn.isPlayerTurn)
        {
            fishLimit = stats.playerStats.limit;
            spawnTable = playerSpawnTable;
        }
        else
        {
            fishLimit = stats.aiStats.limit;
            spawnTable = aiSpawnTable;
        }
        // Resets rare fish count
        //rareFishCount = 0;

        // Respawn new fish
        InitialPopulation();
    }


}
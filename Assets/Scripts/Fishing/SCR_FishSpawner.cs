using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FishSpawner : MonoBehaviour
{
    public GameObject gameArea;
    public GameObject fishPrefab;

    // List of fish sprites to choose from
    public Sprite[] fishSprites;

    // Controls amount of fish
    public int fishCount = 0;
    public int fishLimit = 10;
    public int fishPerFrame = 1;
    public int rareFishCount = 0;

    // Controls game area
    public float spawnCircleRadius = 5.0f;
    public float gameBoundaryCircleRadius = 10.0f;

    // Control fish speed
    public float fastestSpeed = 12.0f;
    public float slowestSpeed = 0.75f;

    public bool fishHooked = false;

    // Start is called before the first frame update
    void Start()
    {
        InitialPopulation();
    }

    public void InitialPopulation()
    {
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

    // Adds fish, for now always just 1 golden fish, lower spawn rate for can fish.
    SCR_Fish AddFish(Vector3 position)
    {
        fishCount++;

        GameObject newFish = Instantiate(fishPrefab, position, Quaternion.FromToRotation(Vector3.up, (gameArea.transform.position - position)), transform);
        SCR_Fish fishScript = newFish.GetComponent<SCR_Fish>();
        fishScript.fishSpawner = this;
        fishScript.gameArea = gameArea;
        SpriteRenderer spriteRenderer = newFish.GetComponent<SpriteRenderer>();

        // Golden fish (rare) creation
        if (rareFishCount != 1) 
        {            
            rareFishCount++;
            spriteRenderer.sprite = fishSprites[0];
            fishScript.speed = fastestSpeed;
            float goldenFishSize = 0.75f;
            newFish.transform.localScale = new Vector3(goldenFishSize, goldenFishSize);

            return fishScript;
        }

        // Random between bass and can fish
        if (fishSprites.Length > 0 && spriteRenderer != null)
        {
            // Gives fish a random value to determine type
            int randFish = Random.Range(1, 101);

            // Bass stats
            if (randFish > 50 && randFish <= 100) // 50%
            {
                spriteRenderer.sprite = fishSprites[1];

                fishScript.speed = Random.Range(slowestSpeed, fastestSpeed);
                float randomSize = Random.Range(1.0f, 1.5f);
                newFish.transform.localScale = new Vector3(randomSize, randomSize);
                return fishScript;
            }
            // Can fish stats
            if (randFish <= 10) // 10%
            {
                spriteRenderer.sprite = fishSprites[2];
                float canFishSize = 0.75f;
                newFish.transform.localScale = new Vector3(canFishSize, canFishSize);

                fishScript.speed = slowestSpeed;
                return fishScript;
            }
            // Shark stats
            if (randFish > 10 && randFish <= 30) // 20%
            {
                spriteRenderer.sprite = fishSprites[3];

                fishScript.speed = Random.Range(slowestSpeed, fastestSpeed);
                float randomSize = 2f;
                newFish.transform.localScale = new Vector3(randomSize, randomSize);
                return fishScript;
            }
            // Angel Fish stats
            if (randFish > 30 && randFish <= 50) // 20%
            {
                spriteRenderer.sprite = fishSprites[4];

                fishScript.speed = Random.Range(slowestSpeed, fastestSpeed);
                float randomSize = Random.Range(1.0f, 1.5f);
                newFish.transform.localScale = new Vector3(randomSize, randomSize);
                return fishScript;
            }
            // Fish sticks Fish stats
            if (randFish == 101) // 20%
            {
                spriteRenderer.sprite = fishSprites[5];

                fishScript.speed = fastestSpeed;
                float randomSize = 1.0f;
                newFish.transform.localScale = new Vector3(randomSize, randomSize);
                return fishScript;
            }
        }


        return fishScript;
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

        // Resets rare fish count
        rareFishCount = 0;

        // Respawn new fish
        InitialPopulation();
    }


}
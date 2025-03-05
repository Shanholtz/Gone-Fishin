using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FishSpawner : MonoBehaviour
{
    public GameObject gameArea;
    public GameObject fishPrefab;

    // Controls amount of fish
    public int fishCount = 0;
    public int fishLimit = 10;
    public int fishPerFrame = 1;

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

    void InitialPopulation()
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

    // Adds fish
    SCR_Fish AddFish(Vector3 position)
    {
        fishCount++;
        GameObject newFish = Instantiate(fishPrefab, position, Quaternion.FromToRotation(Vector3.up, (gameArea.transform.position - position)), transform);
        SCR_Fish fishScript = newFish.GetComponent<SCR_Fish>();
        fishScript.fishSpawner = this;
        fishScript.gameArea = gameArea;
        fishScript.speed = Random.Range(slowestSpeed, fastestSpeed);

        return fishScript;
    }
    
}
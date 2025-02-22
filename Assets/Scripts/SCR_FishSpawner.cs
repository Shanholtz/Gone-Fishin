using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FishSpawner : MonoBehaviour
{
    public GameObject game_area;
    public GameObject fish_prefab;

    public int fish_count = 0;
    public int fish_limit = 10;
    public int fish_per_frame = 1;

    public float spawn_circle_radius = 80.0f;
    public float death_circle_radius = 90.0f;

    public float fastest_speed = 12.0f;
    public float slowest_speed = 0.75f;


    // Start is called before the first frame update
    void Start()
    {
        InitialPopulation();
    }

    // Update is called once per frame
    void Update()
    {
        MaintainPopulation();
    }

    void InitialPopulation()
    {
        for (int i = 0; i < fish_limit; i++) 
        {
            Vector3 position = GetRandomPosition(true);
            SCR_Fish fish_script = AddFish(position);
            fish_script.transform.Rotate(Vector3.forward * Random.Range(0.0f, 260f));
        }
    }

    void MaintainPopulation()
    {
        if (fish_count < fish_limit)
        {
            for (int i = 0; i < fish_per_frame; i++)
            {
                Vector3 position = GetRandomPosition(false);
                SCR_Fish fish_script = AddFish(position);

                // This makes the fish start swimming within a 45 degree angle of the center, removing it will make the fish swim towards the center.
                fish_script.transform.Rotate(Vector3.forward * Random.Range(-45.0f, 45.0f));
            }
        }
    }

    Vector3 GetRandomPosition(bool within_camera)
    {
        Vector3 position = Random.insideUnitCircle;

        // .normalized makes the fish spawn on the spawn radius edges
        if (within_camera == false)
        {
            position = position.normalized;
        }

        position *= spawn_circle_radius;
        position += game_area.transform.position;

        return position;
    }

    SCR_Fish AddFish(Vector3 position)
    {
        fish_count += 1;
        GameObject new_fish = Instantiate(
            fish_prefab, 
            position, 
            Quaternion.FromToRotation(Vector3.up, (game_area.transform.position-position)), 
            gameObject.transform
        );

        SCR_Fish fish_script = new_fish.GetComponent<SCR_Fish>();
        fish_script.fish_spawner = this;
        fish_script.game_area = game_area;
        fish_script.speed = Random.Range(slowest_speed, fastest_speed);

        return fish_script;

    }
}

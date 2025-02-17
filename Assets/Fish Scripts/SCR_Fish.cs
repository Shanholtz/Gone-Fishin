using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Fish : MonoBehaviour
{
    public SCR_FishSpawner fish_spawner;
    public GameObject game_area;

    public float speed;


    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += transform.up * (Time.deltaTime * speed);

        float distance = Vector3.Distance(transform.position, game_area.transform.position);
        if (distance > fish_spawner.death_circle_radius)
        {
            RemoveShip();
        }

    }

    void RemoveShip()
    {
        Destroy(gameObject);
        fish_spawner.fish_count -= 1;
    }


}

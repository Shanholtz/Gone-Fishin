using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Fish : MonoBehaviour
{
    private FishingCastController fishingCastController;
    public SCR_FishSpawner fish_spawner;
    public GameObject game_area;

    public float speed;

    public bool hookInSight;

    private SCR_Hook hookToFollow;
    private Coroutine changeDirectionCoroutine; // Track the coroutine

    void Start()
    {
        fishingCastController = FindObjectOfType<FishingCastController>();
        changeDirectionCoroutine = StartCoroutine(ChangeDirectionRoutine()); // Start the coroutine and store its reference

        //add action and event listners
        fishingCastController.OnHookCast += AssignHook;
        fishingCastController.OnFishCaught += DisperseFish;
    }

    /// <summary>
    /// Assigns current hook to follow.
    /// </summary>
    /// <param name="hook"></param>
    void AssignHook(SCR_Hook hook){
        hookToFollow = hook;
    }

    void DisperseFish(SCR_Fish fish)
    {
        if (fish != this)
        {
          hookInSight = false;  
        }
        
    }

    void OnDestroy(){
        fishingCastController.OnHookCast -= AssignHook;
        fishingCastController.OnFishCaught -= DisperseFish;
    }

    void Update()
    {
        if(fishingCastController._fishingStates == FishingCastController.FishingStates.Cast)
            CheckHookProximity();
            
        Move();
    }

    void Move()
    {
        if (hookInSight && hookToFollow != null)
        {
            Vector3 direction = (hookToFollow.transform.position - transform.position).normalized;
            transform.position += direction * (Time.deltaTime * speed);

            // Smoothly rotate towards the hook
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);

            // Stop the coroutine when moving towards the hook
            if (changeDirectionCoroutine != null)
            {
                StopCoroutine(changeDirectionCoroutine);
                changeDirectionCoroutine = null;
            }
        }
        else
        {
            transform.position += transform.up * (Time.deltaTime * speed);

            // Restart the coroutine if not already running
            if (changeDirectionCoroutine == null)
            {
                changeDirectionCoroutine = StartCoroutine(ChangeDirectionRoutine());
            }
        }

        // Check if the fish is outside the game area
        float distanceSqr = (transform.position - game_area.transform.position).sqrMagnitude;
        if (distanceSqr > fish_spawner.death_circle_radius * fish_spawner.death_circle_radius)
        {
            RemoveFish();
        }
    }

    void RemoveFish()
    {
        // Stop the coroutine if it's running
        if (changeDirectionCoroutine != null)
        {
            StopCoroutine(changeDirectionCoroutine);
        }
        //transform.Rotate(Vector3.forward * 180);
        Destroy(gameObject);
        fish_spawner.fish_count = Mathf.Max(0, fish_spawner.fish_count - 1); // Ensure fish_count doesn't go below zero
    }

    IEnumerator ChangeDirectionRoutine()
    {
        while (!hookInSight)
        {
            float waitTime = Random.Range(1f, 5f); // Random time between direction changes
            yield return new WaitForSeconds(waitTime);

            float randomAngle = Random.Range(-90f, 90f); // Adjust rotation range
            transform.Rotate(Vector3.forward * randomAngle);
        }
    }

    void CheckHookProximity()
    {
        if (hookToFollow != null)
        {
            float distanceToHook = Vector3.Distance(hookToFollow.transform.position, transform.position); // Use squared distance for performance

            hookInSight = distanceToHook <= hookToFollow.attractionRadius;

            if (distanceToHook <= hookToFollow.hookedRadius)
            {
                fishingCastController.CatchFish(this);
            }
        }
    }
}
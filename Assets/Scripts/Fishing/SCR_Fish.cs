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

    private SCR_FishSpawner fishSpawner; // Reference to the Fish Spawner


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
    void AssignHook(SCR_Hook hook)
    {
        hookToFollow = hook;
    }

    void DisperseFish(SCR_Fish fish)
    {
        if (fish != this)
        {
            hookInSight = false;

            // Make fish swim away from the hook, without this, the fish will just continue towards the hook.
            if (hookToFollow != null)
            {
                Vector3 runAwayDircection = (transform.position - hookToFollow.transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, runAwayDircection);

                // Move in the new direction
                transform.position += runAwayDircection * (Time.deltaTime * speed * 2f); // Move faster when dispersing
            }

        }

    }

    void OnDestroy()
    {
        fishingCastController.OnHookCast -= AssignHook;
        fishingCastController.OnFishCaught -= DisperseFish;
    }

    void Update()
    {
        if (fishingCastController._fishingStates == FishingCastController.FishingStates.Cast)
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

        float distanceSqr = (transform.position - game_area.transform.position).sqrMagnitude;
        float boundarySqr = fish_spawner.game_boundary_circle_radius * fish_spawner.game_boundary_circle_radius;

        // Checks if fish is outside of the game boundary.
        if (distanceSqr > boundarySqr)
        {
            if (fish_spawner.fishHooked)
            {
                // if a fish is hooked, removes the fish who touch the boundary
                Debug.Log("Fish is outside boundary and a fish is hooked. Removing fish.");
                RemoveFish();
            }
            else
            {
                // makes fish turn around and adjust angle.
                transform.Rotate(Vector3.forward * 180);
                transform.Rotate(Vector3.forward * Random.Range(-45.0f, 45.0f));
            }
        }


    }

    void RemoveFish()
    {
        // Stop the coroutine if it's running
        if (changeDirectionCoroutine != null)
        {
            StopCoroutine(changeDirectionCoroutine);
        }

        Destroy(gameObject);
        fish_spawner.fish_count = Mathf.Max(0, fish_spawner.fish_count - 1); // Ensure fish_count doesn't go below zero
    }

    IEnumerator ChangeDirectionRoutine()
    {
        while (!hookInSight && !fish_spawner.fishHooked) // Stop changing direction when a fish is hooked

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

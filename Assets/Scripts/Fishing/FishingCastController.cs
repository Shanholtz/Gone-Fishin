using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingCastController : MonoBehaviour
{
    public TurnManager turn;
    public FishingAI ai;
    public StatManager stats;
    public TextMeshProUGUI red;
    public TextMeshProUGUI blue;

    // Fishing line and hook references
    public GameObject fishingLinePrefab; // Prefab for the fishing line (LineRenderer)
    public GameObject hook; // Hook object

    // Fishing mechanics
    public float reelSpeed = 5f;
    public Slider powerBar; // Power meter for casting strength

    public enum FishingStates // Fishing states
    {
        Idle,
        Cast,
        Reeling,
    }
    public FishingStates _fishingStates;

    // Events
    public Action<SCR_Hook> OnHookCast;
    public Action<SCR_Fish> OnFishCaught;
    public Catch catching;

    public Action endOfTurn; // event for end of turn

    public Vector3 origHookPos; // Original hook position
    public SCR_Hook hookLogic;
    public Vector2 startPosition; // Starting position of the line (rod's position)
    public GameObject currentLine; // Instantiated fishing line object
    public GameObject currentHook; // Instantiated hook object
    public LineRenderer lineRenderer; // Line renderer component
    private float power; // Casting power

    public GameObject hookSprite;

    private SCR_FishSpawner fishSpawner; // Reference to the Fish Spawner

    public bool isDraggingPowerBar = false;
    public bool disableRod = false;

    void Start()
    {
        catching = FindObjectOfType<Catch>();
        if (catching != null)
        {
            catching.complete += OnCatchingComplete; // Subscribe to the event, player succeeded
            catching.onSequenceFailed += ReleaseFish; // Player failed
        }
        // Initialize fishSpawner
        fishSpawner = FindObjectOfType<SCR_FishSpawner>();
        if (fishSpawner == null)
        {
            Debug.LogError("FishSpawner not found in the scene!");
        }
    }

    void Update()
    {
        //// Casting the fishing line

        // Handle reeling
        if (_fishingStates == FishingStates.Reeling)
        {
            HandleReeling();
        }
    }

    

    public void OnPowerBarReleased()
    {
        if (_fishingStates == FishingStates.Idle)
        {
            CastLine();
            disableRod = true;
        }
    }


    public void CastLine()
    {
        // Set starting position for cast
        startPosition = transform.position;
        power = powerBar.value;
        
        // If power value is less than 30, defaults it to 30 so the end of the line isnt in the rod.
        if (power < 30)
        {
            power = 30;
        }

        // Instantiate fishing line and configure LineRenderer
        currentLine = Instantiate(fishingLinePrefab, startPosition, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startPosition);

        // Calculate cast direction and endpoint
        Vector2 castDirection = transform.up.normalized;
        Vector2 endPos = startPosition + castDirection * (power / 10);
        lineRenderer.SetPosition(1, endPos);

        // Instantiate hook at end position
        currentHook = Instantiate(hook, endPos, Quaternion.identity);
        currentHook.transform.position = endPos;
        hookLogic = currentHook.GetComponent<SCR_Hook>();

        hookSprite.SetActive(false);

        // Notify fish about hook being cast
        OnHookCast?.Invoke(hookLogic);
        _fishingStates = FishingStates.Cast;



        // Start coroutine for auto-retraction after a delay
        StartCoroutine(ReturnLine());
    }

    private void HandleReeling()
    {
        float speed = reelSpeed;

        // Increase reeling speed if holding left mouse button
        if (Input.GetMouseButton(0))
        {
            speed *= 2;
        }

        // Ensure currentHook and hookLogic exist before proceeding
        if (currentHook == null || hookLogic == null)
        {
            Debug.LogWarning("HandleReeling: No hook found!");
            return;
        }


        SCR_Fish hookedFish = hookLogic.GetComponentInChildren<SCR_Fish>();

        // Move the hook towards the fishing rod
        currentHook.transform.position = Vector3.MoveTowards(currentHook.transform.position, transform.position, speed * Time.deltaTime);
        lineRenderer.SetPosition(1, currentHook.transform.position);

        // Ensure the fish rotates to face downward along the fishing line
        if (hookedFish != null)
        {
            Vector3 directionToRod = (transform.position - hookedFish.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToRod);
            hookedFish.transform.rotation = Quaternion.Slerp(hookedFish.transform.rotation, targetRotation, Time.deltaTime * speed);
        }

        // Check if hook has returned close to rod
        if (Vector3.Distance(currentHook.transform.position, transform.position) <= 1)
        {
            
            if (hookedFish != null)
            {
                int points = hookedFish.scoreValue;

                //Will have to implement a point system.
                if (turn.isPlayerTurn)
                {
                    stats.playerStats.score += points;
                    
                }
                if (!turn.isPlayerTurn)
                {
                    stats.aiStats.score += points;
                }

                red.text = $"That's worth {points} points!"; //Total score: {playerScore}");

                if (points == 1)
                {
                    blue.text = "You caught a Canned Fish.";
                }
                if (points == 3)
                {
                    blue.text = "You caught a Bass.";
                }
                if (points == 5)
                {
                    blue.text = "You caught a Shark.";
                }
                if (points == 7)
                {
                    blue.text = "You caught an Angel Fish!";
                }
                if (points == 10)
                {
                    blue.text = "You caught a Goldie!";
                }
                if (points == 20)
                {
                    blue.text = "You caught Fish Sticks!";
                }

                hookedFish.RemoveFish();

                endOfTurn?.Invoke(); // calls to end turn, switching turn and scene.

                hookSprite.SetActive(true);

            }

        }
    }


    private void OnCatchingComplete()
    {
        _fishingStates = FishingStates.Reeling; // Switch to reeling state
    }

    IEnumerator ReturnLine()
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds
        Debug.Log("waiting 5 seconds");
        // Only return the line if no fish is hooked
        if (hookLogic != null && !hookLogic.isFishHooked)
        {
            DoReturnLine(); // Automatically return the line after a delay
            endOfTurn?.Invoke(); // ends turn, switchs scene.

        }
    }

    void DoReturnLine()
    {
        // Only clean up the hook and line if no fish is hooked
        if (hookLogic != null && !hookLogic.isFishHooked)
        {
            // Clean up the hook and line when retracting
            if (currentHook != null)
            {
                currentHook.GetComponent<SCR_Hook>().isFishHooked = false; // Reset the hook state
                Destroy(currentHook);
            }
            if (currentLine != null)
            {
                Destroy(currentLine);
            }
            hookSprite.SetActive(true);
            _fishingStates = FishingStates.Idle; // Reset the fishing state
        }
    }

    // starts the letter sequence once fish is caught.
    public void CatchFish(SCR_Fish fish)
    {
        // Notify all fish that one has been caught
        fishSpawner.fishHooked = true;
        OnFishCaught?.Invoke(fish);

        Debug.Log("Fish is on");

        // Mark the hook as having caught a fish
        if (hookLogic != null)
        {
            hookLogic.isFishHooked = true;
        }

        // Start the input sequence when the fish is hooked
        if (catching != null)
        {
            catching.GenerateSequence();
            catching.UpdateUI();
        }

        // Attach fish to the instantiated hook
        fish.transform.SetParent(currentHook.transform);
    }

    // Called when time runs out
    void ReleaseFish()
    {
        if (hookLogic != null && hookLogic.isFishHooked)
        {
            Debug.Log("Releasing fish...");

            // Detach fish from the hook
            SCR_Fish hookedFish = hookLogic.GetComponentInChildren<SCR_Fish>();
            if (hookedFish != null)
            {
                hookedFish.transform.SetParent(null);
                hookedFish.hookInSight = false; // Make fish stop following the hook

                // Make fish swim away in a random direction
                Vector3 escapeDirection = (hookedFish.transform.position - transform.position).normalized;
                hookedFish.transform.rotation = Quaternion.LookRotation(Vector3.forward, escapeDirection);
                hookedFish.speed = 9f; // Make fish swim away faster
            }

            // Reset hook state
            hookLogic.isFishHooked = false;

            // Clean up the line and hook
            DoReturnLine();

            endOfTurn?.Invoke(); // ends turn, switchs scene.
        }
    }

    public void ResetPowerBar()
    {
        powerBar.value = 10f;
    }

}
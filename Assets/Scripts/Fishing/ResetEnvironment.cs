using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEnvirement : MonoBehaviour
{
    private FishingCastController fishingController;
    private SCR_FishSpawner fishSpawner;
    private Catch catchSystem;

    private Vector3 originalHookPosition;

    public SceneManager sceneManager;


    void Start()
    {
        fishingController = FindObjectOfType<FishingCastController>();
        fishSpawner = FindObjectOfType<SCR_FishSpawner>();
        catchSystem = FindObjectOfType<Catch>();

        if (fishingController != null)
        {
            originalHookPosition = fishingController.transform.position;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeScene();
        }
    }

    public void ResetGame()
    {
        Debug.Log("Game Reset!");

        // Reset the fishing rod and hook position
        if (fishingController != null)
        {
            fishingController._fishingStates = FishingCastController.FishingStates.Idle;
            fishingController.transform.position = originalHookPosition;

            // Destroy the existing fishing line and hook if they exist
            if (fishingController.currentLine != null)
            {
                Destroy(fishingController.currentLine);
                fishingController.currentLine = null;
            }

            if (fishingController.currentHook != null)
            {
                Destroy(fishingController.currentHook);
                fishingController.currentHook = null;
            }
        }

        // Ensure the fishing rod can rotate again
        FishingRodController rodController = FindObjectOfType<FishingRodController>();
        if (rodController != null)
        {
            rodController.enabled = true; // Reactivate rotation
            rodController.transform.rotation = Quaternion.identity; // Reset to default rotation
        }

        // **Detach the fish from the hook and reset it**
        SCR_Hook hook = FindObjectOfType<SCR_Hook>();
        if (hook != null)
        {
            SCR_Fish hookedFish = hook.GetComponentInChildren<SCR_Fish>();
            if (hookedFish != null)
            {
                hookedFish.transform.SetParent(null); // Remove parent-child relationship
                hookedFish.hookInSight = false; // Make fish stop following the hook
            }

            hook.isFishHooked = false; // Ensure the hook is empty
        }

        // Reset the sequence system
        if (catchSystem != null)
        {
            catchSystem.sequence = "";
            catchSystem.UpdateUI();

            // Reset the bool values
            catchSystem.isTimerRunning = false;
            catchSystem.isSequenceOver = false;

            // Stop any running timers
            catchSystem.StopTimer();
                        
        }

        // Reset fish spawner
        if (fishSpawner != null)
        {
            fishSpawner.ResetFish();
        }
    }

    public void ChangeScene()
    {
        Debug.Log("changing scene");

        sceneManager.ChangeScene();

    }

}
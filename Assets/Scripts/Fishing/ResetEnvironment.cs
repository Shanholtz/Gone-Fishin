using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEnvirement : MonoBehaviour
{
    private FishingCastController fishingController;
    private SCR_FishSpawner fishSpawner;
    private Catch catchSystem;
    private TimerBar timerBar;

    private Vector3 originalHookPosition;

    public FishingAI ai;
    public SceneManager sceneManager;
    public TurnManager turn;
    public StatManager stats;

    private Coroutine changeTurnCoroutine;

    private bool initReset = false; 

    void Start()
    {
        fishingController = FindObjectOfType<FishingCastController>();
        fishSpawner = FindObjectOfType<SCR_FishSpawner>();
        catchSystem = FindObjectOfType<Catch>();
        timerBar = FindObjectOfType<TimerBar>();

        if (fishingController != null)
        {
            originalHookPosition = fishingController.transform.position;

            fishingController.endOfTurn += ChangeTurnScene; // Subscribe to the event, resets game and waits 3 seconds to change scene.
        }

       
    }

    void Update()
    {
        if (initReset == false) // This is here because both player fishing tables spawn on the initial fishing game, has the fishing game reset to get rid of them.
        {
            ResetGame();
            initReset = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeScene();
        }
    }

    void ChangeTurnScene()
    {
        // Start the coroutine and store a reference to it
        if (changeTurnCoroutine != null)
        {
            StopCoroutine(changeTurnCoroutine);
        }

        if(!sceneManager.Final)
        {
            changeTurnCoroutine = StartCoroutine(ChangeTurnSceneWithDelay());
        }
        

        if (turn.isPlayerTurn && sceneManager.Final)
        {
            sceneManager.Playerdone = true;
        }
                
        if (!turn.isPlayerTurn && sceneManager.Final)
        {
            sceneManager.Aidone = true;
        }
    }

    IEnumerator ChangeTurnSceneWithDelay()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Now execute the methods
        ResetGame();
        ChangeScene();
        turn.SwapTurn();
    }

    public void ResetGame()
    {
        Debug.Log("Game Reset!");

        // Reset timer bar first
        if (timerBar != null)
        {
            timerBar.ResetTimer();
        }

        // Reset the fishing rod and hook position
        if (fishingController != null)
        {
            fishingController._fishingStates = FishingCastController.FishingStates.Idle;
            fishingController.transform.position = originalHookPosition;
            fishingController.ResetPowerBar();

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

            ai.enabled = true;
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

        if(turn.isPlayerTurn)
        {
            stats.playerLimit = 3f;
            stats.playerRadius = 2.5f;
            stats.playerTimer = 5f;
        }

        if(!turn.isPlayerTurn)
        {
            stats.aiLimit = 3f;
            stats.aiRadius = 2.5f;
            stats.aiTimer = 5f;
        }
       
    }

    public void ChangeScene()
    {
        Debug.Log("changing scene");

        sceneManager.ChangeScene();

    }

}
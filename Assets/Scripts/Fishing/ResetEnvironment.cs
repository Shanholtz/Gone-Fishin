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

        // Reset fishing rod and hook position
        if (fishingController != null)
        {
            fishingController._fishingStates = FishingCastController.FishingStates.Idle;
            fishingController.transform.position = originalHookPosition;
            fishingController.ResetPowerBar();
            fishingController.disableRod = false;

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

        // Reset fishing rod rotation
        FishingRodController rodController = FindObjectOfType<FishingRodController>();
        if (rodController != null)
        {
            rodController.enabled = true;
            rodController.transform.rotation = Quaternion.identity;
        }

        // Detach any hooked fish
        SCR_Hook hook = FindObjectOfType<SCR_Hook>();
        if (hook != null)
        {
            SCR_Fish hookedFish = hook.GetComponentInChildren<SCR_Fish>();
            if (hookedFish != null)
            {
                hookedFish.transform.SetParent(null);
                hookedFish.hookInSight = false;
            }
            hook.isFishHooked = false;
        }

        // Reset sequence system
        if (catchSystem != null)
        {
            catchSystem.sequence = "";
            catchSystem.UpdateUI();
            catchSystem.isTimerRunning = false;
            catchSystem.isSequenceOver = false;
            catchSystem.StopTimer();
        }

        // Reset fish spawner with correct tables
        if (fishSpawner != null)
        {
            // Set spawn table and limit based on whose turn it is
            if (turn.isPlayerTurn)
            {
                fishSpawner.spawnTable = fishSpawner.playerSpawnTable;
                fishSpawner.fishLimit = stats.playerLimit;
            }
            else
            {
                fishSpawner.spawnTable = fishSpawner.aiSpawnTable;
                fishSpawner.fishLimit = stats.aiLimit;
            }

            fishSpawner.ResetFish();
        }

        // Reset stats ONLY if we're past initial setup
        if (initReset)
        {
            if (turn.isPlayerTurn)
            {
                Debug.Log("Player Stats Are RESET!!!");
                // Reset player stats after their turn ends
                stats.playerLimit = 3f;
                stats.playerRadius = 2.5f;
                stats.playerTimer = 5f;
            }
            else
            {
                Debug.Log("AI Stats Are RESET!!!");
                // Reset AI stats after their turn ends
                stats.aiLimit = 3f;
                stats.aiRadius = 2.5f;
                stats.aiTimer = 5f;
            }
        }
    }


    public void ChangeScene()
    {
        Debug.Log("changing scene");

        sceneManager.ChangeScene();

    }

}
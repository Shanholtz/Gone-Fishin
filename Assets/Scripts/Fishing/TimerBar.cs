using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    Catch catchTimer;
    Image timerBar; // Ensure this is an Image, NOT TimerBar
    public TurnManager turn;
    public StatManager stats;
    public float maxTime;
    float timeLeft;

    void Start()
    {
        timerBar = GetComponent<Image>(); // Now this will work correctly
        timerBar.fillAmount = 0f;

        if (catchTimer == null)
        {
            catchTimer = FindObjectOfType<Catch>(); // Find the Catch script
            if (catchTimer == null)
            {
                Debug.LogError("TimerBar: Could not find Catch script in the scene!");
                return;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        }
        else if (timeLeft <= 0 && maxTime > 0)
        {
            // Time's up, but no need to freeze the game unless specified
            // Time.timeScale = 0; // Pauses the game (if needed)
        }
    }

    // Start the timer and show the timer bar
    public void StartTimer()
    {
        if(turn.isPlayerTurn)
        {
            maxTime = stats.playerTimer;
        }
        if(!turn.isPlayerTurn)
        {
            maxTime = stats.aiTimer;
        }
        // Safe to use after checking
        timeLeft = maxTime; // Reset the timer
        timerBar.fillAmount = 1f; // Reset the UI fill amount

       
    }

    public void ResetTimer()
    {
        timeLeft = 0f;
        timerBar.fillAmount = 0f;
    }

}

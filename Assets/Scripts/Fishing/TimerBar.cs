using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    Catch catchTimer;
    Image timerBar; // Ensure this is an Image, NOT TimerBar
    public float maxTime;
    float timeLeft;

    void Start()
    {
        timerBar = GetComponent<Image>(); // Now this will work correctly

        if (catchTimer == null)
        {
            catchTimer = FindObjectOfType<Catch>(); // Find the Catch script
            if (catchTimer == null)
            {
                Debug.LogError("TimerBar: Could not find Catch script in the scene!");
                return;
            }
        }

        // Hide the timer bar initially
        //timerBar.gameObject.SetActive(false);
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
        maxTime = catchTimer.timerDuration; // Safe to use after checking
        timeLeft = maxTime; // Reset the timer

        // Show the timer bar when the timer starts
        //timerBar.gameObject.SetActive(true);
    }
}

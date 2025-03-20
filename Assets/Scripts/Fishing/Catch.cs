using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Catch : MonoBehaviour
{
    public string sequence = "";
    public int sequenceLength = 5;
    public char[] possibleInputs = { 'W', 'A', 'S', 'D' };

    public TMP_Text sequenceText;

    public Action complete; // event for success
    public Action onSequenceFailed; // event for failure, when time runs out

    private Coroutine sequenceTimerCoroutine; // Track the timer coroutine
    public float timerDuration = 5f; // In seconds

    private SCR_Hook hookLogic;

    private bool isTimerRunning = false;
    private bool isSequenceOver = false; 

    // Begins the letter seqeunce. 
    void Update()
    {
     
        if (sequence.Length > 0 && isSequenceOver == false)
        {
            char expectedKey = sequence[0];

            foreach (char key in possibleInputs)
            {
                if (Input.GetKeyDown(key.ToString().ToLower()))
                {
                    if (key == expectedKey) // Correct input
                    {
                        sequence = sequence.Substring(1); // Remove first letter
                        UpdateUI();

                        if (sequence.Length == 0) // If sequence is completed
                        {
                            Debug.Log("Success! Sequence completed.");
                            StopTimer(); // Stop the timer
                            complete?.Invoke();
                        }
                    }
                    else // Incorrect input
                    {
                        Debug.Log("Incorrect input. Restarting sequence.");
                        GenerateSequence();
                        UpdateUI();
                    }
                    break; // Prevent multiple keys from being processed at once
                }
            }
        }
    }


    public void GenerateSequence()
    {
        Debug.Log("Generating Sequence");

        sequence = "";
        for (int i = 0; i < sequenceLength; i++)
        {
            // Explicitly use UnityEngine.Random to avoid ambiguity
            sequence += possibleInputs[UnityEngine.Random.Range(0, possibleInputs.Length)];
        }

        // Ensures that the timer isnt reset when new sequence is generated
        if(isTimerRunning == false)
        {
            StartTimer();
            isTimerRunning = true;
        }
        
    }

    public void UpdateUI()
    {
        sequenceText.text = sequence;
    }

    private IEnumerator SequenceTimer()
    {
        yield return new WaitForSeconds(timerDuration); // wait for how many seconds

        if (sequence.Length > 0) // If the sequence is not completed
        {
            Debug.Log("Times up! fish escaped.");
            onSequenceFailed?.Invoke(); // Trigger the failure event

            // Clears the sequence text and prevents the sequence from being generated again.
            sequenceText.text = "";
            isSequenceOver = true; 
        }
    }

    private void StartTimer()
    {
        Debug.Log("Timer Start");
        StopTimer(); // ensures no previous timer is running
        sequenceTimerCoroutine = StartCoroutine(SequenceTimer());
    }

    private void StopTimer()
    {
        if (sequenceTimerCoroutine != null)
        {
            StopCoroutine(sequenceTimerCoroutine);
            sequenceTimerCoroutine = null;
        }
    }

}
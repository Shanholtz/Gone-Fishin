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

    public Action complete;

    private SCR_Hook hookLogic;

    // Begins the letter seqeunce. 
    void Update()
    {
     
        if (sequence.Length > 0)
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
        sequence = "";
        for (int i = 0; i < sequenceLength; i++)
        {
            // Explicitly use UnityEngine.Random to avoid ambiguity
            sequence += possibleInputs[UnityEngine.Random.Range(0, possibleInputs.Length)];
        }
    }

    public void UpdateUI()
    {
        sequenceText.text = sequence;
    }
}
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Catch : MonoBehaviour
{
    public string sequence = "";
    public int sequenceLength = 5;
    public char[] possibleInputs = {'W', 'A', 'S', 'D'};

    public TMP_Text sequenceText;

    public Action complete;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)){
            generateSequence();
            UpdateUI();
        }

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
                        generateSequence();
                        UpdateUI();
                    }
                    break; // Prevent multiple keys from being processed at once
                }
            }
        }
    }

    public void generateSequence()
    {
        sequence = "";
        for (int i=0; i < sequenceLength; i++)
        {
            sequence += possibleInputs[UnityEngine.Random.Range(0, possibleInputs.Length)];
        }
    }

    public void UpdateUI()
    {
        sequenceText.text = sequence;
    }
}

using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Catch : MonoBehaviour
{
    private string sequence = "";
    private string userInput = "";
    public int sequenceLength = 5;
    private char[] possibleInputs = {'W', 'A', 'S', 'D'};

    public TMP_Text sequenceText;

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

        if (Input.anyKeyDown)
        {
            foreach (char key in possibleInputs)
            {
                if (Input.GetKeyDown(key.ToString().ToLower()))
                {
                    userInput += key;
                    CheckInput();
                }
            }
        }
    }

    void generateSequence()
    {
        sequence = "";
        for (int i=0; i < sequenceLength; i++)
        {
            sequence += possibleInputs[Random.Range(0, possibleInputs.Length)];
        }
        
    }

    void CheckInput()
    {
        if (userInput == sequence)
        {
            Debug.Log("Success! You entered the correct sequence.");
            userInput = "";
        }
        else if (!sequence.StartsWith(userInput))
        {
            Debug.Log("Incorrect sequence. Try again.");
            userInput = "";
        }
    }

    void UpdateUI()
    {
        sequenceText.text = sequence;
    }
}

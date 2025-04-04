using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishingAI : MonoBehaviour
{
    public TurnManager turn;
    public FishingCastController cast;
    public Catch catching;
    string sequence = "";
    public char[] possibleInputs = { 'W', 'A', 'S', 'D' };
    public float rotationSpeed = 150f;
    public float maxAngle = 50f;
    private float randomInput;
    private float rotateTimer = 0f;
    private float clickTimer = 5f; // Timer for simulated click

    public void Update()
    {
        if (!turn.isPlayerTurn)
        {
            RotateRod();

            // Countdown to simulated click
            clickTimer -= Time.deltaTime;
            if (clickTimer <= 0)
            {
                cast.powerBar.value = Random.Range(30,100);

                cast.CastLine();
                SimulateClick(); 
            }
        }
    }

    public void AISequence()
    {
        sequence = "";

        // Copys the first part of the generated sequence except the last letter
        int copyLength = Mathf.Max(0, catching.sequenceLength - 1); 
        sequence = catching.sequence.Substring(0, copyLength);

        // Decide whether to choose the correct letter or a wrong one
        float chance = UnityEngine.Random.value; // gives a float between 0.0 and 1.0

        char correctLastChar = catching.sequence[copyLength];
        char chosenChar;

        if (chance <= 0.6f)
        {
            // 60% chance to pick the correct final input
            chosenChar = correctLastChar;
        }
        else
        {
            // 40% chance to pick a wrong input
            List<char> wrongOptions = new List<char>(possibleInputs);
            wrongOptions.Remove(correctLastChar);
            chosenChar = wrongOptions[UnityEngine.Random.Range(0, wrongOptions.Count)];
        }

        sequence += chosenChar;
        Debug.Log("AI sequence: " + sequence);

        StartCoroutine(SimulateInputSequence(sequence));
    }

    private IEnumerator SimulateInputSequence(string aiSequence)
    {
        for (int i = 0; i < aiSequence.Length; i++)
        {
            char currentChar = aiSequence[i];

            // Simulate key press by feeding the key to the catching system
            if (currentChar == catching.sequence[0])
            {
                // Correct key, remove it from the target sequence
                catching.sequence = catching.sequence.Substring(1);
                catching.UpdateUI();

                if (catching.sequence.Length == 0)
                {
                    Debug.Log("AI completed the sequence!");
                    catching.StopTimer();
                    catching.complete?.Invoke();
                    yield break;
                }
            }
            else
            {
                // Incorrect key, reset the sequence
                Debug.Log("AI made a mistake!");
                catching.GenerateSequence();
                catching.UpdateUI();

                // Optionally restart the AI after a short pause
                yield return new WaitForSeconds(1f);
                StartCoroutine(catching.StallAI());
                yield break;
            }

            yield return new WaitForSeconds(0.5f); // Delay between inputs
        }
    }

    public void RotateRod()
    {
        if (clickTimer >= 0)
        {
            rotateTimer -= Time.deltaTime;
            if (rotateTimer <= 0)
            {
                randomInput = Random.Range(-1f, 1f); // Change input at intervals
                rotateTimer = Random.Range(0.5f, 2f); // Set new interval before changing again
            }

            float currentAngle = transform.eulerAngles.z;
            if (currentAngle > 180)
                currentAngle -= 360;

            float newAngle = currentAngle - randomInput * rotationSpeed * Time.deltaTime;
            float clamped = Mathf.Clamp(newAngle, -maxAngle, maxAngle);

            transform.rotation = Quaternion.Euler(0, 0, clamped); 
        }
            
    }

    // Simulated Click After 5 Seconds
    private void SimulateClick()
    {
        Debug.Log("AI Simulated Click!");
        clickTimer = 5f;
        DisableAI();
    }

    private void DisableAI()
    {
        this.enabled = false;
    }
}

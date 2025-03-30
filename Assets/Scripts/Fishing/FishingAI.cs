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
                cast.powerBar.value = Random.Range(10f,100f);

                cast.CastLine();
                SimulateClick(); 
            }
        }
    }

    public void AISequence()
    {
        sequence = "";
        for (int i = 0; i < catching.sequenceLength; i++)
        {
            // Explicitly use UnityEngine.Random to avoid ambiguity
            sequence += possibleInputs[UnityEngine.Random.Range(0, possibleInputs.Length)];
        }
        
        Debug.Log("AI sequence: " + sequence);
        catching.compareAI(sequence);
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

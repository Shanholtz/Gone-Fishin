using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class AngleNeedle : MonoBehaviour
{
    public float rotationSpeed = 100f; // Rotation speed (degrees per second)
    public float maxRotation = 70f; // Maximum rotation in degrees to the left and right
    private float currentRotation = 0f; // The current rotation of the needle
    private bool isRotating = true;

    ScriptReference scrip;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //Disables abiltiy to rotate on Mouse Release
            isRotating = false;
        }
        
        if (isRotating){
            // Get the user input (typically A/D or arrow keys)
            float input = Input.GetAxis("Horizontal") * -1; // -1 to 1 (left to right)

            // Calculate the rotation amount based on input and rotation speed
            float rotationAmount = rotationSpeed * input * Time.deltaTime;

            // Update the current rotation, clamped between -90 and 90 degrees
            float newRotation = currentRotation + rotationAmount;
            
            // Apply the rotation only if the new rotation is within the limit
            if (Mathf.Abs(newRotation) <= maxRotation)
            {
                // Update the rotation
                currentRotation = newRotation;

                // Rotate the object around the pivot point (player.position)
                transform.RotateAround(player.position, Vector3.forward, rotationAmount);
            }    
        }
        
        
    }
}

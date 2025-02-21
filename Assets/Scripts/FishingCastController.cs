using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class FishingCastController : MonoBehaviour
{
    public GameObject fishingLinePrefab; // The prefab for the fishing line (LineRenderer)
    public GameObject hook;
    public Slider powerBar;
    private Vector3 startPosition; // Starting position of the line (rod's position) 
    private GameObject currentLine; // The instantiated fishing line
    private GameObject currentHook;
    private LineRenderer lineRenderer; // The line renderer component
    float power;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) // Release mouse button to cast the line
        {
            CastLine();
        }
    }

    private void CastLine()
    {

        // Start the casting when player clicks
        startPosition = transform.position;
        startPosition.z = 0f; // Ensure the position is at the same plane as the rod

        power = powerBar.value;

        // Create a new line renderer for the cast (we’ll update it while dragging)
        currentLine = Instantiate(fishingLinePrefab, startPosition, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startPosition); // Set the start position of the line at the rod's position

        Vector2 castDirection = transform.up.normalized; // Change this to the desired casting direction
        Vector2 endPos = (Vector2)startPosition + castDirection * (power/10); 
        lineRenderer.SetPosition(1, endPos);
        // Calculate the direction the rod is facing (based on its rotation)

        power = powerBar.value;

        // Optionally, make the line disappear after the cast
         // Destroy the line after 3 seconds (adjust as needed)

        currentHook = Instantiate(hook, endPos, Quaternion.identity);

        currentHook.transform.position = endPos;

        Destroy(currentHook, 3f);
        Destroy(currentLine, 3f);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Power : MonoBehaviour
{

    private bool dragging = false;
    private Vector3 offset;
    private Vector3 startPos;
    private Vector3 initialMousePos;
    private float meterHeight;

    public float speed = 0.005f;
    public GameObject Bar;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        meterHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {

        // mouse button pressed, start dragging
        if(Input.GetMouseButtonDown(0))
        {
            dragging = true;
            
            Debug.Log("Pressed");
            
            // Store initial mouse position
            initialMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initialMousePos.z = 0;
        }

        // Mouse Button Released, stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        if (dragging)
        {
            // Get the current mouse position in world space
            Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePos.z = 0; // Keep Z position fixed for 2D

            // Calculate the offset movement based on the difference from the initial position
            float deltaY = currentMousePos.y - initialMousePos.y;
            deltaY *= speed;

            // Calculate the top and bottom limits for the Bar based on Power's height
            float topLimit = startPos.y + meterHeight / 2;
            float bottomLimit = startPos.y - meterHeight / 2;

            // Limit the Bar's Y position to stay within the bounds of the Power object
            float clampedY = Mathf.Clamp(Bar.transform.position.y + deltaY, bottomLimit, topLimit);

            // Update the 'Bar' object position
            Bar.transform.position = new Vector3(Bar.transform.position.x, clampedY, Bar.transform.position.z);
        }
    }
}


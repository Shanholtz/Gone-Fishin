using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FishingCastController : MonoBehaviour
{
    public GameObject fishingLinePrefab; // The prefab for the fishing line (LineRenderer)
    public GameObject hook;
    Vector3 origHookPos;
    private SCR_Hook hookLogic;
    public float reelSpeed = .5f;

    public Slider powerBar;
    public Vector2 endPos;
    private Vector2 startPosition; // Starting position of the line (rod's position) 
    private GameObject currentLine; // The instantiated fishing line
    private GameObject currentHook;
    private LineRenderer lineRenderer; // The line renderer component
    float power;
    public enum FishingStates{
        idle = 0,
        cast = 1,
        reeling = 2,
    }   

    public FishingStates _fishingStates;
    public Action<SCR_Hook> OnHookCast;
    public Action<SCR_Fish> OnFishCaught;
    public Catch catching;

    void Start() {
        origHookPos = hook.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && _fishingStates == FishingStates.idle) // Release mouse button to cast the line
        {
            CastLine();
        }

        if (_fishingStates == FishingStates.cast)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                //Return to start/despawn line 
                DoReturnLine();
            }

            //if(caught);
        }

        if(_fishingStates == FishingStates.reeling)
        {
            float speed = reelSpeed;
            if(Input.GetMouseButton(0))
            {
                speed *= 2;
                // float segment = calcLength();
                // Vector2 reelDirection = -transform.up;
                // Vector2 endPos = (Vector2)startPosition + reelDirection * segment; 
                // lineRenderer.SetPosition(1, endPos);
            }
            
            
            currentHook.transform.position = Vector3.MoveTowards(currentHook.transform.position, transform.position, speed * Time.deltaTime);
            
            lineRenderer.SetPosition(1, currentHook.transform.position);

            float distanceFromRod = Vector3.Distance(currentHook.transform.position, transform.position);

            if(distanceFromRod <= 1)
            {
                Debug.Log("Fish caught");
            }

        }
        
    }

    private void CastLine()
    {
        // Start the casting when player clicks
        startPosition = transform.position;

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
        currentHook = Instantiate(hook, endPos, Quaternion.identity);
        currentHook.transform.position = endPos;  
        hookLogic = currentHook.GetComponent<SCR_Hook>();

        //Let Fish know we threw a hook out!

    
        OnHookCast?.Invoke(hookLogic);  
        
        _fishingStates = FishingStates.cast;

        StartCoroutine(ReturnLine());
    }

    IEnumerator ReturnLine()
    {
        yield return new WaitForSeconds(3f);

        //DoReturnLine();
    }

    void DoReturnLine()
    {
        Destroy(currentHook);
        Destroy(currentLine);

        _fishingStates = FishingStates.idle;
    }

    public void CatchFish(SCR_Fish fish)
    {
        _fishingStates = FishingStates.reeling;
        
        fish.transform.SetParent(hook.transform);

        OnFishCaught.Invoke(fish);
    }

    float calcLength()
    {
        float stringLength = catching.sequenceLength;
        Vector2 line = endPos - startPosition;
        float lineLength = line.magnitude;
        float segmentLength = lineLength / stringLength;

        return segmentLength;
    }
        
}
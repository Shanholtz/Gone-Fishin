using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingCastController : MonoBehaviour
{
    // Fishing line and hook references
    public GameObject fishingLinePrefab; // Prefab for the fishing line (LineRenderer)
    public GameObject hook; // Hook object
    // Fishing mechanics
    public float reelSpeed = 0.5f;
    public Slider powerBar; // Power meter for casting strength
    public enum FishingStates   // Fishing states
    {
        Idle,
        Cast,
        Reeling,
    }
    public FishingStates _fishingStates;
    
    // Events
    public Action<SCR_Hook> OnHookCast;
    public Action<SCR_Fish> OnFishCaught;
    public Catch catching;

    private Vector3 origHookPos; // Original hook position
    private SCR_Hook hookLogic;
    private Vector2 startPosition; // Starting position of the line (rod's position)
    private GameObject currentLine; // Instantiated fishing line object
    private GameObject currentHook; // Instantiated hook object
    private LineRenderer lineRenderer; // Line renderer component
    private float power; // Casting power
    
    
    

    void Start()
    {
        catching = FindObjectOfType<Catch>();
        if (catching != null)
        {
            catching.complete += OnCatchingComplete; // Subscribe to the event
        }
    }

    void Update()
    {
        // Casting the fishing line
        if (Input.GetMouseButtonUp(0) && _fishingStates == FishingStates.Idle)
        {
            CastLine();
        }

        // Handling reeling and returning
        if (_fishingStates == FishingStates.Cast)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                DoReturnLine(); // Return the line
            }
        }

        if (_fishingStates == FishingStates.Reeling)
        {
            HandleReeling();
        }
    }

    private void CastLine()
    {
        // Set starting position for cast
        startPosition = transform.position;
        power = powerBar.value;

        // Instantiate fishing line and configure LineRenderer
        currentLine = Instantiate(fishingLinePrefab, startPosition, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startPosition);
        
        // Calculate cast direction and endpoint
        Vector2 castDirection = transform.up.normalized;
        Vector2 endPos = startPosition + castDirection * (power / 10);
        lineRenderer.SetPosition(1, endPos);

        // Instantiate hook at end position
        currentHook = Instantiate(hook, endPos, Quaternion.identity);
        currentHook.transform.position = endPos;
        hookLogic = currentHook.GetComponent<SCR_Hook>();

        // Notify fish about hook being cast
        OnHookCast?.Invoke(hookLogic);
        _fishingStates = FishingStates.Cast;

        // Start coroutine for auto-retraction after a delay
        StartCoroutine(ReturnLine());
    }

    private void HandleReeling()
    {
        float speed = reelSpeed;
        
        // Increase reeling speed if holding left mouse button
        if (Input.GetMouseButton(0))
        {
            speed *= 2;
        }

        // Move the hook towards the fishing rod
        currentHook.transform.position = Vector3.MoveTowards(currentHook.transform.position, transform.position, speed * Time.deltaTime);
        lineRenderer.SetPosition(1, currentHook.transform.position);

        // Check if hook has returned close to rod
        if (Vector3.Distance(currentHook.transform.position, transform.position) <= 1)
        {
            Debug.Log("Fish caught");
        }
    }

    private void OnCatchingComplete()
    {
        _fishingStates = FishingStates.Reeling; // Switch to reeling state
    }

    IEnumerator ReturnLine()
    {
        yield return new WaitForSeconds(3f);
        //DoReturnLine(); // Uncomment if auto-return is desired
    }

    void DoReturnLine()
    {
        // Clean up the hook and line when retracting
        Destroy(currentHook);
        Destroy(currentLine);
        _fishingStates = FishingStates.Idle;
    }

    public void CatchFish(SCR_Fish fish)
    {
        // Start reeling state and attach fish to hook
        fish.transform.SetParent(hook.transform);
        OnFishCaught?.Invoke(fish);
    }
}

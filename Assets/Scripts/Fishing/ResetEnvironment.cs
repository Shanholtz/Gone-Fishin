using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEnvirement : MonoBehaviour
{
    public SCR_FishSpawner fishSpawner;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key has been pressed.");
            fishSpawner.InitialPopulation();

        }

    
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class FishingRodController : MonoBehaviour
{

    public float rotationSpeed = 150f;
    public float maxAngle = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateRod();
    }

    private void RotateRod()
    {
        // Get horizontal player input
        float input = Input.GetAxis("Horizontal");

        float currentAngle = transform.eulerAngles.z;

        if(currentAngle > 180)
            currentAngle -= 360;

        float newAngle = currentAngle - input * rotationSpeed * Time.deltaTime;

        float clamped = Mathf.Clamp(newAngle, -maxAngle, maxAngle);

        transform.rotation = Quaternion.Euler(0,0, clamped);

        // Rotate rod
        //transform.Rotate(Vector3.forward * -input * rotationSpeed * Time.deltaTime);
    }
}

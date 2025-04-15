using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRodController : MonoBehaviour
{
    public TurnManager turn;
    public FishingCastController disableRod;
    public float rotationSpeed = 150f;
    public float maxAngle = 50f;

    // Update is called once per frame
    public void Update()
    {
        if (turn.isPlayerTurn)
        {
            RotateRod();

            if (disableRod == null)
            {
                Debug.LogWarning("FishingCastController is not assigned to FishingRodController!");
            }
            else
            {
                //Debug.Log("disableRod bool value is: " + disableRod.disableRod);

                if (disableRod.disableRod)
                {
                    Debug.Log("Rod rotation script DISABLED via bool!");
                    this.enabled = false;
                }
            }
        }
    }

    public virtual void RotateRod()
    {
        // Get horizontal player input
        float input = Input.GetAxis("Horizontal");

        float currentAngle = transform.eulerAngles.z;

        if (currentAngle > 180)
            currentAngle -= 360;

        float newAngle = currentAngle - input * rotationSpeed * Time.deltaTime;

        float clamped = Mathf.Clamp(newAngle, -maxAngle, maxAngle);

        transform.rotation = Quaternion.Euler(0, 0, clamped);
    }

}
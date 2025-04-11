using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Hook : MonoBehaviour
{
    public GameObject gameArea;

    public float attractionRadius = 2.5f;
    public float hookedRadius = 0.25f;

    public bool isFishHooked = false; // New flag to track if a fish is hooked
}
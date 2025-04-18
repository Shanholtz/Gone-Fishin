using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public GameObject GoFish;
    public GameObject Start;
    public GameObject Scene;
    public GameObject chars;
    public void startButton()
    {
        Debug.Log("Button Clicked");
        GoFish.SetActive(true);
        Scene.SetActive(true);
        chars.SetActive(true);
        Start.SetActive(false);
    }
}

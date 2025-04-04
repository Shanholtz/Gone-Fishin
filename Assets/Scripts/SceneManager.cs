using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    public GameObject GoFish;
    public GameObject Fishing;
    public HandManager hand;
    public AIManager aiHand;

    // Start is called before the first frame update
    void Start()
    {
        GoFish.SetActive(true);
        Fishing.SetActive(false);
    }

    public void ChangeScene()
    {
        GoFish.SetActive(!GoFish.activeSelf);
        Fishing.SetActive(!Fishing.activeSelf);

        if (GoFish)
        {
            if(hand.hand.Count == 0)
            {
                hand.DrawHand();
            }

            if(aiHand.hand.Count == 0)
            {
                hand.DrawHand();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

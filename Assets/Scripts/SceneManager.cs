using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    public GameObject GoFish;
    public GameObject Fishing;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendLine : MonoBehaviour
{

    //public float distance = GameObject.Find("Power").GetComponent<Power>;
    public float stretchSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {


            transform.localScale += new Vector3(0, stretchSpeed * Time.deltaTime, 0);
        }
    }
        
}

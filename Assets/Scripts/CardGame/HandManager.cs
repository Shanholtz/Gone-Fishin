using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public Transform handPanel;

    public void UpdateHand(List<Card> hand)
    {
        foreach (Transform child in handPanel) Destroy(child.gameObject);

        foreach (Card card in hand)
        {
            
            GameObject cardGO = Instantiate(card.gameObject, handPanel);
            cardGO.transform.localScale = Vector3.one;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public string rank;
    public string suit;
    public bool isFaceUp = false;

    public Sprite frontSprite;
    public Sprite backSprite;

    private SpriteRenderer spriteRenderer;

    private bool dragging;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.Log("spriteRenderer is null");
        }

        spriteRenderer.sortingLayerName = "Cards";
        UpdateCardFace();
    }

    public void FlipCard(bool faceUp)
    {
        isFaceUp = faceUp;
        UpdateCardFace();
    }

    private void UpdateCardFace()
    {
        if (frontSprite == null)
        {
            Debug.LogError("Front sprites are not assigned on " + gameObject.name);
            return;
        }

        if (backSprite == null)
        {
            Debug.LogError("Back sprites are not assigned on " + gameObject.name);
            return;
        }

        spriteRenderer.sprite = isFaceUp ? frontSprite : backSprite;
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked " + gameObject.name);
    }

    private void OnMouseOver()
    {
        Debug.Log("Hovering on " + gameObject.name);
    }

    private void OnMouseDrag()
    {
        Debug.Log("Dragging " + gameObject.name);

        //todo find way to move with mouse
        transform.position = Camera.main.ScreenToViewportPoint( Input.mousePosition);
        dragging = true;
    }

    private void OnMouseUp()
    {
        if (dragging)
        {
            Debug.Log("Dragging ended for" + gameObject.name);
        }
    }

    //For collision. This may require at least one of the objects to have Rigidbody2d. 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //could detect that card was placed on another. 
        if (collision.gameObject.CompareTag("Card"))
        {
            Debug.Log(gameObject.name + " collided with " + collision.gameObject.name);
        }

        //Can check for end of drag outcomes here depending on what we hit with our dragging card 
    }
}


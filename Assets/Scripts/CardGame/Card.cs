using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public string rank;
    public string suit;
    public bool isFaceUp = false;
    public float hoverHeight = 0.5f; 

    public Sprite frontSprite;
    public Sprite backSprite;

    private SpriteRenderer spriteRenderer;
    private bool dragging;
    private Vector2 assignedPosition;
    private Vector2 hoverOffset;

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

        hoverOffset = new Vector2(0, hoverHeight);
    }

    public void SetPosition(Vector2 newPos)
    {
        assignedPosition = newPos;
        transform.position = assignedPosition;
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

    private void OnMouseOver()
    {
        if (transform.parent != transform.parent.CompareTag("Deck"))
        {
            transform.position = assignedPosition + hoverOffset;
        }
        
    }

    private void OnMouseExit()
    {
        transform.position = assignedPosition;
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public string rank;
    public string suit;
    public bool isFaceUp = false;
    public float hoverHeight = 0.5f;

    public Sprite frontSprite;
    public Sprite backSprite;
    public Button AskButton;

    private SpriteRenderer spriteRenderer;
    private Vector2 assignedPosition;
    private Vector2 hoverOffset;

    private PlayerManager playerHand;

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

        // Find references to player and AI hands
        playerHand = FindObjectOfType<PlayerManager>();
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
        spriteRenderer.sprite = isFaceUp ? frontSprite : backSprite;
    }

    private void OnMouseOver()
    {
        if (transform.parent == playerHand)
        {
            transform.position = assignedPosition + hoverOffset;
        }
    }

    private void OnMouseExit()
    {
        transform.position = assignedPosition;
    }

    private void OnMouseDown()
    {
        if (!playerHand.isTurn) return; // Only allow selection during player's turn
        AskButton.enabled = true;
        playerHand.SelectCard(this);
    }
}

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
}


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
        spriteRenderer.sortingLayerName = "Cards";
        UpdateCardFace();
    }

    // Update is called once per frame
    void Update()
    {

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
}


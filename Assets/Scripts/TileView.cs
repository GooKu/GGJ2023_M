using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{
    private SpriteRenderer render;

    private void Start()
    {
        GetComponent<SpriteRenderer>();
    }

    public void UpdateSprite(Sprite sprite)
    {
        render.sprite = sprite;
    }
}

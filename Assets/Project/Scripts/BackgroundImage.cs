using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImage : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;

    public void SetSprite(Sprite newSprite)
    {
        _sr.sprite = newSprite;
    }
}

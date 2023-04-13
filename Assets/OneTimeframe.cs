using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneTimeframe : MonoBehaviour
{
    [SerializeField]
    private Image timeframeImage;

    private bool isCorrupted = false;

    public bool IsCorrupted { get => isCorrupted; set => isCorrupted = value; }
    public void Init(Sprite newSprite, bool isCorrupted)
    {
        timeframeImage.sprite = newSprite;
        this.isCorrupted = isCorrupted;
    }
}

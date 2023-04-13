using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneTimeframe : MonoBehaviour
{
    [SerializeField]
    private Image timeframeImage;

    public void Init(Sprite newSprite)
    {
        timeframeImage.sprite = newSprite;
    }
}

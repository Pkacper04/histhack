using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneTimeframe : MonoBehaviour
{
    [SerializeField]
    private Image timeframeImage;

    private bool isCorrupted = false;

    private AllMinigames minigameData;

    public bool IsCorrupted { get => isCorrupted; set => isCorrupted = value; }

    public AllMinigames MinigameData { get => minigameData; set => minigameData = value; }

    public void Init(Sprite newSprite, bool isCorrupted, AllMinigames minigameData)
    {
        timeframeImage.sprite = newSprite;
        this.isCorrupted = isCorrupted;
        this.minigameData = minigameData;
    }

    public void UnlockTimeFrame(Sprite newSprite)
    {
        timeframeImage.sprite = newSprite;
        this.isCorrupted = false;
    }
}

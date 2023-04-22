using Histhack.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneTimeframe : MonoBehaviour
{
    [SerializeField]
    private Image timeframeImage;

    [SerializeField]
    private Image corruptedImage;

    [SerializeField]
    private float unlockTimeframeDuration;

    private bool isCorrupted = false;

    private AllMinigames minigameData;

    private string year;

    public bool IsCorrupted { get => isCorrupted; set => isCorrupted = value; }

    public string Year { get=> year; set => year = value; }

    public AllMinigames MinigameData { get => minigameData; set => minigameData = value; }

    public void Init(Sprite newSprite, Sprite corruptedSprite, bool isCorrupted, AllMinigames minigameData, string year)
    {

        this.isCorrupted = isCorrupted;

        if (isCorrupted)
        {
            this.corruptedImage.sprite = corruptedSprite;
            this.corruptedImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            timeframeImage.sprite = newSprite;
        }

        this.minigameData = minigameData;
        this.year = year;
    }

    public void UnlockTimeFrame(Sprite newSprite)
    {
        timeframeImage.sprite = newSprite;
        this.isCorrupted = false;
        MainGameController.Instance.AddictionalMethods.FadeElement(unlockTimeframeDuration, corruptedImage, 1, 0);
    }
}

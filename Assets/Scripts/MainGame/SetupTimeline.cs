using Histhack.Core;
using Managers.Sounds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupTimeline : MonoBehaviour
{
    [SerializeField]
    private Sprite corruptedSprite;

    [SerializeField]
    private List<Sprite> normalSprites;

    [SerializeField]
    private List<OneElementData> elementsToAdd = new List<OneElementData>();

    [SerializeField]
    private RectTransform parentTransform;

    [SerializeField]
    private OneTimeframe oneTimeframe;

    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private Image mainBackground;

    private List<OneTimeframe> elementsAddedOnScene = new List<OneTimeframe>();

    private int currentTimeFrame = 0;

    private bool blockInteraction = false;

    private void Start()
    {
        
        Setup();
        SetCurrentBackgroundImage();
       
    }

    private void Update()
    {
        if (blockInteraction)
            return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ChangeTimeFrame(-1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ChangeTimeFrame(1);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivatePanel();
        }
    }

    private void ActivatePanel()
    {
        if (elementsAddedOnScene[currentTimeFrame].IsCorrupted)
        {
            blockInteraction = true;
            ActivateMinigame();
        }

    }

    private void ActivateMinigame()
    {
        SetMinigameData(elementsAddedOnScene[currentTimeFrame].MinigameData);
        MainGameController.Instance.MinigameIndex = currentTimeFrame;
        MainGameController.Instance.MinigameStarted = true;
        MainGameController.Instance.LastMinigameSucceded = false;
        MainGameController.Instance.StartMinigame();
    }

    private void SetMinigameData(AllMinigames minigameData)
    {
        MainGameController.Instance.MinigameType = minigameData.minigameType;

        switch (minigameData.minigameType)
        {
            case MinigamesTypes.QuizMinigame:
                MainGameController.Instance.MinigameData.Question = minigameData.quiz;
                break;
            case MinigamesTypes.PuzzleMinigame:
                MainGameController.Instance.MinigameData.MapIndex = minigameData.mapIndex;
                break;
            case MinigamesTypes.TextToImage:
                MainGameController.Instance.MinigameData.PicturesQuizzes = minigameData.pictures;
                break;
            case MinigamesTypes.TextToText:
                MainGameController.Instance.MinigameData.Pseudonyms = minigameData.pseudonyms;
                break;
        }
    }

    private void ChangeTimeFrame(int direction)
    {
        if (direction > 0 && currentTimeFrame == 0)
            return;
        else if (direction < 0 && currentTimeFrame == (elementsToAdd.Count - 1))
            return;

        if (elementsAddedOnScene[currentTimeFrame].IsCorrupted && direction < 0)
            return;

        blockInteraction = true;
        
        Debug.Log("current time frame: "+currentTimeFrame);
        if (currentTimeFrame == 0)
            playerMovement.MovePlayer(direction, () => blockInteraction = false);
        else if (currentTimeFrame == (elementsToAdd.Count - 1))
            playerMovement.MovePlayer(direction, () => blockInteraction = false);
        else if(currentTimeFrame == 1 && direction > 0)
            playerMovement.MovePlayer(direction, () => blockInteraction = false);
        else if(currentTimeFrame == (elementsToAdd.Count - 2) && direction < 0)
            playerMovement.MovePlayer(direction, () => blockInteraction = false);
        else
            playerMovement.MoveBackground(direction, () => blockInteraction = false);

        currentTimeFrame += direction * -1;
        SetCurrentBackgroundImage();
        MainGameController.Instance.DateController.UpdateDate(int.Parse(elementsAddedOnScene[currentTimeFrame].Year));
    }

    public void Setup()
    {
        SoundManager.Instance.PlayClip(SoundManager.Instance.MusicSource, SoundManager.Instance.MusicCollection.clips[3], true);
        for (int i = 0; i < elementsToAdd.Count; i++)
        {
            OneTimeframe newElement = Instantiate(oneTimeframe, parentTransform);

            if (elementsToAdd[i].IsCorrupted)
                newElement.Init(corruptedSprite, elementsToAdd[i].IsCorrupted, elementsToAdd[i].MinigameData, elementsToAdd[i].Year);
            else
                newElement.Init(normalSprites[Random.Range(0, normalSprites.Count)], elementsToAdd[i].IsCorrupted, elementsToAdd[i].MinigameData, elementsToAdd[i].Year);
            
            elementsAddedOnScene.Add(newElement);
        }

        UpdateTimeline();
        MainGameController.Instance.DateController.UpdateDate(int.Parse(elementsAddedOnScene[currentTimeFrame].Year));
    }

    public void SetCurrentBackgroundImage()
    {
        if (elementsAddedOnScene[currentTimeFrame].IsCorrupted)
        {
            MainGameController.Instance.AddictionalMethods.FadeElement(1f, mainBackground, 1, 0, null);
        }
        else
        {
            mainBackground.sprite = elementsToAdd[currentTimeFrame].UnlockedBackground;
            MainGameController.Instance.AddictionalMethods.FadeElement(1f, mainBackground, 0, 1, null);
        }
    }
    private void UpdateTimeline()
    {
        foreach (int indexes in MainGameController.Instance.FinishedMinigames)
        {
            elementsAddedOnScene[indexes].UnlockTimeFrame(normalSprites[Random.Range(0, normalSprites.Count)]);
        }
    }

}

[System.Serializable]
public class OneElementData
{
    [SerializeField]
    private bool isCorrupted;

    [SerializeField]
    private AllMinigames minigameData;

    [SerializeField]
    private Sprite unlockedBackground;

    [SerializeField]
    private string year;

    public bool IsCorrupted { get => isCorrupted; set => isCorrupted = value; }
    public AllMinigames MinigameData { get => minigameData; set => minigameData = value; }
    public Sprite UnlockedBackground { get => unlockedBackground; set => unlockedBackground = value; }

    public string Year { get => year; set => year = value; }
}

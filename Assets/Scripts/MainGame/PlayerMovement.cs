using DG.Tweening;
using Histhack.Core;
using Histhack.Core.SaveLoadSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private AnimatedUI timelineBackground;

    [SerializeField]
    private AnimatedUI timelineBackground2;

    [SerializeField]
    private AnimatedUI buttonsAnimation;

    [SerializeField]
    private AnimatedUI playerAnimation;

    [SerializeField]
    private Vector2 movementValue;

    [SerializeField]
    private Vector2 teleportationValue;

    [SerializeField]
    private float differenceOffset;

    [SerializeField]
    private PlayerState playerState;

    private RectTransform firstTimelineBackground;

    private RectTransform secondTimelineBackground;

    private RectTransform playerTransform;

    private RectTransform buttonsTransform;

    private float movingDirection = 0;

    public float MovingDirection { get => movingDirection; }

    private string savePath = "ElementsPositions";

    private void Awake()
    {
        firstTimelineBackground = timelineBackground.RectMovementAnimationData[0].ObjectTransform;
        secondTimelineBackground = timelineBackground2.RectMovementAnimationData[0].ObjectTransform;
        playerTransform = playerAnimation.RectMovementAnimationData[0].ObjectTransform;
        buttonsTransform = buttonsAnimation.RectMovementAnimationData[0].ObjectTransform;

        MainGameController.Instance.GameEvents.OnSaveGame += SavePositions;
        MainGameController.Instance.GameEvents.OnLoadGame += LoadPositions;
        MainGameController.Instance.GameEvents.OnMinigameStart += StoreDataForMinigame;
        MainGameController.Instance.GameEvents.OnMinigameReturn += LoadPositionAfterMinigame;
    }


    private void OnDisable()
    {
        MainGameController.Instance.GameEvents.OnSaveGame -= SavePositions;
        MainGameController.Instance.GameEvents.OnLoadGame -= LoadPositions;
        MainGameController.Instance.GameEvents.OnMinigameStart -= StoreDataForMinigame;
        MainGameController.Instance.GameEvents.OnMinigameReturn -= LoadPositionAfterMinigame;
    }

    public void MovePlayer(int direction, TweenCallback tweenCallback)
    {
        playerAnimation.OnAnimationEnd += playerState.ReturnToNormalState;
        playerAnimation.OnAnimationEnd += (int _) => movingDirection = 0;

        movingDirection = direction;

        if (tweenCallback != null)
            playerAnimation.SetActionToStartAfterAnimationEnd(tweenCallback);

        Vector2 newPosition = playerTransform.anchoredPosition + movementValue * -direction;
        playerAnimation.StartRectMovementAnimationX(playerTransform.anchoredPosition.x, newPosition.x);

        if (direction < 0)
            playerState.RightMovementStart();
        else
            playerState.LeftMovementStart();
    }

    public void MoveBackground(int direction, TweenCallback tweenCallback)
    {

        timelineBackground2.OnAnimationEnd += playerState.ReturnToNormalState;
        timelineBackground2.OnAnimationEnd += (int _) => movingDirection = 0;

        movingDirection = direction;

        if (tweenCallback != null)
            timelineBackground2.SetActionToStartAfterAnimationEnd(tweenCallback);

        CheckForTeleportation(firstTimelineBackground, direction);
        CheckForTeleportation(secondTimelineBackground, direction);


        Vector2 newPosition = firstTimelineBackground.anchoredPosition + movementValue * direction;
        timelineBackground.StartRectMovementAnimation(firstTimelineBackground.anchoredPosition, newPosition);

        newPosition = secondTimelineBackground.anchoredPosition + movementValue * direction;
        timelineBackground2.StartRectMovementAnimation(secondTimelineBackground.anchoredPosition, newPosition);

        newPosition = buttonsTransform.anchoredPosition + movementValue * direction;
        buttonsAnimation.StartRectMovementAnimation(buttonsTransform.anchoredPosition, newPosition);

        if (direction < 0)
            playerState.RightMovementStart();
        else
            playerState.LeftMovementStart();
    }

    private void CheckForTeleportation(RectTransform transformToCheck, int direction)
    {
        if(direction > 0)
        {
            if (transformToCheck.anchoredPosition.x - teleportationValue.y >= -differenceOffset)
            {
                transformToCheck.anchoredPosition = new Vector2(teleportationValue.x, transformToCheck.anchoredPosition.y);
            }
        }
        else
        {
            if (transformToCheck.anchoredPosition.x - teleportationValue.x <= differenceOffset)
            {
                transformToCheck.anchoredPosition = new Vector2(teleportationValue.y, transformToCheck.anchoredPosition.y);
            }
        }
    }

    private void StoreDataForMinigame()
    {
        Debug.Log("store position");
        MainGameController.Instance.PlayerMovementData = new PlayerMovementData(firstTimelineBackground.anchoredPosition.x, secondTimelineBackground.anchoredPosition.x, playerTransform.anchoredPosition.x, buttonsTransform.anchoredPosition.x);
    }
    
    private void LoadPositionAfterMinigame()
    {
        Debug.Log("load position");
        PlayerMovementData data = MainGameController.Instance.PlayerMovementData;

        firstTimelineBackground.anchoredPosition = new Vector2(data.firstTimelineX, firstTimelineBackground.anchoredPosition.y);
        secondTimelineBackground.anchoredPosition = new Vector2(data.secondTimelineX, secondTimelineBackground.anchoredPosition.y);
        playerTransform.anchoredPosition = new Vector2(data.playerX, playerTransform.anchoredPosition.y);
        buttonsTransform.anchoredPosition = new Vector2(data.buttonsX, buttonsTransform.anchoredPosition.y);
    }

    private void SavePositions()
    {
        PlayerMovementData data = new PlayerMovementData(firstTimelineBackground.anchoredPosition.x, secondTimelineBackground.anchoredPosition.x, playerTransform.anchoredPosition.x, buttonsTransform.anchoredPosition.x);
        SaveSystem.SaveClass<PlayerMovementData>(data, savePath, SaveDirectories.Player);
    }

    private void LoadPositions()
    {
        if (SaveSystem.CheckIfFileExists(savePath, SaveDirectories.Player))
        {
            PlayerMovementData data = SaveSystem.LoadClass<PlayerMovementData>(savePath, SaveDirectories.Player);

            firstTimelineBackground.anchoredPosition = new Vector2(data.firstTimelineX, firstTimelineBackground.anchoredPosition.y);
            secondTimelineBackground.anchoredPosition = new Vector2(data.secondTimelineX, secondTimelineBackground.anchoredPosition.y);
            playerTransform.anchoredPosition = new Vector2(data.playerX, playerTransform.anchoredPosition.y);
            buttonsTransform.anchoredPosition = new Vector2(data.buttonsX, buttonsTransform.anchoredPosition.y);
        }
    }

}

public class PlayerMovementData
{
    public float firstTimelineX;
    public float secondTimelineX;
    public float playerX;
    public float buttonsX;

    public PlayerMovementData() { }
    public PlayerMovementData(float firstTimelineX, float secondTimelineX, float playerX, float buttonsX)
    {
        this.firstTimelineX = firstTimelineX;
        this.secondTimelineX = secondTimelineX;
        this.playerX = playerX;
        this.buttonsX = buttonsX;
    }
}

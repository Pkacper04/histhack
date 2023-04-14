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

    private RectTransform firstTimelineBackground;

    private RectTransform secondTimelineBackground;

    private string saveValue = "PlayerMovement";

    private void OnEnable()
    {
        firstTimelineBackground = timelineBackground.RectMovementAnimationData[0].ObjectTransform;
        secondTimelineBackground = timelineBackground2.RectMovementAnimationData[0].ObjectTransform;

        MainGameController.Instance.GameEvents.OnSaveGame += Save;
        MainGameController.Instance.GameEvents.OnLoadGame += Load;
    }

    private void OnDisable()
    {
        MainGameController.Instance.GameEvents.OnSaveGame -= Save;
        MainGameController.Instance.GameEvents.OnLoadGame -= Load;
    }

    public void Save()
    {
        float playerPosition = playerAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition.x;
        float firstTapePosition = firstTimelineBackground.anchoredPosition.x;
        float secondTapePosition = secondTimelineBackground.anchoredPosition.x;
        float buttonsPosition = buttonsAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition.x;
        PlayerMovementData player = new PlayerMovementData(playerPosition,firstTapePosition,secondTapePosition, buttonsPosition);
        SaveSystem.SaveClass<PlayerMovementData>(player, saveValue, SaveDirectories.Player);
    }

    public void Load()
    {
        if(SaveSystem.CheckIfFileExists(saveValue,SaveDirectories.Player))
        {
            PlayerMovementData player = SaveSystem.LoadClass<PlayerMovementData>(saveValue, SaveDirectories.Player);

            Vector2 playerStartPosition = playerAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition;
            Vector2 buttonsStartPosition = buttonsAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition;

            playerAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition = new Vector2(player.playerPositionX, playerStartPosition.y);
            firstTimelineBackground.anchoredPosition = new Vector2(player.firstTapePositionX, firstTimelineBackground.anchoredPosition.y);
            secondTimelineBackground.anchoredPosition = new Vector2(player.secondTapePositionX,secondTimelineBackground.anchoredPosition.y);
            buttonsAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition = new Vector2(player.buttonsPositionX, buttonsStartPosition.y);
        }
    }

    public void MovePlayer(int direction, TweenCallback tweenCallback)
    {
        if (tweenCallback != null)
            playerAnimation.SetActionToStartAfterAnimationEnd(tweenCallback);

        Vector2 newPosition = playerAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition + movementValue * -direction;
        playerAnimation.StartRectMovementAnimationX(playerAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition.x, newPosition.x);
    }

    public void MoveBackground(int direction, TweenCallback tweenCallback)
    {

        if(tweenCallback != null)
            timelineBackground2.SetActionToStartAfterAnimationEnd(tweenCallback);

        CheckForTeleportation(firstTimelineBackground, direction);
        CheckForTeleportation(secondTimelineBackground, direction);


        Vector2 newPosition = firstTimelineBackground.anchoredPosition + movementValue * direction;
        timelineBackground.StartRectMovementAnimation(firstTimelineBackground.anchoredPosition, newPosition);

        newPosition = secondTimelineBackground.anchoredPosition + movementValue * direction;
        timelineBackground2.StartRectMovementAnimation(secondTimelineBackground.anchoredPosition, newPosition);

        newPosition = buttonsAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition + movementValue * direction;
        buttonsAnimation.StartRectMovementAnimation(buttonsAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition, newPosition);
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
}

public class PlayerMovementData
{
    public float playerPositionX;
    public float firstTapePositionX;
    public float secondTapePositionX;
    public float buttonsPositionX;

    public PlayerMovementData() { }

    public PlayerMovementData(float playerPositionTemp, float firstTapePositionTemp, float secondTapePositionTemp, float buttonsPositionTemp)
    {
        this.playerPositionX = playerPositionTemp;
        this.firstTapePositionX = firstTapePositionTemp;
        this.secondTapePositionX = secondTapePositionTemp;
        this.buttonsPositionX = buttonsPositionTemp;
    }
}

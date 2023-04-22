using DG.Tweening;
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

    private float movingDirection = 0;

    public float MovingDirection { get => movingDirection; }

    private void Start()
    {
        firstTimelineBackground = timelineBackground.RectMovementAnimationData[0].ObjectTransform;
        secondTimelineBackground = timelineBackground2.RectMovementAnimationData[0].ObjectTransform;
    }


    public void MovePlayer(int direction, TweenCallback tweenCallback)
    {
        playerAnimation.OnAnimationEnd += playerState.ReturnToNormalState;
        playerAnimation.OnAnimationEnd += (int _) => movingDirection = 0;

        movingDirection = direction;

        if (tweenCallback != null)
            playerAnimation.SetActionToStartAfterAnimationEnd(tweenCallback);

        Vector2 newPosition = playerAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition + movementValue * -direction;
        playerAnimation.StartRectMovementAnimationX(playerAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition.x, newPosition.x);

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

        newPosition = buttonsAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition + movementValue * direction;
        buttonsAnimation.StartRectMovementAnimation(buttonsAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition, newPosition);

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
}

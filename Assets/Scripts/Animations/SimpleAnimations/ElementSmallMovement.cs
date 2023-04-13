using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSmallMovement : MonoBehaviour
{
    [SerializeField]
    private RectTransform objectToMove;

    [SerializeField]
    private Vector2 movementOffsets;

    [SerializeField]
    private float movementTime;

    private Vector2 startingPosition;

    private void Start()
    {
        startingPosition = objectToMove.anchoredPosition;
        MoveUp();
    }

    private void MoveUp()
    {
        objectToMove.DOAnchorPosY(startingPosition.y + movementOffsets.y, movementTime).OnComplete(() => MoveDown());
    }

    private void MoveDown()
    {
        objectToMove.DOAnchorPosY(startingPosition.y, movementTime).OnComplete(() => MoveUp());
    }

}

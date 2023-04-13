using Histhack.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotPuzzlePiece : MonoBehaviour
{
    [SerializeField]
    private int pieceId;

    [SerializeField]
    private RectTransform slotRectTransform;


    public void SelectSlot(DragPuzzlePiece puzzlePiece)
    {

        if (puzzlePiece.PieceID == pieceId)
        {
            puzzlePiece.SnapPiece(slotRectTransform);
            MainGameController.Instance.GameEvents.CallOnSlotFinished(this);
        }
        else
            puzzlePiece.ResetPosition();
    }
}

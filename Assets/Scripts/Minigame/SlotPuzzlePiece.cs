using Histhack.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotPuzzlePiece : MonoBehaviour
{
    [SerializeField]
    private int pieceId;

    [SerializeField]
    private RectTransform slotRectTransform;

    [SerializeField]
    private Vector2 positionOffset = Vector2.zero;

    private Image backgroundImage;

    public int PieceId { get => pieceId; set => pieceId = value; }


    private void Start()
    {
        backgroundImage = GetComponent<Image>();
    }

    public bool CheckSlot(DragPuzzlePiece puzzlePiece)
    {
        if (puzzlePiece.PieceID == pieceId)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetSlot(DragPuzzlePiece puzzlePiece)
    {
        puzzlePiece.SnapPiece(slotRectTransform, positionOffset);
        backgroundImage.color = new Color(0, 0, 0, 0);
    }
}

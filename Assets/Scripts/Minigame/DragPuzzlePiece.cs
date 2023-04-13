using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPuzzlePiece : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private RectTransform pieceRectTransform;

    [SerializeField]
    private Canvas pieceCanvas;

    [SerializeField]
    private CanvasGroup pieceCanvasGroup;

    [SerializeField]
    private int pieceID;

    [SerializeField]
    private bool returnToPositionAlways = false;

    private Vector2 startingPosition;

    private bool pieceSet = false;

    public int PieceID { get => pieceID; }

    private void Start()
    {
        startingPosition = pieceRectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (pieceSet)
            return;

        pieceCanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (pieceSet)
            return;

        pieceRectTransform.anchoredPosition += eventData.delta / pieceCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        List<SlotPuzzlePiece> puzzleSlots = new List<SlotPuzzlePiece>();

        foreach(RaycastResult result in results)
        {
            SlotPuzzlePiece tempSlot = null;

            if (result.gameObject.TryGetComponent<SlotPuzzlePiece>(out tempSlot))
                puzzleSlots.Add(tempSlot);
        }

        foreach(SlotPuzzlePiece onePiece in puzzleSlots)
        {
            onePiece.SelectSlot(this);
        }

        if (pieceSet)
            return;

        pieceCanvasGroup.blocksRaycasts = true;

        if (returnToPositionAlways) { 
            if(!pieceSet)
            {
                ResetPosition();
            }
        }
    }

    public void ResetPosition()
    {
        pieceRectTransform.anchoredPosition = startingPosition;
    }

    public void SnapPiece(RectTransform newParent)
    {
        pieceSet = true;
        pieceRectTransform.SetParent(newParent);
        pieceRectTransform.anchoredPosition = new Vector2(0, 0);
    }
}

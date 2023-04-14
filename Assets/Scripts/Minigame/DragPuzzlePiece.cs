using Histhack.Core;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEditor.ShaderGraph.Internal;
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

    [SerializeField]
    private Vector2 overrideStartingPosition = Vector2.zero;


    private Vector2 startingPosition;

    private bool pieceSet = false;

    public int PieceID { get => pieceID; set => pieceID = value; }

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

        SlotPuzzlePiece positionFound = null;
        SlotPuzzlePiece wrongPositionFound = null;

        foreach(SlotPuzzlePiece onePiece in puzzleSlots)
        {
            if (onePiece.CheckSlot(this))
            {
                positionFound = onePiece;
            }
            else
            {
                wrongPositionFound = onePiece;
            }
        }

        if(positionFound != null)
        {
            positionFound.SetSlot(this);
            MainGameController.Instance.GameEvents.CallOnSlotFinished(positionFound);
        }
        else if(wrongPositionFound != null)
        {
            ResetPosition();
            MainGameController.Instance.GameEvents.CallOnSlotWrongSelect(wrongPositionFound);
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
        if (overrideStartingPosition != Vector2.zero)
            pieceRectTransform.anchoredPosition = overrideStartingPosition;
        else
            pieceRectTransform.anchoredPosition = startingPosition;
    }

    public void SnapPiece(RectTransform newParent, Vector2 positionOffset)
    {
        pieceSet = true;
        pieceRectTransform.SetParent(newParent);

        Debug.Log("possition offset: "+ pieceRectTransform.anchoredPosition);

        pieceRectTransform.anchoredPosition = positionOffset;
    }
}

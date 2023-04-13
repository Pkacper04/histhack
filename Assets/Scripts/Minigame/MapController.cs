using Histhack.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private List<SlotPuzzlePiece> slots = new List<SlotPuzzlePiece>();

    private List<SlotPuzzlePiece> currentSlots = new List<SlotPuzzlePiece>();

    private void Start()
    {
        currentSlots = slots;
    }

    private void OnEnable()
    {
        MainGameController.Instance.GameEvents.OnSlotFinished += OnSlotFinished;
    }

    private void OnDisable()
    {
        MainGameController.Instance.GameEvents.OnSlotFinished -= OnSlotFinished;
    }

    private void OnSlotFinished(SlotPuzzlePiece slotFinished)
    {
        currentSlots.Remove(slotFinished);

        CheckIfAllSlotsFinished();
    }

    private void CheckIfAllSlotsFinished()
    {
        if(currentSlots.Count == 0)
        {
            Debug.Log("all slots finished");
        }
    }
}

using Histhack.Core;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private ScreenShake screenShake;

    [SerializeField]
    private List<SlotPuzzlePiece> slots = new List<SlotPuzzlePiece>();

    [SerializeField]
    private ErrorCounter errorCounter;

    private List<SlotPuzzlePiece> currentSlots = new List<SlotPuzzlePiece>();

    private int wrongAnswersCounter = 0;

    private MapMinigame mapMinigame;

    private void Start()
    {
        currentSlots = new List<SlotPuzzlePiece>(slots);
    }

    private void OnEnable()
    {
        MainGameController.Instance.GameEvents.OnSlotFinished += OnSlotFinished;
        MainGameController.Instance.GameEvents.OnSlotWrongSelect += OnSlotWrongSelect;
    }

    private void OnDisable()
    {
        MainGameController.Instance.GameEvents.OnSlotFinished -= OnSlotFinished;
        MainGameController.Instance.GameEvents.OnSlotWrongSelect -= OnSlotWrongSelect;
    }

    public void Init(MapMinigame mapMinigame)
    {
        wrongAnswersCounter = 0;
        this.mapMinigame = mapMinigame;

        errorCounter.MaxErrors = mapMinigame.MaxErrors;
        errorCounter.ChangeErrorText(0);
    }

    private void OnSlotFinished(SlotPuzzlePiece slotFinished)
    {
        if (!currentSlots.Contains(slotFinished))
            return;

        currentSlots.Remove(slotFinished);

        CheckIfAllSlotsFinished();
    }

    private void OnSlotWrongSelect(SlotPuzzlePiece slotFinished)
    {
        if (!slots.Contains(slotFinished))
            return;

        wrongAnswersCounter++;

        errorCounter.ChangeErrorText(wrongAnswersCounter);

        screenShake.StartShake();

        Debug.Log("map answers counter: "+wrongAnswersCounter + " max errors: "+mapMinigame.MaxErrors);

        if (wrongAnswersCounter == mapMinigame.MaxErrors)
        {
            mapMinigame.FinishMinigame(false);
        }
    }


    private void CheckIfAllSlotsFinished()
    {
        if(currentSlots.Count == 0)
        {
            mapMinigame.FinishMinigame(true);
        }
    }
}

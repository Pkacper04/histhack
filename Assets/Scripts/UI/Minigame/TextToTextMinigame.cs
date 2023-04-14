using Histhack.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextToTextMinigame : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup minigameCanvas;

    [SerializeField]
    private List<SlotPuzzlePiece> slots = new List<SlotPuzzlePiece>();

    [SerializeField]
    private List<DragPuzzlePiece> dragElements = new List<DragPuzzlePiece>();

    [SerializeField]
    private ScreenShake screenShake;

    [SerializeField]
    private ErrorCounter errorCounter;

    [SerializeField]
    private int maxErrors;

    private List<TMP_Text> dragTexts = new List<TMP_Text>();
    private List<TMP_Text> slotTexts = new List<TMP_Text>();

    private MinigameController minigameController;

    private List<SlotPuzzlePiece> currentSlots = new List<SlotPuzzlePiece>();

    private int currentErrors = 0;

    private void Awake()
    {
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(minigameCanvas);
    }

    private void OnEnable()
    {
        MainGameController.Instance.GameEvents.OnSlotFinished += OnSlotSelected;
        MainGameController.Instance.GameEvents.OnSlotWrongSelect += OnWrongSlotSelected;
    }

    private void OnDisable()
    {
        MainGameController.Instance.GameEvents.OnSlotFinished -= OnSlotSelected;
        MainGameController.Instance.GameEvents.OnSlotWrongSelect -= OnWrongSlotSelected;
    }

    public void Init(MinigameController minigameController)
    {
        this.minigameController = minigameController;
    }

    private void InitText()
    {
        foreach (DragPuzzlePiece drag in dragElements)
        {
            TMP_Text temp = drag.GetComponentInChildren<TMP_Text>();
            dragTexts.Add(temp);
        }

        foreach(SlotPuzzlePiece slot in slots)
        {
            TMP_Text temp = slot.GetComponentInParent<TMP_Text>();
            slotTexts.Add(temp);
        }
    }

    public void SetupQuestions(List<Pseudonyms> pseudonyms)
    {
        InitText();

        currentSlots = new List<SlotPuzzlePiece>(slots);

        currentErrors = 0;

        errorCounter.MaxErrors = maxErrors;
        errorCounter.ChangeErrorText(currentErrors);

        slotTexts[0].text = pseudonyms[0].question;
        slotTexts[1].text = pseudonyms[1].question;
        slotTexts[2].text = pseudonyms[2].question;

        slots[0].PieceId = 0;
        slots[1].PieceId = 1;
        slots[2].PieceId = 2;

        List<Pseudonyms> tempPseudonyms = new List<Pseudonyms>(pseudonyms);

        int randomIndex = Random.Range(0, tempPseudonyms.Count);

        dragElements[0].PieceID = SetID(tempPseudonyms[randomIndex]);
        dragTexts[0].text = tempPseudonyms[randomIndex].answer;

        tempPseudonyms.RemoveAt(randomIndex);


        randomIndex = Random.Range(0, tempPseudonyms.Count);

        dragElements[1].PieceID = SetID(tempPseudonyms[randomIndex]);
        dragTexts[1].text = tempPseudonyms[randomIndex].answer;

        tempPseudonyms.RemoveAt(randomIndex);


        dragElements[2].PieceID = SetID(tempPseudonyms[0]);
        dragTexts[2].text = tempPseudonyms[0].answer;

        MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(minigameCanvas);
    }

    private int SetID(Pseudonyms oneQuiz)
    {
        for (int i = 0; i < slotTexts.Count; i++)
        {
            if (slotTexts[i].text == oneQuiz.question)
                return slots[i].PieceId;
        }

        return 0;
    }

    private void OnSlotSelected(SlotPuzzlePiece slotPuzzlePiece)
    {
        if (!currentSlots.Contains(slotPuzzlePiece))
            return;

        currentSlots.Remove(slotPuzzlePiece);

        if (currentSlots.Count == 0)
        {
            minigameController.OnMinigameFinish(true);
        }
    }

    private void OnWrongSlotSelected(SlotPuzzlePiece slotPuzzlePiece)
    {
        if (!slots.Contains(slotPuzzlePiece))
            return;

        currentErrors += 1;

        errorCounter.ChangeErrorText(currentErrors);
        screenShake.StartShake();

        if (currentErrors == maxErrors)
        {
            BlockMinigame();
            minigameController.OnMinigameFinish(false);
        }
    }

    private void BlockMinigame()
    {
        minigameCanvas.interactable = false;
        minigameCanvas.blocksRaycasts = false;
    }
}

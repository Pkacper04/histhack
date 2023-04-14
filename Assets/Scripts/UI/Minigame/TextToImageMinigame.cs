using Histhack.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextToImageMinigame : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup minigameCanvas;

    [SerializeField]
    private List<SlotPuzzlePiece> slots = new List<SlotPuzzlePiece>();

    [SerializeField]
    private List<DragPuzzlePiece> dragElements = new List<DragPuzzlePiece>();

    [SerializeField] 
    private List<Image> personsImages = new List<Image>();

    [SerializeField]
    private ScreenShake screenShake;

    [SerializeField]
    private ErrorCounter errorCounter;

    [SerializeField]
    private int maxErrors;

    private List<TMP_Text> dragText = new List<TMP_Text>();

    private MinigameController minigameController;

    private List<SlotPuzzlePiece> currentSlots = new List<SlotPuzzlePiece>();

    private int currentErrors = 0;

    private void Awake()
    {
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(minigameCanvas);
    }

    private void Start()
    {
        foreach(DragPuzzlePiece drag in dragElements)
        {
            TMP_Text temp = drag.GetComponentInChildren<TMP_Text>();
            dragText.Add(temp);
        }
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

    public void SetupQuestions(List<PicturesQuiz> picturesQuiz)
    {
        currentErrors = 0;

        errorCounter.MaxErrors = maxErrors;
        errorCounter.ChangeErrorText(0);

        currentSlots = new List<SlotPuzzlePiece>(slots);
        personsImages[0].sprite = picturesQuiz[0].picture;
        personsImages[1].sprite = picturesQuiz[1].picture;
        personsImages[2].sprite = picturesQuiz[2].picture;

        slots[0].PieceId = 0;
        slots[1].PieceId = 1;
        slots[2].PieceId = 2;

        List<PicturesQuiz> tempPictures = new List<PicturesQuiz>(picturesQuiz);

        int randomIndex = Random.Range(0, tempPictures.Count);

        dragElements[0].PieceID = SetID(tempPictures[randomIndex]);
        dragText[0].text = tempPictures[randomIndex].asnwer;

        tempPictures.RemoveAt(randomIndex);


        randomIndex = Random.Range(0, tempPictures.Count);

        dragElements[1].PieceID = SetID(tempPictures[randomIndex]);
        dragText[1].text = tempPictures[randomIndex].asnwer;

        tempPictures.RemoveAt(randomIndex);


        dragElements[2].PieceID = SetID(tempPictures[0]);
        dragText[2].text = tempPictures[0].asnwer;

        MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(minigameCanvas);
    }

    private int SetID(PicturesQuiz oneQuiz)
    {
        for(int i=0; i<personsImages.Count; i++)
        {
            if (personsImages[i].sprite == oneQuiz.picture)
                return slots[i].PieceId;
        }

        return 0;
    }

    private void OnSlotSelected(SlotPuzzlePiece slotPuzzlePiece)
    {
        if (!currentSlots.Contains(slotPuzzlePiece))
            return;

        currentSlots.Remove(slotPuzzlePiece);

        if(currentSlots.Count == 0)
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

using Histhack.Core;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMinigame : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup minigameCanvas;

    [SerializeField]
    private MapController firstMap;

    [SerializeField]
    private MapController secondMap;

    [SerializeField]
    private int maxErrors = 3;

    private MinigameController minigameController;

    public int MaxErrors { get => maxErrors; }

    private void Awake()
    {
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(minigameCanvas);
    }

    public void Init(MinigameController minigameController)
    {
        this.minigameController = minigameController;
    }

    public void SetupQuestions(int mapNumber)
    {
        firstMap.Init(this);

        if(mapNumber == 0)
            firstMap.gameObject.SetActive(true);
        else
            secondMap.gameObject.SetActive(true);

        MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(minigameCanvas);
    }

    public void FinishMinigame(bool status)
    {
        BlockScreen();
        minigameController.OnMinigameFinish(status);
    }

    private void BlockScreen()
    {
        minigameCanvas.blocksRaycasts = false;
        minigameCanvas.interactable = false;
    }

}

using DG.Tweening;
using Histhack.Core;
using Histhack.Core.SaveLoadSystem;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialoguesController : MonoBehaviour
{

    private Template_UIManager manager;

    [SerializeField]
    private List<VIDE_Assign> VIDES;

    [SerializeField, Scene]
    private string minigameScene;

    private int currentDialogue = 0;

    private bool firstDialogueStarted = false;

    private string SavePath = "Dialogues";


    public List<VIDE_Assign> VIDESProperty { get => VIDES; }

    public int CurrentDialogue { get => currentDialogue; set => currentDialogue = value; }
    public bool FirstDialogueStarted { get => firstDialogueStarted; set => firstDialogueStarted = value; }

    public void Init()
    {
        manager = FindObjectOfType<Template_UIManager>();
    }

    private void OnEnable()
    {
        MainGameController.Instance.GameEvents.OnGameDailogueStart += StartDialogue;
    }

    private void OnDisable()
    {
        MainGameController.Instance.GameEvents.OnGameDailogueStart -= StartDialogue;
    }

    public void StartDialogue()
    {
        if (currentDialogue == VIDES.Count)
            return;

        MainGameController.Instance.PostprocessManager.ChangePostProcess(Histhack.Core.Effects.PostProcessesToChange.DepthOfField, true);

        manager.Interact(VIDES[currentDialogue]);
    }

    public void ChangeCurrentDialogue()
    {
        currentDialogue++;
    }

    public void LoadMinigame()
    {
        MainGameController.Instance.PostprocessManager.ChangePostProcess(Histhack.Core.Effects.PostProcessesToChange.DepthOfField, false);
        MainGameController.Instance.StartTransition(AnimationTypes.AnchoreMovement,() => SceneManager.LoadScene(minigameScene));
    }

    public void FirstDialogue()
    {
        MainGameController.Instance.PostprocessManager.ChangePostProcess(Histhack.Core.Effects.PostProcessesToChange.DepthOfField, false);
        firstDialogueStarted = true;
    }

    public void SaveCurrentDialogue()
    {
        DialoguesData data = new DialoguesData(currentDialogue, firstDialogueStarted);
        SaveSystem.SaveClass<DialoguesData>(data, SavePath, SaveDirectories.Player);
    }

    public void LoadCurrentDialogue()
    {
        if (SaveSystem.CheckIfFileExists(SavePath, SaveDirectories.Player))
        {
            DialoguesData data = SaveSystem.LoadClass<DialoguesData>(SavePath, SaveDirectories.Player);

            currentDialogue = data.currentDialogue;
            firstDialogueStarted = data.firstDialogueStarted;
        }
    }

}

public class DialoguesData
{

    public int currentDialogue;
    public bool firstDialogueStarted;

    public DialoguesData() { }

    public DialoguesData(int currentDialogue, bool firstDialogueStarted)
    {
        this.currentDialogue = currentDialogue;
        this.firstDialogueStarted = firstDialogueStarted;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;
using System;
using Histhack.Core;
using UnityEngine.UI;
using Histhack.Core.SaveLoadSystem;

public class MainMenuController : MonoBehaviour
{
    [SerializeField, Scene]
    private string gameSceneToLoad;

    [SerializeField, BoxGroup("Menu Animation")]
    private Animator menuAnimator;

    [SerializeField, AnimatorParam(nameof(menuAnimator)), BoxGroup("Menu Animation")]
    private string animationParam;

    [SerializeField, BoxGroup("Panels")]
    private CanvasGroup creditsCanvasGroup;

    [SerializeField, BoxGroup("Buttons")]
    private Button continueButton;


    private void Start()
    {
        SetupMenu();
    }

    private void SetupMenu()
    {
        DeactivateOtherPanels();
        DeactivateButtons();
    }

    private void DeactivateOtherPanels()
    {
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(creditsCanvasGroup);
    }

    private void DeactivateButtons()
    {
        continueButton.gameObject.SetActive(SaveSystem.CheckIfSaveExists());
    }

    #region MainMenuButtons

    public void Continue()
    {
        SceneManager.LoadScene(gameSceneToLoad);
    }

    public void StartGame()
    {
        SaveSystem.DeleteAllSaves();
        SceneManager.LoadScene(gameSceneToLoad);
    }

    public void Settings()
    {
        //TODO
    }

    public void Credits()
    {
        if (!menuAnimator.GetBool(animationParam))
            SlideCreditsIn();
        else
            SlideCreditsOut();
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    #endregion

    #region Animations

    private void SlideCreditsIn()
    {
        MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(creditsCanvasGroup);
        menuAnimator.SetBool(animationParam, true);
    }

    private void SlideCreditsOut()
    {
        menuAnimator.SetBool(animationParam, false);
    }

    private void DeactivateCredits()
    {
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(creditsCanvasGroup);
    }
    #endregion Animations
}

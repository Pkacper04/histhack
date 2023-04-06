using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;
using System;
using Histhack.Core;

public class MainMenuController : MonoBehaviour
{
    [SerializeField, Scene]
    private string gameSceneToLoad;

    [SerializeField]
    private Animator menuAnimator;

    [SerializeField, AnimatorParam(nameof(menuAnimator))]
    private string animationParam;

    [SerializeField]
    private CanvasGroup creditsCanvasGroup;


    private void Start()
    {
        DeactivateOtherPanels();
    }

    private void DeactivateOtherPanels()
    {
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(creditsCanvasGroup);
    }

    #region MainMenuButtons

    public void ContinueGame()
    {
        //TODO
    }

    public void StartGame()
    {
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

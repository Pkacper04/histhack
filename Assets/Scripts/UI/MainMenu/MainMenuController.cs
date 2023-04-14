using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;
using System;
using Histhack.Core;
using UnityEngine.UI;
using Histhack.Core.SaveLoadSystem;
using TMPro;
using Histhack.Core.Settings;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using Managers.Sounds;

public class MainMenuController : MonoBehaviour
{
    [SerializeField, Scene]
    private string gameSceneToLoad;

    [SerializeField, Scene]
    private string afterLoadingScene;

    
    [SerializeField, BoxGroup("MainMenuAnimations")]
    private AnimatedUI buttonsBackgroundAnimation;

    [SerializeField, BoxGroup("MainMenuAnimations")]
    private AnimatedUI buttonsAnimation;

    [SerializeField, BoxGroup("MainMenuAnimations")]
    private AnimatedUI creditsAnimation;

    [SerializeField, BoxGroup("MainMenuAnimations")]
    private AnimatedUI SettingsAnimation;


    [SerializeField, BoxGroup("Panels")]
    private CanvasGroup creditsCanvasGroup;

    [SerializeField, BoxGroup("Panels")]
    private CanvasGroup settingsCanvasGroup;

    [SerializeField, BoxGroup("Panels")]
    private CanvasGroup buttonsCanvasGroup;


    [SerializeField, BoxGroup("MainMenuButtons")]
    private Button continueButton;

    [SerializeField]
    private InfoDisplayerBlock infoDisplayer;

    private List<Button> mainMenuButtons = new List<Button>();


    #region Initialization

    private void Start()
    {
        mainMenuButtons = new List<Button>(buttonsAnimation.GetComponentsInChildren<Button>());

        SetupMenu();
        SlideMainPanelIn();
    }

    private void SetupMenu()
    {
        DeactivateOtherPanels();
        DeactivateButtons();
    }

    private void DeactivateOtherPanels()
    {
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(creditsCanvasGroup);
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(settingsCanvasGroup);
    }

    private void DeactivateButtons()
    {
        continueButton.gameObject.SetActive(SaveSystem.CheckIfSaveExists());
    }

    private void ChangeInteractableAllMenuButtons(bool newButtonStatus)
    {
        foreach(Button oneButton in mainMenuButtons)
        {
            oneButton.interactable = newButtonStatus;
        }
    }

    #endregion Initialization

    public void StartNewGame()
    {
        MainGameController.Instance.NextSceneToLoad = afterLoadingScene;
        SaveSystem.DeleteAllSaves();

        MainGameController.Instance.StartTransition(AnimationTypes.AnchoreMovement, () => SceneManager.LoadScene(gameSceneToLoad));
    }

    private void ShowInfoDisplayer()
    {
        infoDisplayer.AnimatedUI.StartRectMovementAnimation(new Vector2(1920, 0), new Vector2(0, 0), 0);
        infoDisplayer.ShowPanel();
    }

    private void HideInfoDisplayer()
    {
        infoDisplayer.AnimatedUI.SetActionToStartAfterAnimationEnd(() => SlideMainPanelIn());

        infoDisplayer.AnimatedUI.StartRectMovementAnimation(new Vector2(0, 0), new Vector2(-1920, 0), 1);
    }


    #region MainMenuButtons

    public void Continue()
    {
        SoundManager.Instance.PlayOneShoot(SoundManager.Instance.UISource, SoundManager.Instance.UICollection.clips[1], 0f);
        SceneManager.LoadScene(gameSceneToLoad);
    }

    public void StartGame()
    {
        SoundManager.Instance.PlayOneShoot(SoundManager.Instance.UISource, SoundManager.Instance.UICollection.clips[1], 0f);

        if (SaveSystem.CheckIfSaveExists())
        {
            infoDisplayer.InitInfoDisplayer("Jesteœ pewny, ¿e chcesz zacz¹æ now¹ grê ?", "Tak", "Nie", () => StartNewGame(), () => HideInfoDisplayer());

            SlideMainPanelOut(() => ShowInfoDisplayer());
        }
        else
        {
            MainGameController.Instance.NextSceneToLoad = afterLoadingScene;

            MainGameController.Instance.StartTransition(AnimationTypes.AnchoreMovement, () => SceneManager.LoadScene(gameSceneToLoad));
        }

    }


    public void Settings()
    {
       

        if (!settingsCanvasGroup.interactable)
            SlideMainPanelOut(() => SlideSettingsIn());
        else
            SlideSettingsOut(() => SlideMainPanelIn());
    }

    public void Credits()
    {
        SoundManager.Instance.PlayOneShoot(SoundManager.Instance.UISource, SoundManager.Instance.UICollection.clips[1], 0f);

        if (!creditsCanvasGroup.interactable)
            SlideMainPanelOut(() => SlideCreditsIn());
        else
            SlideCreditsOut(() => SlideMainPanelIn());
    }

    public void ExitGame()
    {
        SoundManager.Instance.PlayOneShoot(SoundManager.Instance.UISource, SoundManager.Instance.UICollection.clips[1], 0f);

        infoDisplayer.InitInfoDisplayer("Jesteœ pewny, ¿e chcesz wyjœæ z gry ?", "Tak", "Nie", () => Application.Quit(), () => HideInfoDisplayer());
        SlideMainPanelOut(() => ShowInfoDisplayer());
    }


    #endregion

    #region Animations

    [Button("animate menu In")]
    private void SlideMainPanelIn()
    {
        DeactivateOtherPanels();
        ChangeInteractableAllMenuButtons(false);

        buttonsBackgroundAnimation.SetActionToStartAfterAnimationEnd(() => buttonsAnimation.StartRectMovementAnimation(new Vector2(0f, -996f), new Vector2(0, 0), 0));
        buttonsAnimation.SetActionToStartAfterAnimationEnd(() => ChangeInteractableAllMenuButtons(true));

        buttonsBackgroundAnimation.StartRectMovementAnimation(new Vector2(0f,-10f),new Vector2(0f,2250f), 0);
    }

    [Button("animate menu out")]
    private void SlideMainPanelOut(TweenCallback tweenCallback)
    {
        ChangeInteractableAllMenuButtons(false);

        buttonsAnimation.SetActionToStartAfterAnimationEnd(() => buttonsBackgroundAnimation.StartRectMovementAnimation(new Vector2(0, 2250f), new Vector2(0f, 4500), 1));

        if(tweenCallback != null)
        {
            buttonsBackgroundAnimation.SetActionToStartAfterAnimationEnd(tweenCallback);
        }

        buttonsAnimation.StartRectMovementAnimation(new Vector2(0, 0), new Vector2(0f, 996f), 1);
    }

    private void SlideCreditsIn()
    {
        creditsAnimation.StartRectMovementAnimation(new Vector2(1920, 0), new Vector2(0, 0), 0);
        MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(creditsCanvasGroup);
    }

    private void SlideCreditsOut(TweenCallback tweenCallback)
    {
        if(tweenCallback != null)
        {
            creditsAnimation.SetActionToStartAfterAnimationEnd(tweenCallback);
        }

        creditsAnimation.StartRectMovementAnimation(new Vector2(0, 0), new Vector2(-1920, 0), 1);
    }

    private void SlideSettingsIn()
    {
        SettingsAnimation.StartRectMovementAnimation(new Vector2(1920, 0), new Vector2(0, 0), 0);
        MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(settingsCanvasGroup);
    }

    private void SlideSettingsOut(TweenCallback tweenCallback)
    {
        if(tweenCallback != null)
        {
            SettingsAnimation.SetActionToStartAfterAnimationEnd(tweenCallback);
        }

        SettingsAnimation.StartRectMovementAnimation(new Vector2(0,0), new Vector2(-1920,0), 1);
    }

    #endregion Animations

}

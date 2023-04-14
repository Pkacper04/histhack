using DG.Tweening;
using Histhack.Core;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField,Scene]
    private string loadingScene;

    [SerializeField, Scene]
    private string sceneAfterLoad;

    [SerializeField, Scene]
    private string finishScene;

    [SerializeField]
    private CanvasGroup pauseMenuGroup;

    [SerializeField]
    private KeyCode keyToPause;

    [SerializeField]
    private AnimatedUI pauseMenuAnimatedUI;

    [SerializeField]
    private CanvasGroup settingsMenuGroup;

    [SerializeField]
    private AnimatedUI settingsAnimatedUI;

    [SerializeField]
    private AnimatedUI saveAnimatedUI;

    [SerializeField]
    private SpriteSwitch savingSpriteSwitch;

    [SerializeField]
    private CanvasGroup savingCanvasGroup;

    [SerializeField]
    private Button nextButton;

    private bool pauseActive = false;

    public bool PauseActive { get => pauseActive; }


    private void Start()
    {
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(pauseMenuGroup);
        StopPauseEffectsStart();
        DeactivateOtherPanels();
    }
    private void Awake()
    {
        DeactivateButtonNext();
        MainGameController.Instance.GameEvents.OnGameFinish += ActivateButtonNext;
    }

    private void OnDisable()
    {
        MainGameController.Instance.GameEvents.OnGameFinish -= ActivateButtonNext;
    }

    private void DeactivateOtherPanels()
    {
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(settingsMenuGroup);
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(savingCanvasGroup);
    }

    public void StartPause()
    {
        Time.timeScale = 0;
        pauseActive = true;
        MainGameController.Instance.PostprocessManager.ChangePostProcess(Histhack.Core.Effects.PostProcessesToChange.DepthOfField, true);

        pauseMenuAnimatedUI.StartRectMovementAnimation(new Vector2(-600,0),new Vector2(0,0),0);
        MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(pauseMenuGroup);
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(settingsMenuGroup);
    }


    public void StopPause(TweenCallback tweenCallback)
    {
        if(tweenCallback != null)
            pauseMenuAnimatedUI.SetActionToStartAfterAnimationEnd(tweenCallback);

        pauseMenuAnimatedUI.StartRectMovementAnimation(new Vector2(0, 0), new Vector2(-600, 0), 1);
    }

    private void StopPauseEffects()
    {
        Time.timeScale = 1;
        pauseActive = false;
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(pauseMenuGroup);
        MainGameController.Instance.PostprocessManager.ChangePostProcess(Histhack.Core.Effects.PostProcessesToChange.DepthOfField, false);
    }

    private void StopPauseEffectsStart()
    {
        Time.timeScale = 1;
        pauseActive = false;
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(pauseMenuGroup);
    }

    private void Update()
    {
        if(Input.GetKeyDown(keyToPause))
        {
            if (pauseActive)
                StopPause(() => StopPauseEffects());
            else
                StartPause();
        }
    }


    #region PauseMenuButtons

    public void Resume()
    {
        StopPause(() => StopPauseEffects());
    }

    public void Settings()
    {
        if (settingsMenuGroup.interactable)
        {
            SettingsAnimationOut(() => StartPause());
        }
        else
        {
            pauseMenuAnimatedUI.SetActionToStartAfterAnimationEnd(() => SettingsAnimationIn());
            pauseMenuAnimatedUI.StartRectMovementAnimation(new Vector2(0, 0), new Vector2(-600, 0), 1);
        }
    }

    public void Save()
    {
        MainGameController.Instance.DataManager.SaveGame();
        StartCoroutine(WaitForSave());
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        MainGameController.Instance.PostprocessManager.ChangePostProcess(Histhack.Core.Effects.PostProcessesToChange.DepthOfField, false);
        MainGameController.Instance.NextSceneToLoad = sceneAfterLoad;
        MainGameController.Instance.WaitForInputAfterLoad = false;

        MainGameController.Instance.StartTransition(AnimationTypes.AnchoreMovement, () => SceneManager.LoadScene(loadingScene));
    }

    #endregion PauseMenuButtons


    private IEnumerator WaitForSave()
    {
        saveAnimatedUI.StartRectMovementAnimation(new Vector2(-175f, -62.5f), new Vector2(-175f, 62.5f), 0);
        MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(savingCanvasGroup);
        savingSpriteSwitch.StartAnimation();

        yield return new WaitForSecondsRealtime(3);

        saveAnimatedUI.SetActionToStartAfterAnimationEnd(() => MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(savingCanvasGroup));

        saveAnimatedUI.StartRectMovementAnimation(new Vector2(-175f, 62.5f), new Vector2(-175f, -62.5f), 1);
        savingSpriteSwitch.StopAnimation();
    }

    private void SettingsAnimationIn()
    {
        settingsAnimatedUI.StartRectMovementAnimation(new Vector2(1920,0),new Vector2(0,0),0);
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(pauseMenuGroup);
        MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(settingsMenuGroup);
    }

    private void SettingsAnimationOut(TweenCallback tweenCallback)
    {
        if (tweenCallback != null)
            settingsAnimatedUI.SetActionToStartAfterAnimationEnd(tweenCallback);

        settingsAnimatedUI.StartRectMovementAnimation(new Vector2(0, 0), new Vector2(-1920, 0), 1);
    }

    public void FinishGame()
    {
        MainGameController.Instance.StartTransition(AnimationTypes.AnchoreMovement, () => SceneManager.LoadScene(finishScene));
    }

    public void ActivateButtonNext()
    {
        nextButton.interactable = true;
    }

    public void DeactivateButtonNext()
    {
        nextButton.interactable = false;
    }
}

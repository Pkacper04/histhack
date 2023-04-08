using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;
using System;
using Histhack.Core;
using UnityEngine.UI;
using Histhack.Core.SaveLoadSystem;
using TMPro;
using Histhack.Core.Settings;

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

    [SerializeField, BoxGroup("Panels")]
    private CanvasGroup settingsCanvasGroup;


    [SerializeField, BoxGroup("MainMenuButtons")]
    private Button continueButton;


    [SerializeField, BoxGroup("Setting Buttons")]
    private Slider masterVolumeSlider;

    [SerializeField, BoxGroup("Setting Buttons")]
    private Slider musicVolumeSlider;

    [SerializeField, BoxGroup("Setting Buttons")]
    private Slider effectsVolumeSlider;

    [SerializeField, BoxGroup("Setting Buttons")]
    private TMP_Dropdown resolutionDropdown;

    [SerializeField, BoxGroup("Setting Buttons")]
    private Toggle fullscreenToggle;


    #region Initialization

    private void Start()
    {
        SetupMenu();
    }

    private void SetupMenu()
    {
        DeactivateOtherPanels();
        DeactivateButtons();
        SetupSettings();
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

    #endregion Initialization


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
        if (settingsCanvasGroup.interactable)
        {
            MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(settingsCanvasGroup);
        }
        else
        {
            MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(settingsCanvasGroup);
        }
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

    #region SettingsPublicMethods

    public void SaveSettings()
    {
        MainGameController.Instance.SettingsController.SaveSettings();
    }

    public void VolumeChanged(Slider sliderChanged)
    {
        SoundTypes soundType = ChooseSoundType(sliderChanged);

        MainGameController.Instance.SettingsController.ChangeVolumeOfSounds(soundType, sliderChanged.value);
    }

    public void SetNewResolution()
    {
        string newResolution = resolutionDropdown.options[resolutionDropdown.value].text;

        MainGameController.Instance.SettingsController.ChangeGameResolution(newResolution);
    }

    public void SetFullscreen()
    {
        MainGameController.Instance.SettingsController.SetFullScreen(fullscreenToggle.isOn);
    }

    #endregion SettingsPublicMethods


    #region SettingsAdditionalMethods

    private void SetupSettings()
    {
        SettingsController settingsController = MainGameController.Instance.SettingsController;

        settingsController.SetupResolutions(resolutionDropdown);

        masterVolumeSlider.value = settingsController.MasterVolumeValue;
        VolumeChanged(masterVolumeSlider);

        musicVolumeSlider.value = settingsController.MusicVolumeValue;
        VolumeChanged(musicVolumeSlider);

        effectsVolumeSlider.value = settingsController.EffectsVolumeValue;
        VolumeChanged(effectsVolumeSlider);

        fullscreenToggle.isOn = settingsController.FullscreenActive;

        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            if (resolutionDropdown.options[i].text == settingsController.ScreenResolution)
            {
                resolutionDropdown.value = i;
                break;
            }
        }
    }

    private SoundTypes ChooseSoundType(Slider slider)
    {
        if (slider == masterVolumeSlider)
            return SoundTypes.Master;
        else if (slider == musicVolumeSlider)
            return SoundTypes.Music;
        else
            return SoundTypes.Effects;
    }

    #endregion SettingsAdditionalMethods

}

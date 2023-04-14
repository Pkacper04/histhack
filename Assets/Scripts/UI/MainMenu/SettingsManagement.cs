using Histhack.Core.Settings;
using Managers.Sounds;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Histhack.Core.UI
{
    public class SettingsManagement : MonoBehaviour
    {

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



        void Start()
        {
            SetupSettings();
        }


        #region SettingsPublicMethods

        public void SaveSettings()
        {
            MainGameController.Instance.SettingsController.SaveSettings();
            //SoundManager.Instance.PlayOneShoot(SoundManager.Instance.UISource, SoundManager.Instance.UICollection.clips[0], 1f);

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
}

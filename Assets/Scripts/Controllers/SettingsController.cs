using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using System;
using Newtonsoft.Json.Linq;
using Histhack.Core.SaveLoadSystem;

namespace Histhack.Core.Settings
{
    public class SettingsController
    {
        private List<string> possibleResolutions = new List<string>();


        private float masterVolumeValue;
        private float musicVolumeValue;
        private float effectsVolumeValue;

        private string screenResolution;

        private bool fullscreenActive;

        public float MasterVolumeValue { get => masterVolumeValue; set => masterVolumeValue = value; }
        public float MusicVolumeValue { get => musicVolumeValue; set => musicVolumeValue = value; }
        public float EffectsVolumeValue { get => effectsVolumeValue; set => effectsVolumeValue = value; }

        public string ScreenResolution { get => screenResolution; set => screenResolution = value; }

        public bool FullscreenActive { get => fullscreenActive; set => fullscreenActive = value; }


        private AudioMixer mainMixer;

        string masterExposedValue = "MasterVolume";
        string musicExposedValue = "MusicVolume";
        string effectsExposedValue = "EffectsVolume";

        string savingPath = "GameSettings";

        public void Init(AudioMixer mainMixer)
        {
            this.mainMixer = mainMixer;
            SetupSettings();
        }

        public void LoadSettings()
        {
            if(SaveSystem.CheckIfFileExists(savingPath, SaveDirectories.Core))
            {
                SettingsData data = SaveSystem.LoadClass<SettingsData>(savingPath, SaveDirectories.Core);

                this.masterVolumeValue = data.masterVolumeValue;
                this.musicVolumeValue = data.musicVolumeValue;
                this.effectsVolumeValue = data.effectsVolumeValue;

                this.fullscreenActive = data.fullscreenActive;

                this.screenResolution = data.screenResolution;

                SetValues();
            }
        }

        public void SaveSettings()
        {
            SettingsData data = new SettingsData(masterVolumeValue, musicVolumeValue, effectsVolumeValue, screenResolution, fullscreenActive);

            SaveSystem.SaveClass<SettingsData>(data,savingPath, SaveDirectories.Core);
        }

        private void SetupSettings()
        {
            masterVolumeValue = 0;
            musicVolumeValue = 0;
            effectsVolumeValue = 0;
            
            fullscreenActive = true;

            screenResolution = SubstractResolution(Screen.currentResolution);

            SetValues();
        }

        private void SetValues()
        {
            ChangeVolumeOfSounds(SoundTypes.Master, masterVolumeValue);
            ChangeVolumeOfSounds(SoundTypes.Music, musicVolumeValue);
            ChangeVolumeOfSounds(SoundTypes.Effects, effectsVolumeValue);

            SetFullScreen(fullscreenActive);

            ChangeGameResolution(screenResolution);
        }

        public void SetupResolutions(TMP_Dropdown dropdown)
        {
            Resolution[] tempResolutions = Screen.resolutions;

            possibleResolutions.Clear();
            dropdown.ClearOptions();

            foreach(Resolution oneResolution in tempResolutions)
            {
                string newResolutionInText = SubstractResolution(oneResolution);
                if (!possibleResolutions.Contains(newResolutionInText))
                    possibleResolutions.Add(newResolutionInText);
            }

            dropdown.AddOptions(possibleResolutions);
        }

        public void ChangeGameResolution(string newResolution)
        {
            screenResolution = newResolution;

            Resolution newResolutionValue;
            if (GetResolutionFromString(newResolution, out newResolutionValue))
            {
                Screen.SetResolution(newResolutionValue.width, newResolutionValue.height, true);
            }
            else
            {
                Debug.LogError("Resolution selected does not exists");
            }
        }

        public void SetFullScreen(bool value)
        {
            fullscreenActive = value;
            Screen.fullScreen = value;
        }

        private string SubstractResolution(Resolution rawResolution)
        {
            string resolutionAsText = rawResolution.ToString();

            int indexToRemoveFrom = (resolutionAsText.IndexOf("@") - 1);
            string newResolution = resolutionAsText.Remove(indexToRemoveFrom);

            return newResolution;
        }

        private bool GetResolutionFromString(string resolution, out Resolution correctResolution)
        {
            Resolution[] tempResolutions = Screen.resolutions;

            foreach (Resolution oneResolution in tempResolutions)
            {
                if (resolution == SubstractResolution(oneResolution))
                {
                    correctResolution = oneResolution;
                    return true;
                }
            }

            correctResolution = tempResolutions[0];
            return false;
        }

        public void ChangeVolumeOfSounds(SoundTypes soundType, float newValue)
        {
            switch(soundType)
            {
                case SoundTypes.Master:
                    masterVolumeValue = newValue;
                    ChanegeVolume(masterExposedValue, newValue);
                    break;
                case SoundTypes.Music:
                    musicVolumeValue = newValue;
                    ChanegeVolume(musicExposedValue, newValue);
                    break;
                case SoundTypes.Effects:
                    effectsVolumeValue = newValue;
                    ChanegeVolume(effectsExposedValue, newValue);
                    break;
            }
        }

        private void ChanegeVolume(string exposedValue, float newValue)
        {
            mainMixer.SetFloat(exposedValue, newValue);
        }
    }

    public enum SoundTypes
    {
        Master,
        Music,
        Effects
    }
}


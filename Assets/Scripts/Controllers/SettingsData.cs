using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Histhack.Core.Settings
{
    public class SettingsData
    {
        public float masterVolumeValue;
        public float musicVolumeValue;
        public float effectsVolumeValue;
        public string screenResolution;
        public bool fullscreenActive;

        public SettingsData() { }

        public SettingsData(float masterVolumeValue, float musicVolumeValue, float effectsVolumeValue, string screenResolution, bool fullscreenActive)
        {
            this.masterVolumeValue = masterVolumeValue;
            this.musicVolumeValue = musicVolumeValue;
            this.effectsVolumeValue = effectsVolumeValue;
            this.screenResolution = screenResolution;
            this.fullscreenActive = fullscreenActive;
        }
    }
}

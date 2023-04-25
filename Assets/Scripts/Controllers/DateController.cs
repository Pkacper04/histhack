using Histhack.Core;
using Histhack.Core.SaveLoadSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Histhack.Core
{
    public class DateController : MonoBehaviour
    {
        [SerializeField]
        private int startingYear;

        private int currentYear;

        private string savePath = "Date";

        public int CurrentYear { get => currentYear; }

        private void Awake()
        {
            currentYear = startingYear;
            MainGameController.Instance.GameEvents.OnSaveGame += SaveDate;
            MainGameController.Instance.GameEvents.OnLoadGame += LoadDate;
        }

        private void OnDisable()
        {
            MainGameController.Instance.GameEvents.OnSaveGame -= SaveDate;
            MainGameController.Instance.GameEvents.OnLoadGame -= LoadDate;
        }

        public void UpdateDate(int newYear)
        {
            currentYear = newYear;
            MainGameController.Instance.GameEvents.CallOnDateChanged(newYear);
        }

        private void SaveDate()
        {
            SaveSystem.Save<int>(currentYear, savePath, SaveDirectories.Level);
        }

        private void LoadDate()
        {
            if (SaveSystem.CheckIfFileExists(savePath, SaveDirectories.Level))
            {
                UpdateDate(SaveSystem.Load<int>(savePath, 1918, SaveDirectories.Level));
            }
        }
    }
}

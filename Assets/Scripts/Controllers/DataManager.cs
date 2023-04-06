using Histhack.Core;
using System;
using UnityEngine;

namespace Histhack.Core.SaveLoadSystem
{
    public class DataManager
    {

        public Encrypter Encrypter { get; private set; }

        #region Initialization

        public void Init()
        {
            Encrypter = new Encrypter();
            Encrypter.Initialize();
            LoadGame();
        }

        #endregion Initialization


        #region Saving & Loading

        public void SaveGame()
        {
            MainGameController.Instance.GameEvents.CallOnSaveGame();
        }
        
        public void LoadGame()
        {
            MainGameController.Instance.GameEvents.CallOnPreLoad();
            MainGameController.Instance.GameEvents.CallOnLoadGame();
        }

        #endregion Saving & Loading

    }

}

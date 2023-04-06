using Histhack.Core.Events;
using Histhack.Core.SaveLoadSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Histhack.Core
{
    public class MainGameController : Singleton<MainGameController>
    {

        #region SerializedVariables

        [SerializeField]
        private AddictionalMethods addictionalMethods;

        #endregion SerializedVariables

        #region PrivateVariables

        private GameEvents gameEvents;
        private PlayerEvents playerEvents;

        private DataManager dataManager;

        #endregion PrivateVariables


        #region PublicProperties

        public AddictionalMethods AddictionalMethods { get => addictionalMethods; }

        public GameEvents GameEvents { get => gameEvents; }
        public PlayerEvents PlayerEvents { get => playerEvents; }

        public DataManager DataManager { get => dataManager; }
        #endregion PublicProperties




        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                DontDestroyOnLoad(this);
            }

            InitializeControllers();
        }

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += LoadScene;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= LoadScene;
        }

        private void LoadScene(Scene arg0, Scene arg1)
        {
            dataManager.LoadGame();
        }

        private void InitializeControllers()
        {
            gameEvents = new GameEvents();
            playerEvents = new PlayerEvents();

            dataManager = new DataManager();
            dataManager.Init();
        }
    }
}

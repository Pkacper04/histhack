using Histhack.Core.Effects;
using Histhack.Core.Events;
using Histhack.Core.SaveLoadSystem;
using Histhack.Core.Settings;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Histhack.Core
{
    public class MainGameController : Singleton<MainGameController>
    {

        #region SerializedVariables

        [SerializeField]
        private AddictionalMethods addictionalMethods;

        [SerializeField]
        private AudioMixer mainAudioMixer;

        [SerializeField]
        private PostprocessManager postprocessManager;


        #endregion SerializedVariables

        #region PrivateVariables

        private GameEvents gameEvents;
        private PlayerEvents playerEvents;

        private DataManager dataManager;

        private SettingsController settingsController;

        private string nextSceneToLoad;

        #endregion PrivateVariables


        #region PublicProperties

        public AddictionalMethods AddictionalMethods { get => addictionalMethods; }

        public GameEvents GameEvents { get => gameEvents; }
        public PlayerEvents PlayerEvents { get => playerEvents; }

        public DataManager DataManager { get => dataManager; }

        public SettingsController SettingsController { get => settingsController; }

        public PostprocessManager PostprocessManager { get => postprocessManager; }

        public string NextSceneToLoad { get => nextSceneToLoad; set => nextSceneToLoad = value; }

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

            InitializePostProcesses();
            InitializeControllers();
        }

        private void InitializePostProcesses()
        {
            postprocessManager.ChangePostProcess(PostProcessesToChange.DepthOfField, false);
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

            settingsController = new SettingsController();
            settingsController.Init(mainAudioMixer);
            settingsController.LoadSettings();


        }
    }

    public enum AnimationTypes
    {
        AnchoreMovement,
        ImageFade,
        CanvasFade,
    }

    public enum MovementTypes
    {
        InMovemnt,
        OutMovemnt,
    }
}

using DG.Tweening;
using Histhack.Core.Effects;
using Histhack.Core.Events;
using Histhack.Core.SaveLoadSystem;
using Histhack.Core.Settings;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
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

        [SerializeField]
        private DateController dateController;

        [SerializeField]
        private AnimatedUI transitionAnimation;

        [SerializeField, Scene]
        private string mainGameScene;

        [SerializeField, Scene]
        private string mainMenuScene;

        [SerializeField, Scene]
        private string minigameScene;

        [SerializeField, Scene]
        private string endScene;

        [SerializeField]
        private DialoguesController dialogueController;


        #endregion SerializedVariables

        #region PrivateVariables

        private GameEvents gameEvents;
        private PlayerEvents playerEvents;

        private DataManager dataManager;

        private SettingsController settingsController;

        private string nextSceneToLoad;

        private bool waitForInputAfterLoad = true;

        private MinigameData minigameData = new MinigameData();

        private MinigamesTypes minigameType;

        private bool lastMinigameSucceded;

        private bool minigameStarted = false;

        private int minigameIndex = 0;

        private List<int> finishedMinigames = new List<int>();

        private bool blockMainGame = false;

        private string savePath = "MinigameFinished";

        private PlayerMovementData playerMovementData;

        private int currentTimeFrame;

        private int currentYear;

        #endregion PrivateVariables


        #region PublicProperties

        public AddictionalMethods AddictionalMethods { get => addictionalMethods; }

        public GameEvents GameEvents { get => gameEvents; }
        public PlayerEvents PlayerEvents { get => playerEvents; }

        public DataManager DataManager { get => dataManager; }

        public SettingsController SettingsController { get => settingsController; }

        public PostprocessManager PostprocessManager { get => postprocessManager; }

        public string NextSceneToLoad { get => nextSceneToLoad; set => nextSceneToLoad = value; }

        public bool WaitForInputAfterLoad { get => waitForInputAfterLoad; set => waitForInputAfterLoad = value; }

        public DateController DateController { get => dateController; }

        public MinigameData MinigameData { get => minigameData; }

        public MinigamesTypes MinigameType { get => minigameType; set => minigameType = value; }

        public bool LastMinigameSucceded { get => lastMinigameSucceded; set => lastMinigameSucceded = value; }

        public bool MinigameStarted { get => minigameStarted; set => minigameStarted = value; }

        public int MinigameIndex { get => minigameIndex; set => minigameIndex = value; }

        public List<int> FinishedMinigames { get => finishedMinigames; set => finishedMinigames = value; }


        public bool BlockMainGame { get => blockMainGame; set => blockMainGame = value; }

        public PlayerMovementData PlayerMovementData {
            get
            {
                return playerMovementData;
            }
            set
            {
                playerMovementData = value;
            }
        }


        public int CurrentTimeFrame { get => currentTimeFrame; set => currentTimeFrame = value; }

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
            GameEvents.OnSaveGame += SaveMinigamesState;
            GameEvents.OnLoadGame += LoadMinigamesState;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= LoadScene;
            GameEvents.OnSaveGame -= SaveMinigamesState;
            GameEvents.OnLoadGame -= LoadMinigamesState;
        }

        private void Start()
        {
            dialogueController.ChangeCurrentDialogue();
        }

        private void LoadScene(Scene arg0, Scene arg1)
        {
            dataManager.LoadGame();

            if(arg1.name == mainGameScene)
            {
                dialogueController.Init();
                EndTransition(AnimationTypes.AnchoreMovement,null);
                if (!SaveSystem.CheckIfSaveExists() && !dialogueController.FirstDialogueStarted)
                {
                    dialogueController.CurrentDialogue = 0;
                    dialogueController.StartDialogue();
                    dialogueController.ChangeCurrentDialogue();
                }
            }
            else if(arg1.name == mainMenuScene)
            {
                EndTransition(AnimationTypes.AnchoreMovement, null);
            }
            else if(arg1.name == endScene)
            {
                EndTransition(AnimationTypes.AnchoreMovement, null);
            }

            if (arg1.name != mainGameScene && arg1.name != minigameScene)
            {
                FinishedMinigames.Clear();
                currentYear = 0;
                playerMovementData = null;
                currentTimeFrame = 0;
                dialogueController.CurrentDialogue = 0;
                dialogueController.FirstDialogueStarted = false;
            }


            if (arg1.name == mainGameScene)
            {
                if (minigameStarted && lastMinigameSucceded)
                {
                    if (!finishedMinigames.Contains(minigameIndex))
                    {
                        finishedMinigames.Add(minigameIndex);
                        GameEvents.CallOnMinigameFinished(minigameIndex);
                        dialogueController.ChangeCurrentDialogue();

                        if (dialogueController.CurrentDialogue == dialogueController.VIDESProperty.Count)
                        {
                            GameEvents.CallOnGameFinish();
                        }
                    }
                }
                else if (minigameStarted && !lastMinigameSucceded)
                {
                    GameEvents.CallOnMinigameFailure(minigameIndex);
                }

                if (minigameStarted)
                {
                    minigameStarted = false;
                    DateController.UpdateDate(currentYear);
                    GameEvents.CallOnMinigameReturn();
                    GameEvents.CallOnSaveGame();
                }
            }

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

        public void StartTransition(AnimationTypes animationType, TweenCallback tweenCallback)
        {
            if(animationType == AnimationTypes.AnchoreMovement)
            {
                if (tweenCallback != null)
                    transitionAnimation.SetActionToStartAfterAnimationEnd(tweenCallback);

                transitionAnimation.StartRectMovementAnimation(new Vector2(1920, 0), new Vector2(0, 0), 0);
            }
            else if(animationType == AnimationTypes.CanvasFade)
            {
                if (tweenCallback != null)
                    transitionAnimation.SetActionToStartAfterAnimationEnd(tweenCallback);

                transitionAnimation.CanvasGroupFadeData.CanvasToFade.alpha = 0;
                transitionAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition = new Vector2(0, 0);
                transitionAnimation.StartCanvasGroupFadeAnimation(0,1);
            }
            else if (animationType == AnimationTypes.ImageFade)
            {
                if (tweenCallback != null)
                    transitionAnimation.SetActionToStartAfterAnimationEnd(tweenCallback);

                transitionAnimation.ImageFadeAnimationData.ImageToFade.color = new Color(transitionAnimation.ImageFadeAnimationData.ImageToFade.color.r, transitionAnimation.ImageFadeAnimationData.ImageToFade.color.g, transitionAnimation.ImageFadeAnimationData.ImageToFade.color.b, 0);
                transitionAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition = new Vector2(0, 0);
                transitionAnimation.StartImageFadeAnimation(0, 1);
            }
        }

        public void EndTransition(AnimationTypes animationType, TweenCallback tweenCallback)
        {
            if (animationType == AnimationTypes.AnchoreMovement)
            {
                if (tweenCallback != null)
                    transitionAnimation.SetActionToStartAfterAnimationEnd(tweenCallback);

                transitionAnimation.StartRectMovementAnimation(new Vector2(0, 0), new Vector2(-1920, 0), 1);
            }
            else if (animationType == AnimationTypes.CanvasFade)
            {
                if (tweenCallback != null)
                    transitionAnimation.SetActionToStartAfterAnimationEnd(tweenCallback);

                transitionAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition = new Vector2(0, 0);
                transitionAnimation.CanvasGroupFadeData.CanvasToFade.alpha = 1;
                transitionAnimation.StartCanvasGroupFadeAnimation(1, 0);
            }
            else if (animationType == AnimationTypes.ImageFade)
            {
                if (tweenCallback != null)
                    transitionAnimation.SetActionToStartAfterAnimationEnd(tweenCallback);

                transitionAnimation.ImageFadeAnimationData.ImageToFade.color = new Color(transitionAnimation.ImageFadeAnimationData.ImageToFade.color.r, transitionAnimation.ImageFadeAnimationData.ImageToFade.color.g, transitionAnimation.ImageFadeAnimationData.ImageToFade.color.b, 1);
                transitionAnimation.RectMovementAnimationData[0].ObjectTransform.anchoredPosition = new Vector2(0, 0);
                transitionAnimation.StartImageFadeAnimation(1, 0);
            }
        }

        public void StartMinigame()
        {
            currentYear = DateController.CurrentYear;
            GameEvents.CallOnMinigameStart();
            dialogueController.StartDialogue();
        }


        public void SaveMinigamesState()
        {
            dialogueController.SaveCurrentDialogue();
            SaveSystem.Save<List<int>>(finishedMinigames, savePath, SaveDirectories.Level);
        }

        public void LoadMinigamesState()
        {
            if (SaveSystem.CheckIfFileExists(savePath, SaveDirectories.Level))
            {
                dialogueController.LoadCurrentDialogue();
                finishedMinigames = SaveSystem.Load<List<int>>(savePath, null, SaveDirectories.Level);

                if (dialogueController.CurrentDialogue == dialogueController.VIDESProperty.Count)
                {
                    GameEvents.CallOnGameFinish();
                }
            }
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

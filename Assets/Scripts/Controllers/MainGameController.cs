using DG.Tweening;
using Histhack.Core.Effects;
using Histhack.Core.Events;
using Histhack.Core.SaveLoadSystem;
using Histhack.Core.Settings;
using NaughtyAttributes;
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

        [SerializeField]
        private DateController dateController;

        [SerializeField]
        private AnimatedUI transitionAnimation;

        [SerializeField, Scene]
        private string mainGameScene;

        [SerializeField, Scene]
        private string mainMenuScene;


        #endregion SerializedVariables

        #region PrivateVariables

        private GameEvents gameEvents;
        private PlayerEvents playerEvents;

        private DataManager dataManager;

        private SettingsController settingsController;

        private string nextSceneToLoad;

        private bool waitForInputAfterLoad = true;

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

            if(arg1.name == mainGameScene)
            {
                EndTransition(AnimationTypes.AnchoreMovement,null);
            }
            else if(arg1.name == mainMenuScene)
            {
                EndTransition(AnimationTypes.AnchoreMovement, null);
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

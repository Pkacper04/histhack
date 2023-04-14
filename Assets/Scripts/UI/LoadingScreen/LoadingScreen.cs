using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Histhack.Core;
using Managers.Sounds;

public class LoadingScreen : MonoBehaviour
{

    [SerializeField, BoxGroup("Button Values")]
    private CanvasGroup buttonCanvasGroup;


    [SerializeField, BoxGroup("Loading Elements")]
    private CanvasGroup AnimationLogo;

    [SerializeField, BoxGroup("Loading Elements")]
    private SimpleHalfFadingEffect textFadingEffect;


    [SerializeField, BoxGroup("Animation Values")]
    private float animationTime;

    [SerializeField, BoxGroup("Animation Values")]
    private float startDelay;


    private AsyncOperation operation;

    private bool readyToChangeScene = false;


    #region Initialization

    void Start()
    {
        MainGameController.Instance.EndTransition(AnimationTypes.AnchoreMovement, () => StartCoroutine(StartLoadingGame()));
        SetupLoadingScreen();
    }

    private void SetupLoadingScreen()
    {
        buttonCanvasGroup.alpha = 0;
    }

    #endregion Initialization

    private void Update()
    {
        if (readyToChangeScene == true && Input.anyKeyDown)
        {
            SoundManager.Instance.StopAudio(SoundManager.Instance.EnviromentSource);
            StartMainGame();
        }
    }


    #region Loading

    private IEnumerator StartLoadingGame()
    {
        SoundManager.Instance.PlayClip(SoundManager.Instance.EnviromentSource, SoundManager.Instance.EnviromentCollection.clips[0], true);

        operation = SceneManager.LoadSceneAsync(MainGameController.Instance.NextSceneToLoad);
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(startDelay);
        
        MainGameController.Instance.AddictionalMethods.FadeElement(animationTime, AnimationLogo, 0f, (() => ActivateNextButton()));
    }

    private void ActivateNextButton()
    {
        if (MainGameController.Instance.WaitForInputAfterLoad)
        {
            MainGameController.Instance.AddictionalMethods.FadeElement(animationTime, buttonCanvasGroup, 1f, (() => ActivateTextAndChangePanel()));
        }
        else
        {
            SoundManager.Instance.StopAudio(SoundManager.Instance.EnviromentSource);
            StartMainGame();
        }
        MainGameController.Instance.WaitForInputAfterLoad = true;
    }

    #endregion Loading

    #region LoadingButtons

    private void ActivateTextAndChangePanel()
    {
        textFadingEffect.StartAnimation();
        readyToChangeScene = true;
    }

    private void StartMainGame()
    {
        MainGameController.Instance.StartTransition(AnimationTypes.AnchoreMovement, () => operation.allowSceneActivation = true);
    }

    #endregion LoadingButtons

}

using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Histhack.Core;
using UnityEditor.Build;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField, Scene]
    private string sceneToLoad;


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
        SetupLoadingScreen();
        StartCoroutine(StartLoadingGame());
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
            StartMainGame();
        }
    }


    #region Loading

    private IEnumerator StartLoadingGame()
    {
        operation = SceneManager.LoadSceneAsync(sceneToLoad);

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
        MainGameController.Instance.AddictionalMethods.FadeElement(animationTime, buttonCanvasGroup, 1f, (() => ActivateTextAndChangePanel()));
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
        operation.allowSceneActivation = true;
    }

    #endregion LoadingButtons

}

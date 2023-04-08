using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Histhack.Core;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField, Scene]
    private string sceneToLoad;


    [SerializeField, BoxGroup("Button Values")]
    private Button startGameButton;

    [SerializeField, BoxGroup("Button Values")]
    private CanvasGroup buttonCanvasGroup;

    [SerializeField, BoxGroup("Loading Elements")]
    private CanvasGroup AnimationLogo;

    [SerializeField, BoxGroup("Animation Values")]
    private float animationTime;

    [SerializeField, BoxGroup("Animation Values")]
    private float startDelay;


    private AsyncOperation operation;

    #region Initialization

    void Start()
    {
        SetupLoadingScreen();
        StartCoroutine(StartLoadingGame());
    }

    private void SetupLoadingScreen()
    {
        startGameButton.interactable = false;

        buttonCanvasGroup.alpha = 0;
    }

    #endregion Initialization

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
        MainGameController.Instance.AddictionalMethods.FadeElement(animationTime, buttonCanvasGroup, 1f, (() => startGameButton.interactable = true));
    }

    #endregion Loading

    #region LoadingButtons

    public void StartMainGame()
    {
        operation.allowSceneActivation = true;
    }

    #endregion LoadingButtons

}

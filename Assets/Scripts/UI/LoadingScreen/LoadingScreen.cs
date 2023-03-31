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

    [SerializeField, BoxGroup("Progress Bar")]
    private Image progressBar;

    [SerializeField, BoxGroup("Progress Bar")]
    private CanvasGroup LoadingBarCanvasGroup;

    [SerializeField, BoxGroup("Animation Values")]
    private float animationTime;


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

        progressBar.fillAmount = 0;
    }

    #endregion Initialization

    #region Loading

    private IEnumerator StartLoadingGame()
    {
        operation = SceneManager.LoadSceneAsync(sceneToLoad);

        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            progressBar.fillAmount = Mathf.Clamp01(operation.progress / 0.9f);
            yield return new WaitForSeconds(0.1f);
        }

        progressBar.fillAmount = 1;

        MainGameController.Instance.AddictionalMethods.FadeElement(animationTime, LoadingBarCanvasGroup, 0f, (() => ActivateNextButton()));
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

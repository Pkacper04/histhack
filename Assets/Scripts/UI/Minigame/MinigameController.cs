using Histhack.Core;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameController : MonoBehaviour
{
    [SerializeField, Scene]
    private string sceneToLoadAfter;

    [SerializeField]
    private InfoDisplayerBlock infoDisplayer;

    [SerializeField]
    private MapMinigame mapMinigame;

    [SerializeField]
    private ThreeQuestionController quizController;

    [SerializeField]
    private TextToImageMinigame textToImageMinigame;

    [SerializeField]
    private TextToTextMinigame textToTextMinigame;

    // Start is called before the first frame update
    void Start()
    {
        MainGameController.Instance.EndTransition(AnimationTypes.AnchoreMovement, null);

        InitMinigames();
        ChooseMinigame();
    }

    private void InitMinigames()
    {
        mapMinigame.Init(this);
        quizController.Init(this);
        textToImageMinigame.Init(this);
        textToTextMinigame.Init(this);
    }

    private void ChooseMinigame()
    {
        switch (MainGameController.Instance.MinigameType) {

            case MinigamesTypes.PuzzleMinigame:
                mapMinigame.SetupQuestions(MainGameController.Instance.MinigameData.MapIndex);
                break;
            case MinigamesTypes.QuizMinigame:
                quizController.SetupQuestions(MainGameController.Instance.MinigameData.Question);
                break;
            case MinigamesTypes.TextToImage:
                textToImageMinigame.SetupQuestions(MainGameController.Instance.MinigameData.PicturesQuizzes);
                break;
            case MinigamesTypes.TextToText:
                textToTextMinigame.SetupQuestions(MainGameController.Instance.MinigameData.Pseudonyms);
                break;
        }
    }

    public void OnMinigameFinish(bool status)
    {
        MainGameController.Instance.LastMinigameSucceded = status;

        SlideInfoPanelIn(status);
    }

    public void SlideInfoPanelIn(bool playerWon)
    {
        if(playerWon)
            infoDisplayer.InitInfoDisplayer("Gratulacje! Uda³o ci siê naprawiæ ten okres czasowy. Mo¿esz podró¿owaæ dalej.","Dalej",() => ReturnToGame());
        else
            infoDisplayer.InitInfoDisplayer("Niestety nie uda³o siê naprawiæ tego okresu czasowego. Chcesz spróbowaæ jeszcze raz ?", "Tak","Nie", () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex), () => ReturnToGame());

        infoDisplayer.AnimatedUI.StartRectMovementAnimation(new Vector2(1920, 0), new Vector2(0, 0), 0);
        infoDisplayer.ShowPanel();
    }

    private void ReturnToGame()
    {
        MainGameController.Instance.StartTransition(AnimationTypes.AnchoreMovement, () => SceneManager.LoadScene(sceneToLoadAfter));
    }
}

public enum MinigamesTypes
{
    PuzzleMinigame,
    QuizMinigame,
    TextToImage,
    TextToText
}

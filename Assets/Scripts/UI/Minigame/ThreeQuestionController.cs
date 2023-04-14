using Histhack.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThreeQuestionController : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup minigameCanvas;

    [SerializeField]
    private TMP_Text mainQuestion;

    [SerializeField]
    private Toggle firstCheckbox;

    [SerializeField]
    private Toggle secondCheckbox;

    [SerializeField]
    private Toggle thirdCheckbox;

    [SerializeField]
    private Toggle forthCheckbox;

    [SerializeField]
    private ScreenShake screenShake;

    private Text firstCheckboxText;
    private Text secondCheckboxText;
    private Text thirdCheckboxText;
    private Text forthCheckboxText;

    private string answer;

    private MinigameController minigameController;

    private void Awake()
    {
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(minigameCanvas);
    }

    public void Init(MinigameController minigameController)
    {
        this.minigameController = minigameController;
    }
    private void Start()
    {
        firstCheckboxText = firstCheckbox.GetComponentInChildren<Text>();
        secondCheckboxText = secondCheckbox.GetComponentInChildren<Text>();
        thirdCheckboxText = thirdCheckbox.GetComponentInChildren<Text>();
        forthCheckboxText = forthCheckbox.GetComponentInChildren<Text>();
    }

    public void SetupQuestions(QuizQuestion question)
    {
        mainQuestion.text = question.question;

        List<string> questions = new List<string>() { question.option1, question.option2, question.option3, question.answer };


        int questionIndex = Random.Range(0, questions.Count);

        firstCheckboxText.text = questions[questionIndex];

        questions.RemoveAt(questionIndex);


        questionIndex = Random.Range(0, questions.Count);

        secondCheckboxText.text = questions[questionIndex];

        questions.RemoveAt(questionIndex);


        questionIndex = Random.Range(0, questions.Count);

        thirdCheckboxText.text = questions[questionIndex];

        questions.RemoveAt(questionIndex);

        forthCheckboxText.text = questions[0];

        answer = question.answer;

        MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(minigameCanvas);
    }

    public void OnQuizFinish()
    {
        if(CheckQuestion())
        {
            minigameController.OnMinigameFinish(true);
        }
        else
        {
            screenShake.StartShake();
            minigameController.OnMinigameFinish(false);
        }
    }

    public bool CheckQuestion()
    {
        if(firstCheckbox.isOn)
        {
            if (firstCheckboxText.text == answer)
                return true;
        }
        else if (secondCheckbox.isOn)
        {
            if (secondCheckboxText.text == answer)
                return true;
        }
        else if(thirdCheckbox.isOn)
        {
            if (thirdCheckboxText.text == answer)
                return true;
        }
        else
        {
            if (forthCheckboxText.text == answer)
                return true;
        }

        return false;
    }

}

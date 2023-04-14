using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameData
{
    [SerializeField]
    private QuizQuestion question;

    [SerializeField]
    private List<PicturesQuiz> pictureQuiz;

    [SerializeField]
    private List<Pseudonyms> pseudonyms;

    [SerializeField]
    private int mapIndex;

    public QuizQuestion Question { get => question; set => question = value; }
    public List<PicturesQuiz> PicturesQuizzes { get => pictureQuiz; set => pictureQuiz = value; }
    public List<Pseudonyms> Pseudonyms { get => pseudonyms; set => pseudonyms = value; }
    public int MapIndex { get => mapIndex; set => mapIndex = value; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AllMinigames", order = 1)]
public class AllMinigames : ScriptableObject
{
    public List<PicturesQuiz> pictures;
    public List<Pseudonyms> pseudonyms;
    public List<QuizQuestion> quiz;
}

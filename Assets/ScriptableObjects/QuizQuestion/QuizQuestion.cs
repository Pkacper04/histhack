using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuizQuestion", order = 4)]
public class QuizQuestion : ScriptableObject
{
    public string question;
    public string option1;
    public string option2;
    public string option3;
    public string answer;
}

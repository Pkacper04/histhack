using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PicturesQuiz", order = 2)]
public class PicturesQuiz : ScriptableObject
{
    public Sprite picture;
    public string asnwer;
}

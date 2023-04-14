using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AllMinigames", order = 1)]
public class AllMinigames : ScriptableObject
{
    public MinigamesTypes minigameType;


    [ShowIf(nameof(minigameType),MinigamesTypes.TextToImage)]
    public List<PicturesQuiz> pictures;

    [ShowIf(nameof(minigameType), MinigamesTypes.TextToText)]
    public List<Pseudonyms> pseudonyms;

    [ShowIf(nameof(minigameType), MinigamesTypes.QuizMinigame)]
    public QuizQuestion quiz;

    [ShowIf(nameof(minigameType), MinigamesTypes.PuzzleMinigame)]
    public int mapIndex;
}

using Histhack.Core;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameIntroStartDialogue : MonoBehaviour
{
    [SerializeField]
    private Template_UIManager manager;
    [SerializeField]
    private VIDE_Assign VIDE;
    [SerializeField, Scene]
    private string mainGameScene;

    void Start()
    {
        //MainGameController.Instance.EndTransition(AnimationTypes.CanvasFade, () => manager.Interact(VIDE));
        MainGameController.Instance.EndTransition(AnimationTypes.AnchoreMovement, () => manager.Interact(VIDE));
    }
    public void GoToNextScene()
    {
        MainGameController.Instance.StartTransition(AnimationTypes.AnchoreMovement, () => SceneManager.LoadScene(mainGameScene));
    }
}

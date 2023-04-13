using Histhack.Core;
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

    
    void Start()
    {
        //MainGameController.Instance.EndTransition(AnimationTypes.CanvasFade, () => manager.Interact(VIDE));
        manager.Interact(VIDE);
    }
    public void GoToNextScene(int sceneIndex)
    {
        MainGameController.Instance.StartTransition(AnimationTypes.CanvasFade, () => SceneManager.LoadScene(sceneIndex));
    }
}

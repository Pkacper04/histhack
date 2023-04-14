using Histhack.Core;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndingDialogue : MonoBehaviour
{
    [SerializeField]
    private Template_UIManager manager;
    [SerializeField]
    private VIDE_Assign VIDE;
    [SerializeField, Scene]
    private string mainMenuScene;

    [SerializeField]
    private GameObject endingImage;

    private bool isEndingVisible = false;
    void Start()
    {
        //MainGameController.Instance.EndTransition(AnimationTypes.CanvasFade, () => manager.Interact(VIDE));
        MainGameController.Instance.EndTransition(AnimationTypes.AnchoreMovement, () => manager.Interact(VIDE));
    }
    public void GoToMainMenu()
    {
        MainGameController.Instance.StartTransition(AnimationTypes.AnchoreMovement, null);
        endingImage.active = true;
        MainGameController.Instance.EndTransition(AnimationTypes.AnchoreMovement, ()=> isEndingVisible = true);
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && isEndingVisible)
            MainGameController.Instance.StartTransition(AnimationTypes.AnchoreMovement, () => SceneManager.LoadScene(mainMenuScene));
    }
}

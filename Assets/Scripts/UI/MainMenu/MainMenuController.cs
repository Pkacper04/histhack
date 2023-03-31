using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField, Scene]
    private string gameSceneToLoad;


    #region MainMenuButtons

    public void ContinueGame()
    {
        //TODO
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneToLoad);
    }

    public void Settings()
    {
        //TODO
    }

    public void Credits()
    {
        //TODO
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    #endregion
}

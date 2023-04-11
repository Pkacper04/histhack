using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using System;
using Histhack.Core;

public class SteelLotusIntroController : MonoBehaviour
{
    [SerializeField]
    VideoPlayer player;

    [SerializeField, Scene]
    private string loadingScene;

    [SerializeField, Scene]
    private string afterLoadingScene;

    private bool delayInActive = false;
    private bool videoFinished = false;

    private void Start()
    {
        StartCoroutine(WaitWithDelay());
    }

    private void Update()
    {
        if(!videoFinished && delayInActive && !player.isPlaying)
        {
            VideoFinished();
        }
    }

    private IEnumerator WaitWithDelay()
    {
        yield return new WaitForSeconds(0.5f);

        delayInActive = true;
    }

    private void VideoFinished()
    {
        videoFinished = true;
        MainGameController.Instance.NextSceneToLoad = afterLoadingScene;
        MainGameController.Instance.WaitForInputAfterLoad = false;
        MainGameController.Instance.StartTransition(AnimationTypes.AnchoreMovement, () => SceneManager.LoadScene(loadingScene));
    }
}

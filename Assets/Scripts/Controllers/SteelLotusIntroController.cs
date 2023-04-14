using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using System;
using Histhack.Core;
using Managers.Sounds;

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
        SoundManager.Instance.PlayOneShoot(SoundManager.Instance.EnviromentSource, SoundManager.Instance.EnviromentCollection.clips[2], 1f);
       
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
        SoundManager.Instance.PlayClip(SoundManager.Instance.EnviromentSource, SoundManager.Instance.EnviromentCollection.clips[0], true);
        
    }
}

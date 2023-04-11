using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SteelLotusIntroController : MonoBehaviour
{
    [SerializeField]
    VideoPlayer player;
    void Update()
    {
        if (!player.isPlaying)
            SceneManager.LoadScene(1);
    }
}

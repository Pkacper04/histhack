using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    private RectTransform transformToShake;

    [SerializeField]
    private float shakeDuration;

    [SerializeField]
    private float shakeStrength;

    [SerializeField]
    private int vibrato;

    public void StartShake()
    {
        transformToShake.DOShakeAnchorPos(shakeDuration, shakeStrength, vibrato);
    }
}

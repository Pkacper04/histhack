using Histhack.Core;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleHalfFadingEffect : MonoBehaviour
{

    [SerializeField]
    private AnimationType animationType;

    [SerializeField, ShowIf(nameof(animationType), AnimationType.CanvasGroup)]
    private CanvasGroup canvasGroupToAnimate;

    [SerializeField, ShowIf(nameof(animationType), AnimationType.Image)]
    private Image imageToAnimate;

    [SerializeField, ShowIf(nameof(animationType), AnimationType.Text)]
    private TMP_Text textToAnimate;

    [SerializeField, Range(0,1)]
    private float startValue;

    [SerializeField, Range(0, 1)]
    private float endValue;

    [SerializeField]
    private float animationTime;


    public void StartAnimation()
    {
        AnimateIn();   
    }

    private void AnimateIn()
    {
        switch(animationType)
        {
            case AnimationType.Image:
                MainGameController.Instance.AddictionalMethods.FadeElement(animationTime, imageToAnimate, startValue, endValue, (() => AnimateOut()));
                break;
            case AnimationType.Text:
                MainGameController.Instance.AddictionalMethods.FadeElement(animationTime, textToAnimate, startValue, endValue, (() => AnimateOut()));
                break;
            case AnimationType.CanvasGroup:
                MainGameController.Instance.AddictionalMethods.FadeElement(animationTime, canvasGroupToAnimate, startValue, endValue, (() => AnimateOut()));
                break;
        }
        
    }

    private void AnimateOut()
    {
        switch (animationType)
        {
            case AnimationType.Image:
                MainGameController.Instance.AddictionalMethods.FadeElement(animationTime, imageToAnimate, endValue, startValue, (() => AnimateIn()));
                break;
            case AnimationType.Text:
                MainGameController.Instance.AddictionalMethods.FadeElement(animationTime, textToAnimate, endValue, startValue,(() => AnimateIn()));
                break;
            case AnimationType.CanvasGroup:
                MainGameController.Instance.AddictionalMethods.FadeElement(animationTime, canvasGroupToAnimate, endValue, startValue,(() => AnimateIn()));
                break;
        }
    }

    private enum AnimationType
    {
        Image,
        CanvasGroup,
        Text
    }
}

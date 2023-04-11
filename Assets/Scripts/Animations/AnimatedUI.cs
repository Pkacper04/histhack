using DG.Tweening;
using Histhack.Core;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedUI : MonoBehaviour
{
    [SerializeField]
    private AnimationTypes animationType;

    [SerializeField, ShowIf(nameof(animationType), AnimationTypes.AnchoreMovement)]
    private List<RectMovementAnimation> rectMovementAnimationData = new List<RectMovementAnimation>();

    [SerializeField, ShowIf(nameof(animationType), AnimationTypes.ImageFade)]
    private ImageFadeAnimation imageFadeAnimationData;

    [SerializeField, ShowIf(nameof(animationType), AnimationTypes.CanvasFade)]
    private CanvasGroupFadeAnimation canvasGroupFadeData;

    [SerializeField]
    private TweenCallback actionToStartAfterEnd = null;

    public event Action<int> OnAnimationStart;
    public event Action<int> OnAnimationEnd;

    public List<RectMovementAnimation> RectMovementAnimationData { get => rectMovementAnimationData; }
    public ImageFadeAnimation ImageFadeAnimationData { get => imageFadeAnimationData; }
    public CanvasGroupFadeAnimation CanvasGroupFadeData { get => canvasGroupFadeData; }

    private void CallOnAnimationStart(int index)
    {
        if (OnAnimationStart != null)
            OnAnimationStart(index);
    }

    private void CallOnAnimationEnd(int index)
    {
        if (OnAnimationEnd != null)
            OnAnimationEnd(index);
    }


    public void SetActionToStartAfterAnimationEnd(TweenCallback tweenCallback)
    {
        actionToStartAfterEnd = tweenCallback;
    }

    public void StartRectMovementAnimation(Vector2 startPosition, Vector2 endPosition, int index = 0)
    {
        CallOnAnimationStart(index);
        DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>  tween = rectMovementAnimationData[index].Animate(startPosition, endPosition, actionToStartAfterEnd);
        actionToStartAfterEnd = null;
        tween.onComplete += () => CallOnAnimationEnd(index);
    }

    public void StartImageFadeAnimation(float startAlpha, float endAlpha)
    {
        imageFadeAnimationData.Animate(startAlpha, endAlpha);
    }

    public void StartCanvasGroupFadeAnimation(float startAlpha, float endAlpha)
    {
        canvasGroupFadeData.Animate(startAlpha, endAlpha);
    }
}


[Serializable]
public class RectMovementAnimation
{
    [SerializeField]
    private RectTransform objectTransform;

    [SerializeField]
    private float animationDuration;

    [SerializeField]
    private Ease animationEase;

    [SerializeField]
    private float setAnimationDelay;

    public RectTransform ObjectTransform { get => objectTransform; set => objectTransform = value; }

    public DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions> Animate(Vector2 StartPostion, Vector2 endPostion, TweenCallback actionOnComplete = null)
    {
        objectTransform.anchoredPosition = StartPostion;
        DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions> tween = objectTransform.DOAnchorPos(endPostion, animationDuration).SetEase(animationEase).SetDelay(setAnimationDelay).OnComplete(actionOnComplete).SetUpdate(true);

        return tween;
    }
}

[Serializable]
public class ImageFadeAnimation
{
    [SerializeField]
    private Image imageToFade;

    [SerializeField]
    private float animationDuration;

    [SerializeField]
    private Ease animationEase;

    [SerializeField]
    private float setAnimationDelay;

    public Image ImageToFade { get => imageToFade; set => imageToFade = value; }

    public void Animate(float startAlpha, float endAlpha)
    {
        imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, startAlpha);

        imageToFade.DOFade(endAlpha, animationDuration).SetEase(animationEase).SetDelay(setAnimationDelay).SetUpdate(true);
    }
}

[Serializable]
public class CanvasGroupFadeAnimation
{
    [SerializeField]
    private CanvasGroup canvasToFade;

    [SerializeField]
    private float animationDuration;

    [SerializeField]
    private Ease animationEase;

    [SerializeField]
    private float setAnimationDelay;

    public CanvasGroup CanvasToFade { get => canvasToFade; set => canvasToFade = value; }

    public void Animate(float startAlpha, float endAlpha)
    {
        canvasToFade.alpha = startAlpha;

        canvasToFade.DOFade(endAlpha, animationDuration).SetEase(animationEase).SetDelay(setAnimationDelay).SetUpdate(true);
    }
}

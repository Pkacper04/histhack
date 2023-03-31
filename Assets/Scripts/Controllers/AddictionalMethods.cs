using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AddictionalMethods : MonoBehaviour
{
    #region Fading

    public void FadeElement(float duration, Image imageToFade, float endAlpha , Action myDelegate = null)
    {
        StartCoroutine(FadeElementCoroutine(duration,imageToFade,endAlpha,myDelegate));
    }

    public void FadeElement(float duration, CanvasGroup canvasToFade, float endAlpha, Action myDelegate = null)
    {
        StartCoroutine(FadeElementCoroutine(duration, canvasToFade, endAlpha, myDelegate));
    }

    private IEnumerator FadeElementCoroutine(float duration, Image imageToFade, float endAlpha , Action myDelegate)
    {
        float timePassed = 0;
        float startedAlpha = imageToFade.color.a;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startedAlpha, endAlpha, timePassed / duration);
            imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, newAlpha);
            yield return null;
        }

        if(myDelegate != null)
            myDelegate.Invoke();
    }

    private IEnumerator FadeElementCoroutine(float duration, CanvasGroup canvasToFade, float endAlpha, Action myDelegate)
    {
        float timePassed = 0;
        float startedAlpha = canvasToFade.alpha;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startedAlpha, endAlpha, timePassed / duration);
            canvasToFade.alpha = newAlpha;
            yield return null;
        }

        if (myDelegate != null)
            myDelegate.Invoke();
    }

    #endregion Fading
}

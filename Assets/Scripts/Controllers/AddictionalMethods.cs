using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddictionalMethods : MonoBehaviour
{
    #region Fading

    public void FadeElement(float duration, Image imageToFade, float endAlpha, Action myDelegate = null, bool ignoreTimeScale = false, float delay = 0)
    {
        StartCoroutine(FadeElementCoroutine(duration,imageToFade,endAlpha,myDelegate, ignoreTimeScale,delay));
    }

    public void FadeElement(float duration, CanvasGroup canvasToFade, float endAlpha, Action myDelegate = null, bool ignoreTimeScale = false, float delay = 0)
    {
        StartCoroutine(FadeElementCoroutine(duration, canvasToFade, endAlpha, myDelegate, ignoreTimeScale, delay));
    }

    public void FadeElement(float duration, TMP_Text textToFade, float endAlpha, Action myDelegate = null, bool ignoreTimeScale = false, float delay = 0)
    {
        StartCoroutine(FadeElementCoroutine(duration, textToFade, endAlpha, myDelegate, ignoreTimeScale, delay));
    }

    private IEnumerator FadeElementCoroutine(float duration, Image imageToFade, float endAlpha , Action myDelegate, bool ignoreTimeScale, float delay)
    {
        if(ignoreTimeScale)
        {
            yield return new WaitForSecondsRealtime(delay);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }

        float timePassed = 0;
        float startedAlpha = imageToFade.color.a;

        while (timePassed < duration)
        {
            if (!ignoreTimeScale)
                timePassed += Time.deltaTime;
            else
                timePassed += Time.unscaledDeltaTime;
            float newAlpha = Mathf.Lerp(startedAlpha, endAlpha, timePassed / duration);
            imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, newAlpha);
            yield return null;
        }

        if(myDelegate != null)
            myDelegate.Invoke();
    }

    private IEnumerator FadeElementCoroutine(float duration, CanvasGroup canvasToFade, float endAlpha, Action myDelegate, bool ignoreTimeScale, float delay)
    {
        if (ignoreTimeScale)
        {
            yield return new WaitForSecondsRealtime(delay);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }

        float timePassed = 0;
        float startedAlpha = canvasToFade.alpha;

        while (timePassed < duration)
        {
            if (!ignoreTimeScale)
                timePassed += Time.deltaTime;
            else
                timePassed += Time.unscaledDeltaTime;
            float newAlpha = Mathf.Lerp(startedAlpha, endAlpha, timePassed / duration);
            canvasToFade.alpha = newAlpha;
            yield return null;
        }

        if (myDelegate != null)
            myDelegate.Invoke();
    }

    private IEnumerator FadeElementCoroutine(float duration, TMP_Text textToFade, float endAlpha, Action myDelegate, bool ignoreTimeScale, float delay)
    {
        if (ignoreTimeScale)
        {
            yield return new WaitForSecondsRealtime(delay);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }

        float timePassed = 0;
        float startedAlpha = textToFade.alpha;

        while (timePassed < duration)
        {
            if (!ignoreTimeScale)
                timePassed += Time.deltaTime;
            else
                timePassed += Time.unscaledDeltaTime;
            float newAlpha = Mathf.Lerp(startedAlpha, endAlpha, timePassed / duration);
            textToFade.alpha = newAlpha;
            yield return null;
        }

        if (myDelegate != null)
            myDelegate.Invoke();
    }

    public void FadeElement(float duration, Image imageToFade, float startAlpha, float endAlpha, Action myDelegate = null)
    {
        StartCoroutine(FadeElementCoroutine(duration, imageToFade, startAlpha, endAlpha, myDelegate));
    }

    public void FadeElement(float duration, CanvasGroup canvasToFade, float startAlpha, float endAlpha, Action myDelegate = null)
    {
        StartCoroutine(FadeElementCoroutine(duration, canvasToFade, startAlpha, endAlpha, myDelegate));
    }

    public void FadeElement(float duration, TMP_Text textToFade, float startAlpha, float endAlpha, Action myDelegate = null)
    {
        StartCoroutine(FadeElementCoroutine(duration, textToFade, startAlpha, endAlpha, myDelegate));
    }

    private IEnumerator FadeElementCoroutine(float duration, Image imageToFade, float startAlpha , float endAlpha, Action myDelegate)
    {
        float timePassed = 0;
        float startedAlpha = startAlpha;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startedAlpha, endAlpha, timePassed / duration);
            imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, newAlpha);
            yield return null;
        }

        if (myDelegate != null)
            myDelegate.Invoke();
    }

    private IEnumerator FadeElementCoroutine(float duration, CanvasGroup canvasToFade, float startAlpha, float endAlpha, Action myDelegate)
    {
        float timePassed = 0;
        float startedAlpha = startAlpha;

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

    private IEnumerator FadeElementCoroutine(float duration, TMP_Text textToFade, float startAlpha, float endAlpha, Action myDelegate)
    {
        float timePassed = 0;
        float startedAlpha = startAlpha;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startedAlpha, endAlpha, timePassed / duration);
            textToFade.alpha = newAlpha;
            yield return null;
        }

        if (myDelegate != null)
            myDelegate.Invoke();
    }

    #endregion Fading

    #region CanvasGroup

    public void ActivateCanvasGroup(CanvasGroup canvasGroup, bool withoutAlpha = false)
    {
        if(!withoutAlpha)
            canvasGroup.alpha = 1;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void DeactivateCanvasGroup(CanvasGroup canvasGroup, bool withoutAlpha = false)
    {
        if(!withoutAlpha)
            canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    #endregion CanvasGroup

    #region TextManagement

    public void BuildText(TMP_Text textEdit, string text, float displayTime, bool onEndIndent = false)
    {
        StartCoroutine(displayText(textEdit, text, displayTime, onEndIndent));
    }

    public void BuildText(Text textEdit, string text, float displayTime, bool onEndIndent = false)
    {
        StartCoroutine(displayText(textEdit, text, displayTime, onEndIndent));
    }

    public void BuildTextInstant(TMP_Text textEdit, string text, bool OnEndIndent = false)
    {
        if (OnEndIndent)
            textEdit.text = text + "\n";
        textEdit.text = text;
    }

    public void BuildTextInstant(Text textEdit, string text, bool OnEndIndent = false)
    {
        if (OnEndIndent)
            textEdit.text = text + "\n";
        textEdit.text = text;
    }


    private IEnumerator displayText(TMP_Text textEdit, string text, float displayTime, bool onEndIndent)
    {
        for (int i = 0; i < text.Length; i++)
        {
            textEdit.text = string.Concat(textEdit.text, text[i]);
            yield return new WaitForSecondsRealtime(displayTime);
        }
        if (onEndIndent)
            textEdit.text += "\n";
    }

    private IEnumerator displayText(Text textEdit, string text, float displayTime, bool onEndIndent)
    {
        for (int i = 0; i < text.Length; i++)
        {
            textEdit.text = string.Concat(textEdit.text, text[i]);
            yield return new WaitForSecondsRealtime(displayTime);
        }
        if (onEndIndent)
            textEdit.text += "\n";
    }

    #endregion TextManagement


}


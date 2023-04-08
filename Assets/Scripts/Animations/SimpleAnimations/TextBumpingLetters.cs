using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class TextBumpingLetters : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text textField;

    [SerializeField]
    private string fullText;

    [SerializeField]
    private string animatedDot;

    [SerializeField, MinValue(0)]
    private int numberOfDots;

    [SerializeField]
    private float waitingTime;

    private int startingTextLength;

    private int currentIndex;

    private void Start()
    {
        startingTextLength = fullText.Length;
        currentIndex = 0;
        StartCoroutine(DotText());
    }

    private IEnumerator DotText()
    {
        if (currentIndex >= numberOfDots)
        {
            fullText = fullText.Remove(startingTextLength);
            textField.text = fullText;
            currentIndex = 0;
        }

        fullText = fullText + animatedDot;
        textField.text = fullText;
        currentIndex++;

        yield return new WaitForSeconds(waitingTime);

        StartCoroutine(DotText());
    }
}

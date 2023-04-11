using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitch : MonoBehaviour
{
    [SerializeField]
    private Image imageToChange;

    [SerializeField]
    private List<Sprite> spritesToChange = new List<Sprite>();

    [SerializeField]
    private float changeTime;

    private int currentIndex = 0;

    private Coroutine animationCoroutine;

    public void StartAnimation()
    {
        animationCoroutine = StartCoroutine(WaitForSpriteChange());
    }

    public void StopAnimation()
    {
        StopCoroutine(animationCoroutine);
    }

    private IEnumerator WaitForSpriteChange()
    {
        while (true)
        {
            imageToChange.sprite = spritesToChange[currentIndex];

            yield return new WaitForSecondsRealtime(changeTime);

            currentIndex++;

            if (currentIndex == spritesToChange.Count)
                currentIndex = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupTimeline : MonoBehaviour
{
    [SerializeField]
    private Sprite corruptedSprite;

    [SerializeField]
    private List<Sprite> normalSprites;

    [SerializeField]
    private List<OneElementData> elementsToAdd = new List<OneElementData>();

    [SerializeField]
    private RectTransform parentTransform;

    [SerializeField]
    private OneTimeframe oneTimeframe;

    private List<OneTimeframe> elementsAddedOnScene = new List<OneTimeframe>();

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        for (int i = 0; i < elementsToAdd.Count; i++)
        {
            OneTimeframe newElement = Instantiate(oneTimeframe, parentTransform);
            elementsAddedOnScene.Add(newElement);

            if (elementsToAdd[i].IsCorrupted)
                newElement.Init(corruptedSprite);
            else
                newElement.Init(normalSprites[Random.Range(0, normalSprites.Count)]);
        }
    }

}

[System.Serializable]
public class OneElementData
{
    [SerializeField]
    private bool isCorrupted;

    public bool IsCorrupted { get => isCorrupted; set => isCorrupted = value; }
}

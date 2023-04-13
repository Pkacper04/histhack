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

    [SerializeField]
    private PlayerMovement playerMovement;

    private List<OneTimeframe> elementsAddedOnScene = new List<OneTimeframe>();

    private int currentTimeFrame = 0;

    private bool blockInteraction = false;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        if (blockInteraction)
            return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ChangeTimeFrame(-1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ChangeTimeFrame(1);
        }
    }

    private void ChangeTimeFrame(int direction)
    {
        if (direction > 0 && currentTimeFrame == 0)
            return;
        else if (direction < 0 && currentTimeFrame == (elementsToAdd.Count - 1))
            return;

        if (elementsAddedOnScene[currentTimeFrame].IsCorrupted && direction < 0)
            return;

        blockInteraction = true;

        Debug.Log("current time frame: "+currentTimeFrame);
        if (currentTimeFrame == 0)
            playerMovement.MovePlayer(direction, () => blockInteraction = false);
        else if (currentTimeFrame == (elementsToAdd.Count - 1))
            playerMovement.MovePlayer(direction, () => blockInteraction = false);
        else if(currentTimeFrame == 1 && direction > 0)
            playerMovement.MovePlayer(direction, () => blockInteraction = false);
        else if(currentTimeFrame == (elementsToAdd.Count - 2) && direction < 0)
            playerMovement.MovePlayer(direction, () => blockInteraction = false);
        else
            playerMovement.MoveBackground(direction, () => blockInteraction = false);

        currentTimeFrame += direction * -1;
    }

    public void Setup()
    {
        for (int i = 0; i < elementsToAdd.Count; i++)
        {
            OneTimeframe newElement = Instantiate(oneTimeframe, parentTransform);
            elementsAddedOnScene.Add(newElement);

            if (elementsToAdd[i].IsCorrupted)
                newElement.Init(corruptedSprite, elementsToAdd[i].IsCorrupted);
            else
                newElement.Init(normalSprites[Random.Range(0, normalSprites.Count)], elementsToAdd[i].IsCorrupted);
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

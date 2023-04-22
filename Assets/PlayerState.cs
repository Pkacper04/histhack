using Histhack.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private Image playerImage;

    [SerializeField]
    private float happySadDuration = 0;

    [SerializeField]
    private PlayerStateImages normalState;

    [SerializeField]
    private PlayerStateImages happyState;

    [SerializeField]
    private PlayerStateImages sadState;

    private PlayerStates playerCurrentState;


    void Awake()
    {
        playerImage.sprite = normalState.ForwardSprite;
        MainGameController.Instance.GameEvents.OnMinigameFinished += CheckState;
        MainGameController.Instance.GameEvents.OnMinigameFailure += CheckState;
    }

    private void OnDisable()
    {
        MainGameController.Instance.GameEvents.OnMinigameFinished -= CheckState;
        MainGameController.Instance.GameEvents.OnMinigameFailure -= CheckState;
    }

    public void RightMovementStart()
    {
        PlayerStateImages currentState = SelectState(playerCurrentState);
        playerImage.sprite = currentState.RightSprite;
    }

    public void LeftMovementStart()
    {
        PlayerStateImages currentState = SelectState(playerCurrentState);
        playerImage.sprite = currentState.LeftSprite;
    }

    public void ReturnToNormalState()
    {
        PlayerStateImages currentState = SelectState(playerCurrentState);
        playerImage.sprite = currentState.ForwardSprite;
    }

    public void ReturnToNormalState(int _)
    {
        PlayerStateImages currentState = SelectState(playerCurrentState);
        playerImage.sprite = currentState.ForwardSprite;
    }

    private void ChangePlayerState(PlayerStates newState)
    {
        playerCurrentState = newState;

        UpdateState();

        if (newState != PlayerStates.normal)
            StartCoroutine(ChangeStateAfterTime());
    }

    private void CheckState(int _)
    {
        if (MainGameController.Instance.LastMinigameSucceded)
            ChangePlayerState(PlayerStates.happy);
        else
            ChangePlayerState(PlayerStates.sad);
    }

    private void UpdateState()
    {
        if (playerMovement.MovingDirection == 0)
            ReturnToNormalState();
        else if (playerMovement.MovingDirection > 0)
            LeftMovementStart();
        else
            RightMovementStart();
    }

    private IEnumerator ChangeStateAfterTime()
    {
        yield return new WaitForSeconds(happySadDuration);
        playerCurrentState = PlayerStates.normal;

        UpdateState();
    }


    private PlayerStateImages SelectState(PlayerStates currentState)
    {
        switch (currentState)
        {
            case PlayerStates.normal:
                return normalState;
            case PlayerStates.happy:
                return happyState;
            case PlayerStates.sad:
                return sadState;
            default:
                return normalState;
        }
    }
}

[Serializable]
public class PlayerStateImages
{
    [SerializeField]
    private Sprite forwardSprite;

    [SerializeField]
    private Sprite leftSprite;

    [SerializeField]
    private Sprite rightSprite;

    public Sprite ForwardSprite { get => forwardSprite; }
    public Sprite LeftSprite { get => leftSprite; }
    public Sprite RightSprite { get => rightSprite; }
}

public enum PlayerStates
{
    normal,
    happy,
    sad
}
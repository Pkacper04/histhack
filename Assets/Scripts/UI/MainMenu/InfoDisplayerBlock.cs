using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Histhack.Core;
using System;

public class InfoDisplayerBlock : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup infoDisplayerCanvas;

    [SerializeField]
    private AnimatedUI animatedUI;

    [SerializeField]
    private TMP_Text infoText;

    [SerializeField]
    private Button confirmButton;

    [SerializeField]
    private Button declineButton;

    [SerializeField]
    private Button neutralButton;


    public AnimatedUI AnimatedUI { get => animatedUI; }

    private TMP_Text confirmButtonText;
    private TMP_Text declineButtonText;
    private TMP_Text neutralButtonText;


    private void Start()
    {
        confirmButtonText = confirmButton.GetComponentInChildren<TMP_Text>();
        declineButtonText = declineButton.GetComponentInChildren<TMP_Text>();
        neutralButtonText = neutralButton.GetComponentInChildren<TMP_Text>();
    }

    private void ChangeYesNoButtons(bool status)
    {
        confirmButton.gameObject.SetActive(status);
        declineButton.gameObject.SetActive(status);
    }

    private void ChangeNeutralButton(bool status)
    {
        neutralButton.gameObject.SetActive(status);
    }

    private void OnEnable()
    {
        animatedUI.OnAnimationEnd += CheckToDisablePanel;
    }

    private void CheckToDisablePanel(int index)
    {
        if(index == 1)
        {
            HidePanel();
        }
    }

    public void InitInfoDisplayer(string infoText, string buttonConfirmText, string buttonDeclineText, UnityAction actionOnConfirm, UnityAction actionOnDecline)
    {
        ChangeNeutralButton(false);
        ChangeYesNoButtons(true);

        confirmButton.onClick.RemoveAllListeners();
        declineButton.onClick.RemoveAllListeners();

        this.infoText.text = infoText;
        this.confirmButtonText.text = buttonConfirmText;
        this.declineButtonText.text = buttonDeclineText;

        confirmButton.onClick.AddListener(actionOnConfirm);
        declineButton.onClick.AddListener(actionOnDecline);
    }

    public void InitInfoDisplayer(string infoText, string buttonNeutralText, UnityAction actionOnNeutralClick)
    {
        ChangeNeutralButton(true);
        ChangeYesNoButtons(false);

        neutralButton.onClick.RemoveAllListeners();

        this.infoText.text = infoText;

        this.neutralButtonText.text = buttonNeutralText;

        neutralButton.onClick.AddListener(actionOnNeutralClick);
    }

    public void HidePanel()
    {
        MainGameController.Instance.AddictionalMethods.DeactivateCanvasGroup(infoDisplayerCanvas);
    }

    public void ShowPanel()
    {
        MainGameController.Instance.AddictionalMethods.ActivateCanvasGroup(infoDisplayerCanvas);
    }
}

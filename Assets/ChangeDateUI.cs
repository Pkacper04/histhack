using Histhack.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeDateUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dateText;


    private void Start()
    {
        dateText.text = MainGameController.Instance.DateController.CurrentYear.ToString();
    }

    private void OnEnable()
    {
        MainGameController.Instance.GameEvents.OnDateChanged += UpdateDate;
    }

    private void OnDisable()
    {
        MainGameController.Instance.GameEvents.OnDateChanged -= UpdateDate;
    }

    private void UpdateDate(int newDate)
    {
        dateText.text = newDate.ToString();
    }
}

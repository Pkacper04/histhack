using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorCounter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text errorTextedit;

    [SerializeField]
    private string prefixText;

    private int maxErrors;

    public int MaxErrors { get => maxErrors; set => maxErrors = value; }

    public void ChangeErrorText(int currentErrorCount)
    {
        errorTextedit.text = prefixText + currentErrorCount + "/" + maxErrors;
    }
}

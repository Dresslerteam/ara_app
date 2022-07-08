using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TaskDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stepNumberText;

    [SerializeField] private TextMeshProUGUI taskTitleText;

    [SerializeField] private TextMeshProUGUI completenessText;
    public Button taskButton;
    public void UpdateDisplayInformation(string number, string title, bool isComplete)
    {
        stepNumberText.text = number;
        taskTitleText.text = title;
        completenessText.text = isComplete ? "Done" : "Open";
    }

    public void ProgressStep()
    {
        MainMenuManager.Instance.AdvanceToJobView();
    }
}
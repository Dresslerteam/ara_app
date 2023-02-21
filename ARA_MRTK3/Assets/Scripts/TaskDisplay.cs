﻿using System;
using System.Collections;
using System.Collections.Generic;
using Ara.Domain.JobManagement;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TaskDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stepNumberText;

    [SerializeField] private TextMeshProUGUI taskTitleText;

    [SerializeField] private TextMeshProUGUI completenessText;
    [SerializeField] private TMP_Dropdown completenessDropdown;
    public PressableButton taskButton;
    public void UpdateDisplayInformation(string number, string title, TaskInfo.TaskStatus status)
    {
        stepNumberText.text = number;
        taskTitleText.text = title;
        //completenessText.text = completenessText.text = status.ToString();
        
        if(completenessDropdown!=null)
            completenessDropdown.value = (int)status;
    }

    public void ProgressStep()
    {
        MainMenuManager.Instance.AdvanceToWorkingView();
    }
}
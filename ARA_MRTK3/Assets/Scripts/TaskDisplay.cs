using System;
using System.Collections;
using System.Collections.Generic;
using Ara.Domain.ApiClients.Dtos;
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
    
    [SerializeField] private TextMeshProUGUI statusText;
    
    public PressableButton taskButton;
    public void UpdateDisplayInformation(string number, string title, TaskInfo.TaskStatus status)
    {
        stepNumberText.text = number;
        taskTitleText.text = title;
        Debug.Log("Status is: " + status);
        if(status==0)
            return;
        statusText.text = status switch
        {
            TaskInfo.TaskStatus.ToDo => "TO DO",
            TaskInfo.TaskStatus.InProgress => "IN PROGRESS",
            TaskInfo.TaskStatus.Completed => "COMPLETED",
            TaskInfo.TaskStatus.OnHold => "ON HOLD"
        };
    }

    public void UpdateTaskStatusViaButton(string status)
    {
        if (MainMenuManager.Instance != null && MainMenuManager.Instance.currentJob != null)
        {
            var jobId = int.Parse(MainMenuManager.Instance.currentJob.Id);
        
            switch (status)
            {
                case "TO DO":
                    MainMenuManager.Instance.currentJob.ChangeTaskStatus(jobId, TaskInfo.TaskStatus.ToDo);
                    break;
                case "IN PROGRESS":
                    MainMenuManager.Instance.currentJob.ChangeTaskStatus(jobId, TaskInfo.TaskStatus.InProgress);
                    break;
                case "COMPLETED":
                    MainMenuManager.Instance.currentJob.ChangeTaskStatus(jobId, TaskInfo.TaskStatus.Completed);
                    break;
                case "ON HOLD":
                    MainMenuManager.Instance.currentJob.ChangeTaskStatus(jobId, TaskInfo.TaskStatus.OnHold);
                    break;
                default:
                    Debug.Log("Error: Invalid status");
                    break;
            }
        }
        else
        {
            Debug.Log("Error: MainMenuManager instance or selectedJobListItem is null");
        }
        statusText.text = status.ToString();
    }

}
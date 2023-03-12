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
    [SerializeField] private TMP_Dropdown completenessDropdown;
    
    [HideInInspector]
    public JobListItemDto job;
    public PressableButton taskButton;
    public void UpdateDisplayInformation(string number, string title, TaskInfo.TaskStatus status, JobListItemDto currentJob)
    {
        stepNumberText.text = number;
        taskTitleText.text = title;
        //completenessText.text = completenessText.text = status.ToString();
        job = currentJob;
        if(completenessDropdown!=null)
            completenessDropdown.value = (int)status;
    }

    public void UpdateTaskStatusViaButton(string status)
    {
        // Status can be To Do, In Progress, Done
        // switch statement for status to update job.taskInfo.status
        
        switch (status)
        {
            case "TO DO":
                job.Status = Job.JobStatus.ToDo;
                break;
            case "IN PROGRESS":
                job.Status = Job.JobStatus.InProgress;
                break;
            case "COMPLETED" :
                job.Status = Job.JobStatus.Completed;
                break;
            default:
                Debug.Log("Error: Invalid status");
                break;
        }
    }

}
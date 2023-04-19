using System;
using System.Collections;
using System.Collections.Generic;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.JobManagement;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class TaskDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stepNumberText;

    [SerializeField] private TextMeshProUGUI taskTitleText;

    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] public PressableButton galleryButton;
    [SerializeField] private TextMeshProUGUI galleryPhotoCount;

    public PressableButton taskButton;
    private int taskId;
    public void UpdateDisplayInformation(int number, string title, TaskInfo.TaskStatus status, int numberOfPhotos)
    {
        taskId = number;
        stepNumberText.text = number.ToString();
        taskTitleText.text = title;
        galleryPhotoCount.text = numberOfPhotos.ToString();
        Debug.Log("Status is: " + status);
        if (status == 0)
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
            switch (status)
            {
                case "TO DO":
                    MainMenuManager.Instance.currentJob.ChangeTaskStatus(taskId, TaskInfo.TaskStatus.ToDo);
                    break;
                case "IN PROGRESS":
                    MainMenuManager.Instance.currentJob.ChangeTaskStatus(taskId, TaskInfo.TaskStatus.InProgress);
                    break;
                case "COMPLETED":
                    MainMenuManager.Instance.currentJob.ChangeTaskStatus(taskId, TaskInfo.TaskStatus.Completed);
                    break;
                case "ON HOLD":
                    MainMenuManager.Instance.currentJob.ChangeTaskStatus(taskId, TaskInfo.TaskStatus.OnHold);
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
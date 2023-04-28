using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private SelectionDropdown statusDropDown;

    public PressableButton taskButton;
    private int taskId;

    private void Awake()
    {
        statusDropDown.Build(Enum.GetNames(typeof(TaskInfo.TaskStatus)).ToList());
        statusDropDown.OnOptionSelected = UpdateTaskStatusViaButton;
    }

    private string BuildStatusText(TaskInfo.TaskStatus status) 
    {
        return StringUtil.AddSpace(status.ToString()).ToUpper();
    }
    public void UpdateDisplayInformation(int number, string title, TaskInfo.TaskStatus status, int numberOfPhotos)
    {
        taskId = number;
        stepNumberText.text = number.ToString();
        taskTitleText.text = title;
        galleryPhotoCount.text = numberOfPhotos.ToString();
        statusText.text = BuildStatusText(status);

    }


    public void UpdateTaskStatusViaButton(int _status)
    {
        TaskInfo.TaskStatus status = (TaskInfo.TaskStatus)_status;

        if (MainMenuManager.Instance != null && MainMenuManager.Instance.currentJob != null)
        {
            MainMenuManager.Instance.currentJob.ChangeTaskStatus(taskId, status);
        }
        else
        {
            Debug.Log("Error: MainMenuManager instance or selectedJobListItem is null");
        }
        statusText.text = BuildStatusText(status);



    }

    public void UpdateTaskStatusViaButton(TaskInfo.TaskStatus status)
    {
        if (MainMenuManager.Instance != null && MainMenuManager.Instance.currentJob != null)
        {
            MainMenuManager.Instance.currentJob.ChangeTaskStatus(taskId, status);
        }
        else
        {
            Debug.Log("Error: MainMenuManager instance or selectedJobListItem is null");
        }
        statusText.text = BuildStatusText(status);


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
        statusText.text = status;

    }

}
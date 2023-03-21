using System;
using System.Collections;
using System.Collections.Generic;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.JobManagement;
using ARA.Frontend;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class MainMenuAesthetic : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateDisplay;

    [Header("Task Menu Banner")]
    [SerializeField] private TextMeshProUGUI vehicleInformationText;
    [SerializeField] private TextMeshProUGUI taskTitle;
    [SerializeField] private TextMeshProUGUI folderTitle;

    // [SerializeField] private TextMeshProUGUI vinText;
    //
    // [SerializeField] private TextMeshProUGUI jobNumberText;
    // [SerializeField] private TextMeshProUGUI vehicleTitleText;

    [SerializeField] private TextMeshProUGUI estimatorTextJobs;
    [SerializeField] private TextMeshProUGUI estimatorTextTasks;
    [SerializeField]
    private LogInMenu _logInMenu;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (_logInMenu == null)
        {
            _logInMenu = FindObjectOfType<LogInMenu>();
        }
        LogInMenu.OnAccountSelected += UpdateEstimatorName;
        if(dateDisplay!=null)
            UpdateDateDisplay();
    }

    private void UpdateEstimatorName()
    {
        if(estimatorTextJobs!=null)
            estimatorTextJobs.text = _logInMenu.estimatorName;
        if(estimatorTextTasks!=null)
            estimatorTextTasks.text = _logInMenu.estimatorName;
    }
    [ContextMenu("UpdateDate")]
    private void UpdateDateDisplay()
    {
        dateDisplay.text = (DateTime.Now.DayOfWeek +
                            " " +
                            DateTime.Today.Month +
                            "/" +
                            DateTime.Today.Day +
                            "/" +
                            DateTime.Now.Year);
    }
    public void UpdateTaskDisplay(JobListItemDto chosenJob, TaskInfo task)
    {
        vehicleInformationText.text = $"Job#: {chosenJob.Id} , {chosenJob.CarInfo.Manufacturer} {chosenJob.CarInfo.Model} {chosenJob.CarInfo.Year} , VIN: {chosenJob.CarInfo.Vin}";
        taskTitle.text = task.Title;
        folderTitle.text = task.RepairManuals[0].Name;
    }
}

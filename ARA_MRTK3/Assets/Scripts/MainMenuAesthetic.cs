using System;
using System.Collections;
using System.Collections.Generic;
using Ara.Domain.ApiClients.Dtos;
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
    [SerializeField] private TextMeshProUGUI vinText;

    [SerializeField] private TextMeshProUGUI jobNumberText;
    [SerializeField] private TextMeshProUGUI vehicleTitleText;

    [SerializeField] private TextMeshProUGUI estimatorText;
    [SerializeField]
    private LogInMenu _logInMenu;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (_logInMenu == null)
        {
            _logInMenu = FindObjectOfType<LogInMenu>();
        }
        if(estimatorText!=null)
            estimatorText.text = _logInMenu.estimatorName;
        if(dateDisplay!=null)
            UpdateDateDisplay();
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
    public void UpdateTaskDisplay(JobListItemDto chosenJob)
    {
        vinText.text = chosenJob.CarInfo.Vin;
        jobNumberText.text = "Job# " + chosenJob.Id;
        vehicleTitleText.text = $"{chosenJob.CarInfo.Manufacturer} {chosenJob.CarInfo.Model} {chosenJob.CarInfo.Year}";
    }
}
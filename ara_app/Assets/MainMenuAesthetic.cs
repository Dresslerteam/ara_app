using System;
using System.Collections;
using System.Collections.Generic;
using ARA.Frontend;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class MainMenuAesthetic : MonoBehaviour
{

    [SerializeField] private Image background;
    [SerializeField] private Image banner;
    [SerializeField] private TextMeshProUGUI dateDisplay;
    [Header("Colors")]
    [SerializeField][OnValueChanged("UpdateColorScheme")]
    private Color primaryColor = Color.blue;
    [SerializeField][OnValueChanged("UpdateColorScheme")]
    private Color secondaryColor = Color.grey;

    [Header("Task Menu Banner")]
    [SerializeField] private TextMeshProUGUI vinText;

    [SerializeField] private TextMeshProUGUI jobNumberText;
    [SerializeField] private TextMeshProUGUI vehicleTitleText;
    // Start is called before the first frame update
    void Start()
    {
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
    public void UpdateColorScheme()
    {
        //background.color = primaryColor;
        //banner.color = secondaryColor;
    }

    public void UpdateTaskDisplay(Job chosenJob)
    {
        vinText.text = chosenJob.vin;
        jobNumberText.text = "Job# " + chosenJob.jobNumber;
        vehicleTitleText.text = chosenJob.GetVehicleTitleString();
    }
}

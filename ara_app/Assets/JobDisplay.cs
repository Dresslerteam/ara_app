using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class JobDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI jobNumberText;

    [SerializeField] private TextMeshProUGUI vehicleTitleText;

    [SerializeField] private TextMeshProUGUI completenessText;
    
    [SerializeField] private Image completenessImage;
    [SerializeField] private Color incompleteColor = Color.gray;
    [SerializeField] private Color completeColor = Color.green;


    public void UpdateDisplayInformation(string number, string title, string complete, float fillAmount)
    {
        jobNumberText.text = number;
        vehicleTitleText.text = title;
        completenessText.text = complete != "100%" ? complete : "Done";
        completenessImage.fillAmount = fillAmount/100;
        completenessImage.color = fillAmount > 99 ? completeColor : incompleteColor;
    }
}

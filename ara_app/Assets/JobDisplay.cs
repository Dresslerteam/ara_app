using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class JobDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI jobNumberText;

    [SerializeField] private TextMeshProUGUI vehicleTitleText;

    [SerializeField] private TextMeshProUGUI completenessText;

    public void UpdateDisplayInformation(string number, string title, string complete)
    {
        jobNumberText.text = number;
        vehicleTitleText.text = title;
        completenessText.text = complete != "100%" ? complete : "Done";
    }
}

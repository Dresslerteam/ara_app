using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.GraphicsTools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class JobDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI jobNumberText;

    [SerializeField] private TextMeshProUGUI vehicleTitleText;

    [SerializeField] private TextMeshProUGUI completenessText;
    public Button jobButton;
    
    [SerializeField] private Image completenessImage;

    [SerializeField] private CanvasMaterialAnimatorCanvasProgressBar progressBar;
    //[SerializeField] private Color incompleteColor = Color.gray;
    //[SerializeField] private Color completeColor = Color.green;


    public void UpdateDisplayInformation(string number, string title, string complete, float fillAmount)
    {
        jobNumberText.text = number;
        vehicleTitleText.text = title;
        completenessText.text = complete != "100%" ? complete : "Done"; //Todo: enum of Todo, working on, blocked, done
        if (progressBar != null)
        {
            progressBar.ApplyToMaterial();
            progressBar._Filled_Fraction_ = fillAmount / 100f;
            progressBar.ApplyToMaterial();
        }
        //completenessImage.fillAmount = fillAmount/100;
        //completenessImage.color = fillAmount > 99 ? completeColor : incompleteColor;
        //TODO: Set it to work with MRTK feedback
    }
}

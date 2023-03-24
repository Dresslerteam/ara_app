using System;
using System.Collections;
using System.Collections.Generic;
using Ara.Domain.ApiClients.Dtos;
using Microsoft.MixedReality.GraphicsTools;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class JobDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI jobNumberText;
    [SerializeField] private TextMeshProUGUI customerText;
    [SerializeField] private TextMeshProUGUI claimNumberText;
    [SerializeField] private TextMeshProUGUI writtenByText;
    [SerializeField] private TextMeshProUGUI vehicleTitleText;
    [SerializeField] private TextMeshProUGUI vinText;
    

    [SerializeField] private TextMeshProUGUI completenessText;
    public PressableButton jobButton;
    public PressableButton pdfButton;
    [SerializeField] private Image progressBar;
    //[SerializeField] private Color incompleteColor = Color.gray;
    //[SerializeField] private Color completeColor = Color.green;


    public void UpdateDisplayInformation(string number, string customer, string claim, string author, string title,
        string vin, string complete, float fillAmount, JobListItemDto availableJobListItem)
    {
        jobNumberText.text = number;
        customerText.text = customer;
        claimNumberText.text = claim;
        writtenByText.text = author;
        vehicleTitleText.text = title;
        vinText.text = vin;
        if(completenessText!=null)
            completenessText.text = complete != "100%" ? complete : "Done"; //Todo: enum of Todo, working on, blocked, done
        if (progressBar != null)
        {
            progressBar.fillAmount = fillAmount / 1f;
            // 
        }
        pdfButton.OnClicked.AddListener(() =>
        {
            if (pdfButton.IsToggled == true)
            {
                MainMenuManager.Instance.pdfLoader.LoadPdf(availableJobListItem.PreliminaryEstimation.Url);
            }
            else
            {
                MainMenuManager.Instance.pdfLoader.HidePdf();

            }
        });
        //completenessImage.fillAmount = fillAmount/100;
        //completenessImage.color = fillAmount > 99 ? completeColor : incompleteColor;
        //TODO: Set it to work with MRTK feedback
    }
}

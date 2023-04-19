using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ara.Domain.ApiClients.Dtos;
using Microsoft.MixedReality.GraphicsTools;
using Microsoft.MixedReality.Toolkit;
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
    public PressableButton galleryButton;
    [SerializeField] private Image progressBar;
    private ToggleCollection toggleCollection;
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
        if (completenessText != null)
            completenessText.text = complete != "100%" ? complete : "Done"; //Todo: enum of Todo, working on, blocked, done
        if (progressBar != null)
        {
            progressBar.fillAmount = fillAmount / 1f;
            // 
        }

        SetupToggleButton(availableJobListItem);
        pdfButton.ForceSetToggled(false);
    }

    private void SetupToggleButton(JobListItemDto availableJobListItem)
    {
        if (pdfButton != null)
            Debug.Log($"pdfButton is not null");

        if (galleryButton != null)
            Debug.Log($"galleryButton is not null");
        // Add the button to the toggle collection
        toggleCollection = GetComponentInParent<ToggleCollection>();
        toggleCollection.Toggles.Add(pdfButton);
        // If toggle is selected and the pdfButton is toggled, force disable the toggle
        toggleCollection.OnToggleSelected.AddListener((ctx) =>
        {
            if (toggleCollection.Toggles[ctx] != pdfButton)
            {
                pdfButton.ForceSetToggled(false, false);
            }
        });
        // Set the toggle mode to toggle
        pdfButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
        pdfButton.OnClicked.AddListener(() =>
        {
            if (pdfButton.IsToggled == true)
            {
                if (pdfButton.ToggleMode != StatefulInteractable.ToggleType.Toggle)
                    pdfButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
                MainMenuManager.Instance.pdfLoader.LoadPdf(availableJobListItem.PreliminaryEstimation.Url);
                Debug.Log($"{gameObject.name}PDF Loaded");
                pdfButton.ForceSetToggled(true, true);
            }
            else if (pdfButton.IsToggled == false)
            {
                if (pdfButton.ToggleMode != StatefulInteractable.ToggleType.Toggle)
                    pdfButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
                MainMenuManager.Instance.pdfLoader.HidePdf();
                Debug.Log($"{gameObject.name}PDF Hidden");
                pdfButton.ForceSetToggled(false, true);
            }
        });

        galleryButton.OnClicked.AddListener(async () =>
        {
            //todo:this solution is not optimal as it needs to load whole Job object
            var curJob = await MainMenuManager.Instance.applicationService.GetJobDetailsAsync(availableJobListItem.Id);
            MainMenuManager.Instance.currentJob = curJob;
            MainMenuManager.Instance.SetToGalleryViewFromJobList();
        });
    }
}

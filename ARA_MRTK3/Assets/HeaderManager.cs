using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

public class HeaderManager : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject jobsButton;
    public GameObject tasksButton;
    public GameObject hideButton;
    public GameObject logoutButton;
    public PressableButton scanDocButton;
    public PressableButton pdfEstimationButton;
    public GameObject modelButton;
    public GameObject cameraHeader;
    [Header("Texts")] 
    public GameObject menuTitleWithUserTexts;
    public GameObject jobinformationTexts;
    [Header("Collections")] public ToggleCollection DocButtonToggleCollection;
    private void OnEnable()
    {
        MainMenuManager.OnMenuPageChanged += UpdateButton;
    }

    private void OnDisable()
    {
        MainMenuManager.OnMenuPageChanged -= UpdateButton;
    }

    private void Start()
    {
        scanDocButton.ForceSetToggled(false);
        pdfEstimationButton.ForceSetToggled(false);
    }

    public void UpdateButton(MenuPage selectedPage)
    {
        switch (selectedPage)
        {
            case MenuPage.splashScreen:
                jobsButton?.SetActive(false);
                tasksButton?.SetActive(false);
                hideButton?.SetActive(false);
                logoutButton?.SetActive(false);
                scanDocButton?.gameObject.SetActive(false);
                pdfEstimationButton?.gameObject.SetActive(false);
                //modelButton?.SetActive(false);
                menuTitleWithUserTexts?.SetActive(false);
                jobinformationTexts.SetActive(false);
                break;

            case MenuPage.loginScreen:
                jobsButton?.SetActive(false);
                tasksButton?.SetActive(false);
                hideButton?.SetActive(false);
                logoutButton?.SetActive(false);
                scanDocButton?.gameObject.SetActive(false);
                pdfEstimationButton?.gameObject.SetActive(false);
               // modelButton?.SetActive(false);
                menuTitleWithUserTexts?.SetActive(false);
                jobinformationTexts.SetActive(false);
                break;

            case MenuPage.jobSelectScreen:
                jobsButton?.SetActive(true);
                tasksButton?.SetActive(false);
                hideButton?.SetActive(true);
                logoutButton?.SetActive(true);
                scanDocButton?.gameObject.SetActive(false);
                pdfEstimationButton?.gameObject.SetActive(false);
               // modelButton?.SetActive(false);
                menuTitleWithUserTexts?.SetActive(true);
                jobinformationTexts.SetActive(false);
                break;

            case MenuPage.modelOverview:
                break;

            case MenuPage.taskSelect:
                jobsButton?.SetActive(true);
                tasksButton?.SetActive(true);
                hideButton?.SetActive(true);
                logoutButton?.SetActive(false);
                scanDocButton?.gameObject.SetActive(true);
                pdfEstimationButton?.gameObject.SetActive(true);
               // modelButton?.SetActive(true);
                menuTitleWithUserTexts?.SetActive(true);
                jobinformationTexts.SetActive(false);
                break;

            case MenuPage.performingJob:
                jobsButton?.SetActive(true);
                tasksButton?.SetActive(true);
                hideButton?.SetActive(true);
                logoutButton?.SetActive(false);
                scanDocButton?.gameObject.SetActive(true);
                pdfEstimationButton?.gameObject.SetActive(true);
               // modelButton?.SetActive(true);
                menuTitleWithUserTexts?.SetActive(false);
                jobinformationTexts.SetActive(true);
                break;
            case MenuPage.takingPhoto:
                jobsButton?.SetActive(false);
                tasksButton?.SetActive(false);
                hideButton?.SetActive(false);
                logoutButton?.SetActive(false);
                scanDocButton?.gameObject.SetActive(false);
                pdfEstimationButton?.gameObject.SetActive(false);
               // modelButton?.SetActive(false);
                menuTitleWithUserTexts?.SetActive(false);
                jobinformationTexts.SetActive(false);
                cameraHeader.SetActive(true);
                break;
            default:
                Debug.LogError("No menu page selected");
                break;
        }
    }
}


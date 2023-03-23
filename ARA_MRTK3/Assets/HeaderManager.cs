using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeaderManager : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject jobsButton;
    public GameObject tasksButton;
    public GameObject hideButton;
    public GameObject logoutButton;
    public GameObject scanDocButton;
    public GameObject pdfEstimationButton;
    public GameObject modelButton;
    [Header("Texts")] 
    public GameObject menuTitleWithUserTexts;
    public GameObject jobinformationTexts;
    private void OnEnable()
    {
        MainMenuManager.OnMenuPageChanged += UpdateButton;
    }

    public void UpdateButton(MenuPage selectedPage)
    {
        switch (selectedPage)
        {
            case MenuPage.splashScreen:
                jobsButton.SetActive(false);
                tasksButton.SetActive(false);
                hideButton.SetActive(false);
                logoutButton.SetActive(false);
                scanDocButton.SetActive(false);
                pdfEstimationButton.SetActive(false);
                modelButton.SetActive(false);
                menuTitleWithUserTexts.SetActive(false);
                jobinformationTexts.SetActive(false);
                break;
            case MenuPage.loginScreen:
                jobsButton.SetActive(false);
                tasksButton.SetActive(false);
                hideButton.SetActive(false);
                logoutButton.SetActive(false);
                scanDocButton.SetActive(false);
                pdfEstimationButton.SetActive(false);
                modelButton.SetActive(false);
                menuTitleWithUserTexts.SetActive(false);
                jobinformationTexts.SetActive(false);
                break;
            case MenuPage.jobSelectScreen:
                jobsButton.SetActive(true);
                tasksButton.SetActive(false);
                hideButton.SetActive(true);
                logoutButton.SetActive(true);
                scanDocButton.SetActive(false);
                pdfEstimationButton.SetActive(false);
                modelButton.SetActive(false);
                menuTitleWithUserTexts.SetActive(true);
                jobinformationTexts.SetActive(false);
                break;
            case MenuPage.modelOverview:
                break;
            case MenuPage.taskSelect:
                jobsButton.SetActive(true);
                tasksButton.SetActive(true);
                hideButton.SetActive(true);
                logoutButton.SetActive(false);
                scanDocButton.SetActive(true);
                pdfEstimationButton.SetActive(true);
                modelButton.SetActive(true);
                menuTitleWithUserTexts.SetActive(true);
                jobinformationTexts.SetActive(false);
                break;
            case MenuPage.performingJob:
                jobsButton.SetActive(true);
                tasksButton.SetActive(true);
                hideButton.SetActive(true);
                logoutButton.SetActive(false);
                scanDocButton.SetActive(true);
                pdfEstimationButton.SetActive(true);
                modelButton.SetActive(true);
                menuTitleWithUserTexts.SetActive(false);
                jobinformationTexts.SetActive(true);
                break;
            default:
                Debug.LogError("No menu page selected");
                break;
        }
    }
}


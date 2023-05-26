using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

public class HeaderManager : MonoBehaviour
{
    [Header("Buttons")]
    public PressableButton jobsButton;
    public PressableButton tasksButton;
    public GameObject hideButton;
    public GameObject logoutButton;
    public PressableButton scanDocButton;
    public PressableButton pdfEstimationButton;
    public GameObject modelButton;
    public CameraHeaderManager cameraHeaderManager;
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

    public void UpdateButton(MenuPage selectedPage, MenuPage parentMenuPage)
    {
        //Debug.Log("Select page  " + selectedPage);
        switch (selectedPage)
        {
            case MenuPage.splashScreen:
                jobsButton?.gameObject.SetActive(false);

                tasksButton.gameObject.SetActive(false);

                hideButton?.SetActive(false);
                logoutButton?.SetActive(false);
                scanDocButton?.gameObject.SetActive(false);
                pdfEstimationButton?.gameObject.SetActive(false);
                modelButton?.SetActive(false);
                menuTitleWithUserTexts?.SetActive(false);
                jobinformationTexts.SetActive(false);
                cameraHeaderManager?.gameObject.SetActive(false);
                break;

            case MenuPage.loginScreen:
                jobsButton?.gameObject.SetActive(false);
                tasksButton.gameObject.SetActive(false);

                hideButton?.SetActive(false);
                logoutButton?.SetActive(false);
                scanDocButton?.gameObject.SetActive(false);
                pdfEstimationButton?.gameObject.SetActive(false);
                modelButton?.SetActive(false);
                menuTitleWithUserTexts?.SetActive(false);
                jobinformationTexts.SetActive(false);
                cameraHeaderManager?.gameObject.SetActive(false);

                break;

            case MenuPage.jobSelectScreen:
                jobsButton?.gameObject.SetActive(true);
                jobsButton.ForceSetToggled(true, true);

                tasksButton.gameObject.SetActive(false);
                tasksButton.ForceSetToggled(false, true);
                hideButton?.SetActive(true);
                logoutButton?.SetActive(true);
                scanDocButton?.gameObject.SetActive(false);
                pdfEstimationButton?.gameObject.SetActive(false);
                modelButton?.SetActive(false);
                menuTitleWithUserTexts?.SetActive(true);
                jobinformationTexts.SetActive(false);
                cameraHeaderManager?.gameObject.SetActive(false);

                break;

            case MenuPage.modelOverview:
                break;

            case MenuPage.taskSelect:
                jobsButton?.gameObject.SetActive(true);
                jobsButton.ForceSetToggled(false, true);

                tasksButton.gameObject.SetActive(true);
                tasksButton.ForceSetToggled(true, true);

                hideButton?.SetActive(true);
                logoutButton?.SetActive(false);
                scanDocButton?.gameObject.SetActive(true);
                pdfEstimationButton?.gameObject.SetActive(true);
                modelButton?.SetActive(true);
                menuTitleWithUserTexts?.SetActive(false);
                jobinformationTexts.SetActive(true);
                cameraHeaderManager?.gameObject.SetActive(false);

                break;

            case MenuPage.performingJob:
                jobsButton?.gameObject.SetActive(true);
                tasksButton.gameObject.SetActive(true);
                tasksButton.ForceSetToggled(false, false);
                hideButton?.SetActive(true);
                logoutButton?.SetActive(false);
                scanDocButton?.gameObject.SetActive(true);
                pdfEstimationButton?.gameObject.SetActive(true);
                modelButton?.SetActive(true);
                menuTitleWithUserTexts?.SetActive(false);
                jobinformationTexts.SetActive(true);
                cameraHeaderManager?.gameObject.SetActive(false);

                break;
            case MenuPage.takingPhoto:
                jobsButton?.gameObject.SetActive(false);
                tasksButton.gameObject.SetActive(false);

                hideButton?.SetActive(false);
                logoutButton?.SetActive(false);
                scanDocButton?.gameObject.SetActive(false);
                pdfEstimationButton?.gameObject.SetActive(false);
                modelButton?.SetActive(false);
                menuTitleWithUserTexts?.SetActive(false);
                jobinformationTexts.SetActive(false);
                cameraHeaderManager?.gameObject.SetActive(true);
                break;
            case MenuPage.gallery:
                {
                    if (parentMenuPage == MenuPage.jobSelectScreen)
                    {

                    }
                    else
                    {
                        jobsButton?.gameObject.SetActive(true);
                        tasksButton.gameObject.SetActive(true);
                        tasksButton.ForceSetToggled(false, false);
                        hideButton?.SetActive(true);
                        logoutButton?.SetActive(false);
                        scanDocButton?.gameObject.SetActive(true);
                        pdfEstimationButton?.gameObject.SetActive(true);
                        modelButton?.SetActive(true);
                        menuTitleWithUserTexts?.SetActive(false);
                        jobinformationTexts.SetActive(true);
                        cameraHeaderManager?.gameObject.SetActive(false);
                    }
                }

                break;
            default:
                Debug.LogError("No menu page selected");
                break;
        }
    }
}


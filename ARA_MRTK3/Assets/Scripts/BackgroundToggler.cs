using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundToggler : MonoBehaviour
{
    [SerializeField] private GameObject outlineFill;
    [SerializeField] private GameObject topFill;
    [SerializeField] private GameObject bottomFill;
    public ActiveBackground _activeBackground;
    
    private void OnEnable()
    {
        MainMenuManager.OnMenuPageChanged += MenuHasSwitched;
    }

    private void OnDisable()
    {
        MainMenuManager.OnMenuPageChanged -= MenuHasSwitched;
    }

    private void MenuHasSwitched(MenuPage menuPage)
    {
        Debug.Log("Menu has switched to: " + menuPage);
        switch (menuPage)
        {
            case MenuPage.splashScreen:
                ToggleBackground(ActiveBackground.None);
                break;
            case MenuPage.loginScreen:
                ToggleBackground(ActiveBackground.None);
                break;
            case MenuPage.jobSelectScreen:
                ToggleBackground(ActiveBackground.All);
                break;
            case MenuPage.modelOverview:
                break;
            case MenuPage.taskSelect:
                ToggleBackground(ActiveBackground.All);
                break;
            case MenuPage.performingJob:
                ToggleBackground(ActiveBackground.All);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(menuPage), menuPage, null);
        }
    }

    public void ToggleBackground(ActiveBackground activeBackground)
    {
        _activeBackground = activeBackground;
        if (activeBackground == ActiveBackground.None)
        {
            // set all to false
            topFill.SetActive(false);
            bottomFill.SetActive(false);
            outlineFill.SetActive(false);
            return;
            
        }
        if (activeBackground == ActiveBackground.All)
        {
            // set all to false
            topFill.SetActive(true);
            bottomFill.SetActive(true);
            outlineFill.SetActive(true);
            return;
        }
        topFill.SetActive(activeBackground == ActiveBackground.Top);
        bottomFill.SetActive(activeBackground == ActiveBackground.Bottom);
        outlineFill.SetActive(activeBackground == ActiveBackground.Outline);
    }
    
}

[System.Flags]
public enum ActiveBackground
{
    Outline = 1<<1,
    Top = 1<<2,
    Bottom = 1<<3,
    None = 0,
    All = Outline | Top | Bottom
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandMenuController : MonoBehaviour
{


    [SerializeField] private GameObject cameraButton;
    [SerializeField] private GameObject galleryButton;
    [SerializeField] private GameObject rulerButton;
    [SerializeField] private GameObject voiceButton;

    private List<GameObject> allButtons = new List<GameObject>();


    private void Awake()
    {
        allButtons.Add(cameraButton);
        allButtons.Add(galleryButton);
        allButtons.Add(rulerButton);
        allButtons.Add(voiceButton);

        MainMenuManager.OnMenuPageChanged += OnMenuChanged;

    }
    private void OnMenuChanged(MenuPage next, MenuPage prev)
    {
        Debug.Log("On menu changed " + next.ToString());
        HandleNewScene(next);
    }
  

    private void HandleNewScene(MenuPage page)
    {
        switch (page)
        {
            case MenuPage.splashScreen:
            case MenuPage.loginScreen:
                AllButtonsOff();
                break;
            case MenuPage.jobSelectScreen:
                cameraButton.SetActive(false);
                galleryButton.SetActive(false);
                rulerButton.SetActive(true);
                voiceButton.SetActive(true);
                break;
            case MenuPage.taskSelect:
            case MenuPage.performingJob:
                AllButtonsOn();
                break;
            case MenuPage.takingPhoto:
                cameraButton.SetActive(false);
                galleryButton.SetActive(false);
                rulerButton.SetActive(true);
                voiceButton.SetActive(true);
                break;
            case MenuPage.gallery:
                cameraButton.SetActive(false);
                galleryButton.SetActive(false);
                rulerButton.SetActive(true);
                voiceButton.SetActive(true);
                break;

        }
    }

    private void AllButtonsOff()
    {
        foreach(GameObject button in allButtons) button.SetActive(false);
    }
    private void AllButtonsOn()
    {
        foreach (GameObject button in allButtons) button.SetActive(true);
    }
}

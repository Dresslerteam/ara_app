using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApplicationServices;
using Ara.Domain.JobManagement;
using ARA.Frontend;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MainMenuAesthetic))]
public class MainMenuManager : MonoBehaviour
{
    [Header("Menus")]
    private MainMenuAesthetic mainMenuAesthetic;

    public GameObject splashScreen;
    public GameObject loginBoard;
    public GameObject jobBoard;
    public GameObject taskBoard;
    public GameObject loaderGO;
    [SerializeField] private QuickMenuDisplay jobQuickMenu;
    [Header("Collection Roots")]
    [SerializeField] private Transform jobSelectionRoot;
    [SerializeField] private Transform taskSelectionRoot;
    

    [Header("Button Prefabs")]
    [SerializeField]
    private GameObject jobButton;
    [SerializeField]
    private GameObject taskButton;
    [Space(10)]
    public List<JobListItemDto> availbleJobs = new List<JobListItemDto>();
    JobApplicationService applicationService = new JobApplicationService();
    private static MainMenuManager _instance;
    public static MainMenuManager Instance { get { return _instance; } }


    public MenuPage currentMenuPage = MenuPage.splashScreen;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        currentMenuPage = MenuPage.splashScreen;
    }

    // The user has selected their account...
    public async Task LoggedIn()
    {
        mainMenuAesthetic = GetComponent<MainMenuAesthetic>();
        currentMenuPage = MenuPage.jobSelectScreen;
        await UpdateJobBoard();
    }

    private void ToggleAllMenus(bool isOn)
    {
        jobBoard.SetActive(isOn);
        if(taskBoard!=null)
            taskBoard.SetActive(isOn);
        if(jobQuickMenu!=null)
            jobQuickMenu.gameObject.SetActive(isOn);
    }

    public async Task UpdateJobBoard()
    {
        //var service = new JobApplicationService();

        ToggleAllMenus(false);
        jobBoard.SetActive(true);
        ClearChildrenButtons(jobSelectionRoot);
        currentMenuPage = MenuPage.jobSelectScreen;
        if(loaderGO!=null) loaderGO.SetActive(true);
        availbleJobs = await applicationService.GetJobsAsync();
        if(loaderGO!=null) loaderGO.SetActive(false);
        foreach (var job in availbleJobs)
        {
            //var curJob = await applicationService.GetJobsAsync(job.Id);
            GameObject jobButton = Instantiate(this.jobButton, jobSelectionRoot);
            //jobButton.transform.localScale = new Vector3(.14f, .14f, .14f);
            JobDisplay jobDisplay = jobButton.GetComponent<JobDisplay>();
            //Interactable jobDisplayInteractable = jobDisplay.GetComponent<Interactable>();
            PressableButton jobDisplayInteractable = jobDisplay.jobButton;

            float tasksDone = 0;

            jobDisplayInteractable.enabled = job.Progress <= 99;
            //...complete: job.progress, is now Random for demonstration purposes
            jobDisplay.UpdateDisplayInformation("Job# " + job.Code,
                $"{job.CarInfo.Manufacturer} {job.CarInfo.Model} {job.CarInfo.Year}",
                Random.Range(0,100) + "%",
                (float)job.Progress);
            jobDisplayInteractable.OnClicked.AddListener(AddJobToButton(job));
            await Task.Yield();
        }
    }
    private UnityAction AddJobToButton(JobListItemDto job)
    {
        UnityAction chosenJob = delegate { AdvanceToTaskList(job); };
        return chosenJob;
    }
    private void ClearChildrenButtons(Transform root)
    {
        int childCount = root.childCount;

        foreach (Transform c in root)
        {
            GameObject.Destroy(c.gameObject);
        }
    }
    
    /// <summary>
    /// Once a job has been chosen, populate the list with tasks that the job holds
    /// </summary>
    /// <param name="chosenJob">The job that was...chosen</param>
    public async void AdvanceToTaskList(JobListItemDto chosenJob)
    {
        ToggleAllMenus(false);
        ClearChildrenButtons(taskSelectionRoot);
        taskBoard.SetActive(true);
        mainMenuAesthetic.UpdateTaskDisplay(chosenJob);
        currentMenuPage = MenuPage.taskSelect;

        int stepIndex = 0;
        if(loaderGO!=null) loaderGO.SetActive(true);
        var jobDetails = await applicationService.GetJobDetailsAsync(chosenJob.Id);
        if(loaderGO!=null) loaderGO.SetActive(false);
        ////Debug.Log(chosenJob.tasks.Count);
        foreach (var jobTask in jobDetails.Tasks)
        {
            GameObject newTaskButton = Instantiate(taskButton, taskSelectionRoot);
            //newTaskButton.transform.localScale = new Vector3(.14f, .14f, .14f);
            TaskDisplay taskDisplay = newTaskButton.GetComponent<TaskDisplay>();
            PressableButton taskDisplayInteractable = taskDisplay.taskButton;
            //taskDisplayInteractable.interactable = jobTask.Status != Task.TaskStatus.Completed;
            stepIndex++;
            taskDisplay.UpdateDisplayInformation(jobTask.Id.ToString("D2"),jobTask.Title, jobTask.Status);
            //string curStep = stepIndex.ToString("D2");                
            //taskDisplay.UpdateDisplayInformation(curStep, jobTask.taskTitle, jobTask.isComplete);
            await Task.Yield();
        }
    }

    public void AdvanceToWorkingView()
    {
        ToggleAllMenus(false);
        currentMenuPage = MenuPage.performingJob;
        if(jobQuickMenu!=null)
            jobQuickMenu.gameObject.SetActive(true);
    }
    public void ReturnToPreviousPage()
    {
        switch (currentMenuPage)
        {
            case MenuPage.splashScreen:
                break;
            case MenuPage.loginScreen:
                ReturnToSplash();
                break;
            case MenuPage.jobSelectScreen:
                ReturnToLogin();
                break;
            case MenuPage.taskSelect:
                ReturnToMenu();
                break;
            case MenuPage.performingJob:
                ReturnToMenu();
                break;
            default:
                ReturnToMenu();
                break;
        }
    }
    public void ReturnToMenu()
    {
        UpdateJobBoard();
    }

    public void ReturnToLogin()
    {
        ToggleAllMenus(false);
        loginBoard.SetActive(true);
    }

    public void ReturnToSplash()
    {
        ToggleAllMenus(false);
        splashScreen.SetActive(true);
    }
}
public enum MenuPage
{
    splashScreen,
    loginScreen,
    jobSelectScreen,
    taskSelect,
    performingJob
}
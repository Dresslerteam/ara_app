using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApplicationServices;
using Ara.Domain.Common.Interfaces;
using Ara.Domain.Common.Services;
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
    public GameObject taskOverview;
    public GameObject loaderGO;
    [Header("ModelOverview")]
    public GameObject modelOverviewGO;
    public GameObject modelOveriewCallOuts;
    [SerializeField] private QuickMenuDisplay jobQuickMenu;
    [Header("Buttons")] [SerializeField] private PressableButton advanceToTaskButton; 
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
    IApplicationEventDispatcher applicationEventDispatcher = new ApplicationEventDispatcher();
    private static MainMenuManager _instance;
    public static MainMenuManager Instance { get { return _instance; } }


    public MenuPage currentMenuPage = MenuPage.splashScreen;
    
    public JobListItemDto selectedJob;

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
        //Todo: I believe quick menu is not needed anymore
        //if(jobQuickMenu!=null)
            //jobQuickMenu.gameObject.SetActive(isOn);
    }

    public async Task UpdateJobBoard()
    {
        var service = new JobApplicationService();

        ToggleAllMenus(false);
        jobBoard.SetActive(true);
        ClearChildrenButtons(jobSelectionRoot);
        currentMenuPage = MenuPage.jobSelectScreen;
        if(loaderGO!=null) loaderGO.SetActive(true);
        availbleJobs = await applicationService.GetJobsAsync();
        if(loaderGO!=null) loaderGO.SetActive(false);
        foreach (var currentJob in availbleJobs)
        {
            //var curJob = await applicationService.GetJobsAsync(job.Id);
            GameObject jobButton = Instantiate(this.jobButton, jobSelectionRoot);
            //jobButton.transform.localScale = new Vector3(.14f, .14f, .14f);
            JobDisplay jobDisplay = jobButton.GetComponent<JobDisplay>();
            //Interactable jobDisplayInteractable = jobDisplay.GetComponent<Interactable>();
            PressableButton jobDisplayInteractable = jobDisplay.jobButton;

            float tasksDone = 0;

            jobDisplayInteractable.enabled = currentJob.NumberOfTasks <= 99;
            //...complete: job.progress, is now Random for demonstration purposes
            float fillAmount = 0;
            Debug.Log("number of done tasks"+currentJob.NumberOfDoneTasks);
            Debug.Log("number of tasks"+currentJob.NumberOfTasks);
            if(currentJob.NumberOfTasks!=0) 
                fillAmount = (currentJob.NumberOfDoneTasks / (float)currentJob.NumberOfTasks);
            Debug.Log(fillAmount+" is fill");
            jobDisplay.UpdateDisplayInformation("Job# " + currentJob.RepairOrderNo,
                currentJob.CarOwner.FirstName + " " + currentJob.CarOwner.LastName,
                currentJob.ClaimNo,
                currentJob.EstimatorFullName,
                $"{currentJob.CarInfo.Manufacturer} {currentJob.CarInfo.Model} {currentJob.CarInfo.Year}",
                currentJob.CarInfo.Vin,
                (currentJob.NumberOfDoneTasks+"/"+currentJob.NumberOfTasks),fillAmount
                );
            jobDisplayInteractable.OnClicked.AddListener(AddJobToButton(currentJob));
            await Task.Yield();
        }
    }
    private UnityAction AddJobToButton(JobListItemDto job)
    {
        UnityAction chosenJob = delegate { AdvanceToModelOverview(job); };
        selectedJob = job;
        return chosenJob;
    }
    private void ClearChildrenButtons(Transform root)
    {
        foreach (Transform child in root)
        {
            Destroy(child.gameObject);
        }
    }

    public async void AdvanceToModelOverview(JobListItemDto job)
    {
        ReturnToModelOverview();
        //advanceToTaskButton.OnClicked.AddListener(delegate { AdvanceToTaskList(job); });
        currentMenuPage = MenuPage.modelOverview;
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
            taskDisplayInteractable.OnClicked.AddListener(AddTaskToButton(chosenJob));
            //taskDisplayInteractable.interactable = jobTask.Status != Task.TaskStatus.Completed;
            stepIndex++;
            taskDisplay.UpdateDisplayInformation(jobTask.Id.ToString(),jobTask.Title, jobTask.Status, chosenJob);
            await Task.Yield();
        }
    }
    private UnityAction AddTaskToButton(JobListItemDto job)
    {
        UnityAction chosenTask = delegate { AdvanceToWorkingView(); };
        return chosenTask;
    }

    public void AdvanceToWorkingView()
    {
        ToggleAllMenus(false);
        currentMenuPage = MenuPage.performingJob;
        if(taskOverview!=null)
            taskOverview.SetActive(true);
       // if(jobQuickMenu!=null)
          //  jobQuickMenu.gameObject.SetActive(true);
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
            case MenuPage.modelOverview:
                ReturnToMenu();
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
    
    //Todo: These 4 methods don't scale well. Consider looping through a list.
    public void ReturnToMenu()
    {
        UpdateJobBoard();
    }
    public void ReturnToTaskList()
    {
        ToggleAllMenus(false);
        taskBoard.SetActive(true);
        AdvanceToTaskList(selectedJob);
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

    public void ReturnToModelOverview()
    {
        ToggleAllMenus(false);
        modelOverviewGO.SetActive(true);
        modelOveriewCallOuts.SetActive(true);
    }
}
public enum MenuPage
{
    splashScreen,
    loginScreen,
    jobSelectScreen,
    modelOverview,
    taskSelect,
    performingJob
}
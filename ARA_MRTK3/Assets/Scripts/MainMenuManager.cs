using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApplicationServices;
using Ara.Domain.JobManagement;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Job = Ara.Domain.JobManagement.Job;

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
    public WorkingHUDManager workingHUDManager;
    public PDFLoader pdfLoader;
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
    [HideInInspector]
    public JobApplicationService applicationService = new JobApplicationService();

    private static MainMenuManager _instance;
    public static MainMenuManager Instance { get { return _instance; } }


    public MenuPage currentMenuPage = MenuPage.splashScreen;
    public Job currentJob;
    
    public JobListItemDto selectedJobListItem;

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

        ToggleAllMenus(false);
        if (splashScreen != null) splashScreen.SetActive(true);
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
        if(jobBoard!=null)
            jobBoard.SetActive(isOn);
        if(taskBoard!=null)
            taskBoard.SetActive(isOn);
        if(workingHUDManager!=null)
            workingHUDManager.gameObject.SetActive(isOn);
    }

    public async Task UpdateJobBoard()
    {
        ToggleAllMenus(false);
        jobBoard.SetActive(true);
        ClearChildrenButtons(jobSelectionRoot);
        currentMenuPage = MenuPage.jobSelectScreen;
        if(loaderGO!=null) loaderGO.SetActive(true);
        availbleJobs = await applicationService.GetJobsAsync();
        if(loaderGO!=null) loaderGO.SetActive(false);
        foreach (var availableJobListItem in availbleJobs)
        {
            float fillAmount = 0;
            var spawnedJobButton = Instantiate(this.jobButton, jobSelectionRoot);
            var jobDisplay = spawnedJobButton.GetComponent<JobDisplay>();
            var jobDisplayInteractable = jobDisplay.jobButton;

            Debug.Log("number of done tasks"+availableJobListItem.NumberOfDoneTasks);
            Debug.Log("number of tasks"+availableJobListItem.NumberOfTasks);
            
            if(availableJobListItem.NumberOfTasks!=0) 
                fillAmount = (availableJobListItem.NumberOfDoneTasks / (float)availableJobListItem.NumberOfTasks);
            
            jobDisplay.UpdateDisplayInformation("Job# " + availableJobListItem.RepairOrderNo,
                availableJobListItem.CarOwner.FirstName + " " + availableJobListItem.CarOwner.LastName,
                availableJobListItem.ClaimNo,
                availableJobListItem.EstimatorFullName,
                $"{availableJobListItem.CarInfo.Manufacturer} {availableJobListItem.CarInfo.Model} {availableJobListItem.CarInfo.Year}",
                availableJobListItem.CarInfo.Vin,
                (availableJobListItem.NumberOfDoneTasks+"/"+availableJobListItem.NumberOfTasks),fillAmount
                );
            jobDisplayInteractable.OnClicked.AddListener(AddJobToButton(availableJobListItem));
            await Task.Yield();
        }
    }
    private UnityAction AddJobToButton(JobListItemDto jobListItem)
    {
        Debug.Log($"Adding listener for job: {jobListItem.Id}");

        UnityAction chosenJob = delegate
        {
            AdvanceToTaskList(jobListItem);
        };
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
        //ReturnToModelOverview();
        //advanceToTaskButton.OnClicked.AddListener(delegate { AdvanceToTaskList(job); });
        //currentMenuPage = MenuPage.modelOverview;
    }
    /// <summary>
    /// Once a job has been chosen, populate the list with tasks that the job holds
    /// </summary>
    /// <param name="chosenJob">The job that was...chosen</param>
    public async void AdvanceToTaskList(JobListItemDto chosenJob)
    {
        Debug.Log($"Advancing to task list for job: {chosenJob.Id}");

        selectedJobListItem = chosenJob;
        ToggleAllMenus(false);
        ClearChildrenButtons(taskSelectionRoot);
        taskBoard.SetActive(true);
        mainMenuAesthetic.UpdateTaskDisplay(selectedJobListItem);
        currentMenuPage = MenuPage.taskSelect;
        if(loaderGO!=null) loaderGO.SetActive(true);
        currentJob = await applicationService.GetJobDetailsAsync(selectedJobListItem.Id);
        if(loaderGO!=null) loaderGO.SetActive(false);
        
        ////Debug.Log(chosenJob.tasks.Count);
        foreach (var jobTask in currentJob.Tasks)
        {
            Debug.Log($"jobTask.Status: {jobTask.Status}");
            GameObject newTaskButton = Instantiate(taskButton, taskSelectionRoot);
            //newTaskButton.transform.localScale = new Vector3(.14f, .14f, .14f);
            TaskDisplay taskDisplay = newTaskButton.GetComponent<TaskDisplay>();
            PressableButton taskDisplayInteractable = taskDisplay.taskButton;
            taskDisplayInteractable.OnClicked.AddListener(AddTaskToButton(jobTask));
            //taskDisplayInteractable.interactable = jobTask.Status != Task.TaskStatus.Completed;
            taskDisplay.UpdateDisplayInformation(jobTask.Id,jobTask.Title, jobTask.Status);
            await Task.Yield();
        }
    }
    private UnityAction AddTaskToButton(TaskInfo job)
    {
        UnityAction chosenTask = delegate { AdvanceToWorkingView(job); };
        return chosenTask;
    }

    public void AdvanceToWorkingView(TaskInfo job)
    {
        ToggleAllMenus(false);
        currentMenuPage = MenuPage.performingJob;
        if(taskOverview!=null){
            taskOverview.SetActive(true);
        workingHUDManager.PopulateTaskGroups(job);
        }
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
    public async void ReturnToTaskList()
    {
        ToggleAllMenus(false);
        taskBoard.SetActive(true);

        // Fetch the list of available jobs
        var availableJobs = await applicationService.GetJobsAsync();

        // Find the updated job details using the stored job ID
        JobListItemDto updatedJobListItem = availableJobs.FirstOrDefault(job => job.Id == selectedJobListItem.Id);

        if (updatedJobListItem != null)
        {
            AdvanceToTaskList(updatedJobListItem);
        }
        else
        {
            Debug.LogError($"Error: Job with ID {selectedJobListItem.Id} not found in the list of available jobs.");
        }
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
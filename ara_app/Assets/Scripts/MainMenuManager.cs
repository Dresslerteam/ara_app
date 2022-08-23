using System;
using System.Collections;
using System.Collections.Generic;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApplicationServices;
using ARA.Frontend;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Task = Ara.Domain.JobManagement.Task;

[RequireComponent(typeof(MainMenuAesthetic))]
public class MainMenuManager : MonoBehaviour
{
    [Header("Menus")]
    private MainMenuAesthetic mainMenuAesthetic;
    public GameObject jobBoard;
    public GameObject taskBoard;
    [SerializeField] private QuickMenuDisplay jobQuickMenu;

    [SerializeField] private Transform mainMenuBacking;
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
    private enum MenuPage
    {
        jobSelect,
        taskSelect,
        performingJob
    }

    private MenuPage currentMenuPage = MenuPage.jobSelect;

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
    }

    // Start is called before the first frame update
    async void Start()
    {
        mainMenuAesthetic = GetComponent<MainMenuAesthetic>();
        await UpdateJobBoard();
    }

    private void ToggleAllMenus(bool isOn)
    {
        jobBoard.SetActive(isOn);
        taskBoard.SetActive(isOn);
        jobQuickMenu.gameObject.SetActive(isOn);
    }

    public async System.Threading.Tasks.Task UpdateJobBoard()
    {
        //var service = new JobApplicationService();

        ToggleAllMenus(false);
        jobBoard.SetActive(true);
        ClearChildrenButtons(jobSelectionRoot);
        currentMenuPage = MenuPage.jobSelect;
        mainMenuBacking.gameObject.SetActive(true);
        availbleJobs = await applicationService.GetJobsAsync();
        Debug.Log(availbleJobs.Count);
        foreach (var job in availbleJobs)
        {
            //var curJob = await applicationService.GetJobsAsync(job.Id);
            GameObject jobButton = Instantiate(this.jobButton, jobSelectionRoot);
            //jobButton.transform.localScale = new Vector3(.14f, .14f, .14f);
            JobDisplay jobDisplay = jobButton.GetComponent<JobDisplay>();
            //Interactable jobDisplayInteractable = jobDisplay.GetComponent<Interactable>();
            Button jobDisplayInteractable = jobDisplay.jobButton;

            float tasksDone = 0;

            jobDisplayInteractable.interactable = job.Progress <= 99;
            jobDisplay.UpdateDisplayInformation("Job# " + job.Number,
                $"{job.CarInfo.Manufacturer} {job.CarInfo.Model} {job.CarInfo.Year}",
                job.Progress + "%",
                (float)job.Progress);
            jobDisplayInteractable.onClick.AddListener(AddJobToButton(job));
            
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
    public void AdvanceToTaskList(JobListItemDto chosenJob)
    {
        ToggleAllMenus(false);
        ClearChildrenButtons(taskSelectionRoot);
        taskBoard.SetActive(true);
        mainMenuBacking.gameObject.SetActive(true);
        mainMenuAesthetic.UpdateTaskDisplay(chosenJob);
        currentMenuPage = MenuPage.taskSelect;
        int stepIndex = 0;
        //Debug.Log(chosenJob.tasks.Count);
        foreach (var jobTask in applicationService.Headliner_RepairManual.StepsGroups)
        {
            for (int i = 0; i < jobTask.Steps.Count; i++)
            {
                
                var step = jobTask.Steps[i];
                GameObject newTaskButton = Instantiate(taskButton, taskSelectionRoot);
                //newTaskButton.transform.localScale = new Vector3(.14f, .14f, .14f);
                TaskDisplay taskDisplay = newTaskButton.GetComponent<TaskDisplay>();
                Button taskDisplayInteractable = taskDisplay.taskButton;
                //taskDisplayInteractable.interactable = jobTask.Status != Task.TaskStatus.Completed;
                stepIndex++;
                taskDisplay.UpdateDisplayInformation(step.Id.ToString("D2"), step.Title, false);
                //string curStep = stepIndex.ToString("D2");                
            }

            //taskDisplay.UpdateDisplayInformation(curStep, jobTask.taskTitle, jobTask.isComplete);
        }
    }

    public void AdvanceToJobView()
    {
        ToggleAllMenus(false);
        currentMenuPage = MenuPage.performingJob;
        mainMenuBacking.gameObject.SetActive(false);
        jobQuickMenu.gameObject.SetActive(true);
    }
    public void ReturnToPreviousPage()
    {
        switch (currentMenuPage)
        {
            case MenuPage.jobSelect:
                Debug.Log("Already on job select");
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
}

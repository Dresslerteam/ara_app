using System;
using System.Collections;
using System.Collections.Generic;
using ARA.Frontend;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    public List<Job> availbleJobs;
    
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
        } else {
            _instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        mainMenuAesthetic = GetComponent<MainMenuAesthetic>();
        UpdateJobBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleAllMenus(bool isOn)
    {
        jobBoard.SetActive(isOn);
        taskBoard.SetActive(isOn);
        jobQuickMenu.gameObject.SetActive(isOn);
    }
    
    private List<Job> FetchJobs()
    {
        // See, here we'd be pulling from a data base
        availbleJobs.Clear();
        return availbleJobs = availbleJobs;
    }

    public void UpdateJobBoard()
    {
        ToggleAllMenus(false);
        jobBoard.SetActive(true);
        ClearChildrenButtons(jobSelectionRoot);
        currentMenuPage = MenuPage.jobSelect;
        mainMenuBacking.gameObject.SetActive(true);
        foreach (var job in availbleJobs)
        {
            GameObject jobButton = Instantiate(this.jobButton,jobSelectionRoot);
            //jobButton.transform.localScale = new Vector3(.14f, .14f, .14f);
            JobDisplay jobDisplay = jobButton.GetComponent<JobDisplay>();
            //Interactable jobDisplayInteractable = jobDisplay.GetComponent<Interactable>();
            Button jobDisplayInteractable = jobDisplay.jobButton;
            float tasksDone = 0;
            for (int i = 0; i < job.tasks.Count; i++)
            {
                if (job.tasks[i].isComplete)
                {
                    tasksDone++;
                }
            }

            float completeAmount = (tasksDone / job.tasks.Count)*100f;
            jobDisplayInteractable.interactable = completeAmount <= 100;
            jobDisplay.UpdateDisplayInformation("Job# " + job.jobNumber,
                job.GetVehicleTitleString(),
                (int)completeAmount + "%",
                completeAmount);
            jobDisplayInteractable.onClick.AddListener(AddJobToButton(job));
        }
    }
    private UnityAction AddJobToButton(Job job)
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
    public void AdvanceToTaskList(Job chosenJob)
    {
        ToggleAllMenus(false);
        ClearChildrenButtons(taskSelectionRoot);
        taskBoard.SetActive(true);
        mainMenuBacking.gameObject.SetActive(true);
        mainMenuAesthetic.UpdateTaskDisplay(chosenJob);
        currentMenuPage = MenuPage.taskSelect;
        int stepIndex = 0;
        Debug.Log(chosenJob.tasks.Count);
        foreach (var jobTask in chosenJob.tasks)
        {
            GameObject newTaskButton = Instantiate(taskButton,taskSelectionRoot);
            //newTaskButton.transform.localScale = new Vector3(.14f, .14f, .14f);
            TaskDisplay taskDisplay = newTaskButton.GetComponent<TaskDisplay>();
            Button taskDisplayInteractable = taskDisplay.taskButton;
            taskDisplayInteractable.interactable = !jobTask.isComplete;
            stepIndex++;
            string curStep = stepIndex.ToString("D2");
            taskDisplay.UpdateDisplayInformation(curStep,jobTask.taskTitle,jobTask.isComplete);
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

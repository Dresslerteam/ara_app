using System;
using System.Collections;
using System.Collections.Generic;
using ARA.Frontend;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MainMenuAesthetic))]
public class MainMenuManager : MonoBehaviour
{
    [Header("Menus")] 
    private MainMenuAesthetic mainMenuAesthetic;
    public GameObject jobBoard;

    [SerializeField] private Transform jobSelectionRoot;
    [SerializeField] private Transform taskSelectionRoot;
    public GameObject taskBoard;

    [Header("Button Prefabs")] 
    [SerializeField]
    private GameObject jobButton;
    [SerializeField]
    private GameObject taskButton;
    public List<Job> availbleJobs;

    private enum MenuPage
    {
        jobSelect,
        taskSelect
    }

    private MenuPage currentMenuPage = MenuPage.jobSelect;
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
        foreach (var job in availbleJobs)
        {
            GameObject jobButton = Instantiate(this.jobButton,jobSelectionRoot);
            JobDisplay jobDisplay = jobButton.GetComponent<JobDisplay>();
            Interactable jobDisplayInteractable = jobDisplay.GetComponent<Interactable>();
            float tasksDone = 0;
            for (int i = 0; i < job.tasks.Count; i++)
            {
                if (job.tasks[i].isComplete)
                {
                    tasksDone++;
                }
            }

            float completeAmount = (tasksDone / job.tasks.Count)*100f;
            jobDisplayInteractable.IsEnabled = completeAmount <= 100;
            jobDisplay.UpdateDisplayInformation("Job# " + job.jobNumber,
                job.GetVehicleTitleString(),
                (int)completeAmount + "%",
                completeAmount);
            jobDisplayInteractable.OnClick.AddListener(AddJobToButton(job));
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
        mainMenuAesthetic.UpdateTaskDisplay(chosenJob);
        currentMenuPage = MenuPage.taskSelect;
        int stepIndex = 0;
        Debug.Log(chosenJob.tasks.Count);
        foreach (var jobTask in chosenJob.tasks)
        {
            GameObject newTaskButton = Instantiate(taskButton,taskSelectionRoot);
            TaskDisplay taskDisplay = newTaskButton.GetComponent<TaskDisplay>();
            Interactable taskDisplayInteractable = taskDisplay.GetComponent<Interactable>();
            taskDisplayInteractable.IsEnabled = !jobTask.isComplete;
            stepIndex++;
            string curStep = stepIndex.ToString("D2");
            taskDisplay.UpdateDisplayInformation(curStep,jobTask.taskTitle,jobTask.isComplete);
        }
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

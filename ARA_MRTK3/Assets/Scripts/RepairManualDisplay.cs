using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Ara.Domain.RepairManualManagement;
using Microsoft.MixedReality.Toolkit.UX;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairManualDisplay : MonoBehaviour
{
    private StepsListController Controller;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI completionAmountText;

    public Transform stepGroupParent;
    public ToggleCollection stepToggleCollection;

    private RectTransform rect;
    private BoxCollider collider;
    private RectTransform scrollerViewport;
    private int CompletedTasksCount = 0;
    private int TotalTasksCount = 0;

    [SerializeField] private VisualSwitchController visualController;
    [SerializeField] private ExpandablePressableButton expandable;
    [SerializeField] private PressableButton toggle;
    public bool isOpen = false;
    bool listBuilt = false;
    private RepairManual manual;

    public Action OnClicked;

    private void Start()
    {
        scrollerViewport = Controller.ScrollRect.viewport;

        collider = GetComponent<BoxCollider>();

        rect = GetComponent<RectTransform>();

         toggle.OnClicked.AddListener(()=>OnToggleSet(!isOpen));

        //toggle.onValueChanged.AddListener(OnToggleSet);
    }



    public void Initialize(RepairManual _manual, StepsListController controller)
    {
        Controller = controller;
        manual = _manual;

        stepToggleCollection.Toggles.Clear();
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;

        expandable.expandablePanelHeight = manual.Steps.Count * 96;

        // Clear previous steps
        foreach (Transform child in stepGroupParent)
        {
            Destroy(child.gameObject);
        }
       
        UpdateDisplayInformation();
    }
    public void UpdateDisplayInformation()
    {
        UpdateDisplayInformation(manual);
    }
    public void UpdateDisplayInformation(RepairManual Manuel)
    {
        this.gameObject.name += Manuel.Id;
        titleText.text = Manuel.Name;
        CompletedTasksCount = ComepletedSegments();
        TotalTasksCount = Manuel.Steps.Count;
        completionAmountText.text = $"{CompletedTasksCount.ToString("0")}/{TotalTasksCount.ToString("0")}";
    }

    private int ComepletedSegments()
    {
        int completedStepscount = 0;
        foreach (var step in manual.Steps) completedStepscount += step.IsCompleted ? 1 : 0;
        return completedStepscount;
    }


    public void OnStepChange(bool value)
    {
        CompletedTasksCount += value ? 1 : -1;
        completionAmountText.text = $"{CompletedTasksCount.ToString("0")}/{TotalTasksCount.ToString("0")}";
    }


    private void OnToggleSet(bool value)
    {
        SetOpen(value);
        if(value) OnClicked?.Invoke();

    }
    public void SetOpen(bool _isOpen)
    {
        isOpen = _isOpen;

        visualController?.SetSelectedState(isOpen);

        if (isOpen && !listBuilt) BuildStepItems(null);
        
        expandable?.ToggleExpand(isOpen);

        Controller.UpdateCollidersLate();
    }



    private void BuildStepItems(Action callback)
    {
        listBuilt = true;

        foreach (var step in manual.Steps)
        {
            StepDisplay stepDisplay = Instantiate(Controller.StepDisplayPrefab).GetComponent<StepDisplay>();
            stepDisplay.UpdateDisplayInformation(step.Id, step.Title, step.IsCompleted, stepGroupParent);
            var button = stepDisplay.GetComponent<PressableButton>();
           // stepToggleCollection.Toggles.Add(button);

            stepDisplay.OnStepChange += OnStepChange;

            Controller.StepDisplays.Add(step.Id, stepDisplay);

            button.OnClicked.AddListener(() =>
            {
               Controller.HUD.SetPdfUrl(manual.Document.Url);
               Controller.HUD.SelectStep(step, manual, Controller.HUD.CurrentTask, stepDisplay);
            });
            Controller.SetupStepToggleButton(button,  step);
        }
    }


    public void UpdateColliders()
    {
        if (scrollerViewport == null || collider == null || rect == null) return;

        Vector3[] corners = new Vector3[4];
        scrollerViewport.GetWorldCorners(corners);

        float top = 0;
        float bottom = float.MaxValue;
        foreach (Vector3 corner in corners)
        {
            if (top < corner.y) top = corner.y;
            if (bottom > corner.y) bottom = corner.y;
        }

    
        bool enabled =
            top > (rect.position.y) &&
          bottom < (rect.position.y);
        collider.enabled = enabled;
    }
}

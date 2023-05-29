using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.Common;
using Ara.Domain.JobManagement;
using Ara.Domain.RepairManualManagement;
using Assets.Scripts;
using Assets.Scripts.Common;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UX;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static Tutorial;

public class WorkingHUDManager : MonoBehaviour
{
    //[FormerlySerializedAs("toggleCollection")][SerializeField] private ToggleCollection stepToggleCollection;
    [SerializeField] private ToggleCollection manualButtonCollection;
    [Header("Steps")]
    //[SerializeField] private Transform groupsRoot;
    //[SerializeField][AssetsOnly] private GameObject repairManualDisplayPrefab;
    //[SerializeField][AssetsOnly] private GameObject stepDisplayPrefab;
    [SerializeField][SceneObjectsOnly] private GameObject completedCameraIcon;
    [SerializeField][SceneObjectsOnly] private GameObject uncompletedCameraIcon;
    //private List<RepairManual> repairManuals = new List<RepairManual>();
    [Header("Visuals")]
    [SerializeField] private TextMeshProUGUI taskTextAboveVisuals;

    [SerializeField] private GameObject selectedStepVisualRoot;
    [SerializeField] private RawImage stepImageVisual;
    [SerializeField] private GameObject preStepSelectionVisuals;
    [SerializeField] public GameObject sideTab;
    [Header("Buttons")]
    [SerializeField] private PressableButton procedureButton;
    [SerializeField] private Transform buttonsRoot;
    [SerializeField] private PressableButton completeButton;
    [SerializeField] private PressableButton unCompleteButton;
    [SerializeField] public GameObject photoRequiredModal;
    [SerializeField] public UnCompleteStepConfirmationModal unCompleteStepConfirmationModal;
    [SerializeField][AssetsOnly] private PressableButton cautionPdfButtonPrefab;
    [SerializeField][AssetsOnly] private PressableButton oemPdfButtonPrefab;
    public GameObject takePicture;
    public GameObject CameraSaverBanner;
    private TextMeshProUGUIAutoSizer textMeshProUGUIAutoSizer;
    //private RepairManualDisplay firstRepairManualDisplay;
    public static Action<ManualStep, RepairManual, StepDisplay> OnStepSelected; //todo:tat This is for PhotoCapture Tool reference
   // private List<Toggle> stepGroupButtonParentList = new List<Toggle>();

    public TaskInfo CurrentTask { set; get; }
    //private Dictionary<int, RepairManualDisplay> _repairManualDisplays = new Dictionary<int, RepairManualDisplay>();
    //private Dictionary<int, StepDisplay> _stepDisplays = new Dictionary<int, StepDisplay>();

    public StepsListController StepsListController { get;  set; }

    private string pdf_url;

    public void SetPdfUrl(string url)
    {
        pdf_url = url;
    }

    public void PopulateTaskGroups(TaskInfo task)
    {
        manualButtonCollection.Toggles.Clear();
        foreach (Transform child in buttonsRoot)
        {
            Destroy(child.gameObject);
        }

        StepsListController.PopulateTaskGroups(task);

        procedureButton.OnClicked.AddListener(HandlePDFView);

        preStepSelectionVisuals.SetActive(true);
        selectedStepVisualRoot.SetActive(false);
        sideTab.gameObject.SetActive(false);

    }
  
    private void HandlePDFView()
    {
        MainMenuManager.Instance. pdfsManager.LoadOrHide( pdf_url);
    }
 


    private void UpdateFileButtons(ManualStep step)
    {

        if (step.ReferencedDocs != null && step.ReferencedDocs.Count > 0)
        {
            // If there are buttons, clear them
            foreach (Transform child in buttonsRoot)
            {
                Destroy(child.gameObject);
            }
            manualButtonCollection.Toggles.Clear();
            // Add caution and OEM PDF buttons
            for (int i = 0; i < step.ReferencedDocs.Count; i++)
            {
                var referencedDoc = step.ReferencedDocs[i];

                // If the referencedDocument type is caution, add caution PDF button
                if (referencedDoc.Type == ManualStep.StepReferencedDocType.Caution)
                {
                    var cautionPdfButton = Instantiate(cautionPdfButtonPrefab, buttonsRoot);
                    cautionPdfButton.transform.localScale = Vector3.one;
                    cautionPdfButton.GetComponentInChildren<TextMeshProUGUI>().text = referencedDoc.Doc.Title;
                    SetupPdfToggleButton(cautionPdfButton, manualButtonCollection, referencedDoc.Doc.Url);
                }

                // Add OEM PDF button
                if (referencedDoc.Type == ManualStep.StepReferencedDocType.Procedure)
                {
                    var oemPdfButton = Instantiate(oemPdfButtonPrefab, buttonsRoot);
                    oemPdfButton.transform.localScale = Vector3.one;
                    oemPdfButton.GetComponentInChildren<TextMeshProUGUI>().text = referencedDoc.Doc.Title;
                    SetupPdfToggleButton(oemPdfButton, manualButtonCollection, referencedDoc.Doc.Url);
                }

            }

        }
        else
        {
            // If there are buttons, clear them
            foreach (Transform child in buttonsRoot)
            {
                Destroy(child.gameObject);
            }
            manualButtonCollection.Toggles.Clear();
        }

    }

    private void SetupPdfToggleButton(PressableButton pdfButton, ToggleCollection toggleCollection, string pdfUrl)
    {
       /* toggleCollection.Toggles.Add(pdfButton);
        // If toggle is selected and the pdfButton is toggled, force disable the toggle
        toggleCollection.OnToggleSelected.AddListener((ctx) =>
        {
            if (toggleCollection.Toggles[ctx] != pdfButton)
            {
                pdfButton.ForceSetToggled(false, false);
            }
        });*/
        // Set the toggle mode to toggle
        pdfButton.ForceSetToggled(false);
        pdfButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
        pdfButton.OnClicked.AddListener(() =>
        {
            if (pdfButton.IsToggled == true)
            {
                if (pdfButton.ToggleMode != StatefulInteractable.ToggleType.Toggle)
                    pdfButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
                MainMenuManager.Instance.pdfsManager.LoadPdf(pdfUrl);
                Debug.Log($"{gameObject.name}PDF Loaded");
                pdfButton.ForceSetToggled(true, true);
            }
            else if (pdfButton.IsToggled == false)
            {
                if (pdfButton.ToggleMode != StatefulInteractable.ToggleType.Toggle)
                    pdfButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
                MainMenuManager.Instance.pdfsManager.HidePdf(pdfUrl);
                Debug.Log($"{gameObject.name}PDF Hidden");
                pdfButton.ForceSetToggled(false, true);
            }
        });


    }



    public void UpdateVisual(string stepTitle, string imageURL)
    {
        if (!string.IsNullOrEmpty(imageURL))
        {
            selectedStepVisualRoot.SetActive(true);
            preStepSelectionVisuals.SetActive(false);
            string newString = imageURL;
            // Get image from inside folder
            if (imageURL.EndsWith(".png"))
            {
                newString = imageURL.Substring(0, imageURL.LastIndexOf("."));
            }

            imageURL = newString;
            //Debug.Log("imageURL: " + imageURL);
            Texture2D stepImage = Resources.Load<Texture2D>(imageURL);
            this.stepImageVisual.enabled = true;
            this.stepImageVisual.texture = stepImage;
        }
        else
        {
            //this.stepImageVisual.enabled = false;
        }
        taskTextAboveVisuals.text = stepTitle;
        if (textMeshProUGUIAutoSizer == null)
            textMeshProUGUIAutoSizer = taskTextAboveVisuals.GetComponent<TextMeshProUGUIAutoSizer>();
        textMeshProUGUIAutoSizer.ResizeTextMeshProUGUI();
    }
    // Enable Camera Icon if the step.PhotoRequired is true
    public void EnableCameraIcon(ManualStep step, RepairManual repairManual, StepDisplay stepDisplay)
    {
        // Set the icon
        completedCameraIcon.SetActive(step.PhotoRequired);
        uncompletedCameraIcon.SetActive(step.PhotoRequired);

        if (step.PhotoRequired)
        {
            // Set the complete button to take a picture
            completeButton.OnClicked.RemoveAllListeners();
            // Set the complete button to complete the step
            completeButton.OnClicked.AddListener(() =>
            {
                if (step.PhotoRequired && step.Photos.Count() == 0 && photoRequiredModal != null)
                    photoRequiredModal.SetActive(true);
                else
                {
                    if (step.IsCompleted)
                    {
                        MoveToNextStep(step, repairManual);
                    }
                    else
                    {
                        CompleteStepAndMoveToNext(step, repairManual, stepDisplay);
                    }
                }

            });
        }
        else
        {
            // Set the complete button to take a picture
            completeButton.OnClicked.RemoveAllListeners();
            completeButton.OnClicked.AddListener(() =>
            {
                // Complete step
                CompleteStepAndMoveToNext(step, repairManual, stepDisplay);

            });
        }

        unCompleteButton.OnClicked.RemoveAllListeners();
        unCompleteButton.OnClicked.AddListener(() =>
        {
            if (step.IsCompleted)
            {
                unCompleteStepConfirmationModal.Show(
                    confirmationCallback: () =>
                    {
                        step.IsCompleted = false;
                        unCompleteButton.gameObject.SetActive(false);
                        completeButton.gameObject.SetActive(true);
                        stepDisplay.UnCompleteStep();
                    },

                    cancellationCallback: () =>
                    {
                        unCompleteStepConfirmationModal.Hide();
                    });
            }

        });
    }

    public void CompleteStepAndMoveToNext(ManualStep step, RepairManual repairManual, StepDisplay stepDisplay)
    {
        Debug.Log($"<color=red>CompleteStepAndMoveToNext on Hud Called step:{step.Id}, manual:{repairManual.Id}</color> ");
        MainMenuManager.Instance.currentJob.CompleteStep(MainMenuManager.Instance.selectedTaskInfo.Id,
                            repairManual.Id, step.Id);
        stepDisplay.CompleteStep();
        completeButton.gameObject.SetActive(false);
        unCompleteButton.gameObject.SetActive(true);
        MoveToNextStep(step, repairManual);
    }

    private void MoveToNextStep(ManualStep step, RepairManual repairManual)
    {
        var nextStepObj = MainMenuManager.Instance.currentJob.GetNextStep(CurrentTask.Id, repairManual.Id, step.Id);
        Debug.Log($"<color=blue/>!!!! what we got from GetNextStep repairManual:{nextStepObj.Payload.RepairManual.Id} step:{nextStepObj.Payload.Step.Id}</color>");
        //It only moves to next step if it belongs to same repair manual.
        if (nextStepObj.Status == ResultStatus.Success && nextStepObj.Payload.RepairManual.Id == repairManual.Id)
        {
            var newStepDisplay = StepsListController.StepDisplays[nextStepObj.Payload.Step.Id];
            SelectStep(nextStepObj.Payload.Step, nextStepObj.Payload.RepairManual, CurrentTask, newStepDisplay);
            var stepButton = newStepDisplay.GetComponent<PressableButton>();
            stepButton.ForceSetToggled(true, true);
        }
    }

    public void SelectStep(ManualStep step, RepairManual repairManual, TaskInfo task, StepDisplay stepDisplay)
    {
        var imageURL = "";
        if (step.Image != null && step.Image.Url != null)
            imageURL = "Photos/" + step.Image.Url;
        UpdateVisual(step.Title, imageURL);
        MainMenuManager.Instance.mainMenuAesthetic.UpdateTaskDisplay(
            MainMenuManager.Instance.selectedJobListItem, task, repairManual);
        EnableCameraIcon(step, repairManual, stepDisplay);
        UpdateFileButtons(step);

        preStepSelectionVisuals.SetActive(false);
        selectedStepVisualRoot.SetActive(true);
        sideTab.gameObject.SetActive(true);
        if (step.IsCompleted)
        {
            completeButton.gameObject.SetActive(false);
            unCompleteButton.gameObject.SetActive(true);
        }
        else
        {
            unCompleteButton.gameObject.SetActive(false);
            completeButton.gameObject.SetActive(true);
        }
        OnStepSelected?.Invoke(step, repairManual, stepDisplay);
    }

}
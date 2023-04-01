using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.JobManagement;
using Ara.Domain.RepairManualManagement;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UX;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WorkingHUDManager : MonoBehaviour
    {
        [FormerlySerializedAs("toggleCollection")] [SerializeField] private ToggleCollection stepToggleCollection;
        [SerializeField] private ToggleCollection manualButtonCollection;
        [Header("Steps")]
        [SerializeField] private Transform groupsRoot;
        [SerializeField] [AssetsOnly] private GameObject repairManualDisplayPrefab;
        [SerializeField] [AssetsOnly] private GameObject stepDisplayPrefab;
        [SerializeField] [SceneObjectsOnly] private GameObject cameraIcon;
        private List<RepairManual> repairManuals = new List<RepairManual>();
        [Header("Visuals")] 
        [SerializeField] private TextMeshProUGUI taskTextAboveVisuals;
        [SerializeField] private RawImage stepImageVisual;
        [Header("Buttons")]
        [SerializeField] private PressableButton procedureButton;
        [SerializeField] private Transform buttonsRoot;
        [SerializeField] [AssetsOnly] private PressableButton cautionPdfButtonPrefab;
        [SerializeField] [AssetsOnly] private PressableButton oemPdfButtonPrefab;
        private TextMeshProUGUIAutoSizer textMeshProUGUIAutoSizer;
        private RepairManualDisplay firstRepairManualDisplay;
        public static Action<ManualStep, RepairManual> OnStepSelected;

        public void PopulateTaskGroups(TaskInfo task)
        {
            // Clear previous groups
            for (int i = groupsRoot.childCount - 1; i >= 0; i--)
            {
                GameObject childObject = groupsRoot.GetChild(i).gameObject;
                Destroy(childObject);
            }

            stepToggleCollection.Toggles.Clear();
            repairManuals.Clear();
            firstRepairManualDisplay = null;
            // Populate repair manual
            repairManuals.AddRange(task.RepairManuals);

            foreach (var repairManual in repairManuals)
            {
                // Add repair manual display
                RepairManualDisplay repairManualDisplay = Instantiate(repairManualDisplayPrefab, groupsRoot)
                    .GetComponent<RepairManualDisplay>();
                firstRepairManualDisplay = repairManualDisplay;
                repairManualDisplay.gameObject.name += repairManual.Id;
                repairManualDisplay.UpdateDisplayInformation(repairManual.Name);
                repairManualDisplay.stepToggleCollection.Toggles.Clear();
                repairManualDisplay.transform.localScale = Vector3.one;
                repairManualDisplay.transform.localRotation = Quaternion.identity;
                // Clear previous steps
                foreach (Transform child in repairManualDisplay.stepGroupParent)
                {
                    Destroy(child.gameObject);
                }

                procedureButton.OnClicked.AddListener(() =>
                {
                    if (procedureButton.IsToggled == true)
                    {
                        if (procedureButton.ToggleMode != StatefulInteractable.ToggleType.Toggle)
                            procedureButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
                        MainMenuManager.Instance.pdfLoader.LoadPdf(repairManual.Document.Url);
                        procedureButton.ForceSetToggled(true, true);
                    }
                    else if (procedureButton.IsToggled == false)
                    {
                        if (procedureButton.ToggleMode != StatefulInteractable.ToggleType.Toggle)
                            procedureButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
                        MainMenuManager.Instance.pdfLoader.HidePdf();
                        procedureButton.ForceSetToggled(false, true);
                    }
                });
                // Add steps
                foreach (var step in repairManual.Steps)
                {
                    var stepDisplay = Instantiate(stepDisplayPrefab);
                    stepDisplay.GetComponent<StepDisplay>().UpdateDisplayInformation(step.Id, step.Title,
                        step.IsCompleted, repairManualDisplay.stepGroupParent);
                    var button = stepDisplay.GetComponent<PressableButton>();
                    repairManualDisplay.stepToggleCollection.Toggles.Add(button);
                    button.OnClicked.AddListener(() =>
                    {
                        var imageURL = "";
                        if (step.Image != null && step.Image.Url != null)
                            imageURL = "Photos/" + step.Image.Url;
                        UpdateVisual(step.Title, imageURL);
                        MainMenuManager.Instance.mainMenuAesthetic.UpdateTaskDisplay(
                            MainMenuManager.Instance.selectedJobListItem, task);
                        EnableCameraIcon(step.PhotoRequired);
                        UpdateFileButtons(step);
                        OnStepSelected?.Invoke(step, repairManual);
                    });
                    SetupStepToggleButton(button, stepToggleCollection, step);
                }
            }

        }

        private void UpdateFileButtons(ManualStep step)
        {

            if(step.ReferencedDocs != null && step.ReferencedDocs.Count > 0)
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
                        SetupPdfToggleButton(cautionPdfButton, manualButtonCollection, referencedDoc.Doc.Url);
                    }

                    // Add OEM PDF button
                    if (referencedDoc.Type == ManualStep.StepReferencedDocType.Procedure)
                    {
                        var oemPdfButton = Instantiate(oemPdfButtonPrefab, buttonsRoot);
                        oemPdfButton.transform.localScale = Vector3.one;
                        SetupPdfToggleButton(oemPdfButton, manualButtonCollection, referencedDoc.Doc.Url);
                    }
                    
                }
                
            }
            
        }
        
        private void SetupPdfToggleButton(PressableButton pdfButton, ToggleCollection toggleCollection, string pdfUrl){
            toggleCollection.Toggles.Add(pdfButton);
            // If toggle is selected and the pdfButton is toggled, force disable the toggle
            toggleCollection.OnToggleSelected.AddListener((ctx) =>
            {
                if (toggleCollection.Toggles[ctx]!=pdfButton)
                {
                    pdfButton.ForceSetToggled(false, false);
                }
            });
            // Set the toggle mode to toggle
            pdfButton.ForceSetToggled(false);
            pdfButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
            pdfButton.OnClicked.AddListener(() =>
            {
                if (pdfButton.IsToggled == true)
                {
                    if (pdfButton.ToggleMode != StatefulInteractable.ToggleType.Toggle)
                        pdfButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
                    MainMenuManager.Instance.pdfLoader.LoadPdf(pdfUrl);
                    Debug.Log($"{gameObject.name}PDF Loaded");
                    pdfButton.ForceSetToggled(true, true);
                }
                else if (pdfButton.IsToggled == false)
                {
                    if (pdfButton.ToggleMode != StatefulInteractable.ToggleType.Toggle)
                        pdfButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
                    MainMenuManager.Instance.pdfLoader.HidePdf();
                    Debug.Log($"{gameObject.name}PDF Hidden");
                    pdfButton.ForceSetToggled(false, true);
                }
            });
            
           
        }

        private void SetupStepToggleButton(PressableButton stepButton, ToggleCollection toggleCollection, ManualStep step)
        {
            toggleCollection.Toggles.Add(stepButton);
            // If toggle is selected and the stepButton is toggled, force disable the toggle
            toggleCollection.OnToggleSelected.AddListener((ctx) =>
            {
                if (toggleCollection.Toggles[ctx] != stepButton)
                {
                    stepButton.ForceSetToggled(false, false);
                }
            });
            // Set the toggle mode to toggle
            stepButton.ForceSetToggled(false);
            stepButton.ToggleMode = StatefulInteractable.ToggleType.OneWayToggle;
            stepButton.OnClicked.AddListener(() =>
            {
                if (stepButton.IsToggled == true)
                {
                    if (stepButton.ToggleMode != StatefulInteractable.ToggleType.OneWayToggle)
                        stepButton.ToggleMode = StatefulInteractable.ToggleType.OneWayToggle;
                    stepButton.ForceSetToggled(true, true);
                }
                else if (stepButton.IsToggled == false)
                {
                    return;
                }
            });
        }

        public void UpdateVisual(string stepTitle, string imageURL)
        { 
            if (!string.IsNullOrEmpty(imageURL))
            {
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
             if(textMeshProUGUIAutoSizer==null)
                 textMeshProUGUIAutoSizer = taskTextAboveVisuals.GetComponent<TextMeshProUGUIAutoSizer>();
             textMeshProUGUIAutoSizer.ResizeTextMeshProUGUI();
        }
        // Enable Camera Icon if the step.PhotoRequired is true
        public void EnableCameraIcon(bool enable)
        {
            cameraIcon.SetActive(enable);
        }
    }
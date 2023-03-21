using System.Collections.Generic;
using System.Linq;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.JobManagement;
using Ara.Domain.RepairManualManagement;
using Microsoft.MixedReality.Toolkit.UX;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkingHUDManager : MonoBehaviour
    {
        [SerializeField] private ToggleCollection toggleCollection;
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
        [SerializeField] private Transform buttonsRoot;
        [SerializeField] [AssetsOnly] private PressableButton cautionPdfButtonPrefab;
        [SerializeField] [AssetsOnly] private PressableButton oemPdfButtonPrefab;
        public void PopulateTaskGroups(TaskInfo task)
        {
            // Clear previous groups
            foreach (Transform child in groupsRoot)
            {
                Destroy(child.gameObject);
            }
            // Populate repair manual
            repairManuals.AddRange(task.RepairManuals);

            foreach (var repairManual in repairManuals)
            {
                // Add repair manual display
                RepairManualDisplay repairManualDisplay = Instantiate(repairManualDisplayPrefab, groupsRoot).GetComponent<RepairManualDisplay>();
                repairManualDisplay.UpdateDisplayInformation(repairManual.Name);
                repairManualDisplay.transform.localScale = Vector3.one;
                repairManualDisplay.transform.localRotation = Quaternion.identity;
                // Clear previous steps
                foreach (Transform child in repairManualDisplay.stepGroupParent)
                {
                    Destroy(child.gameObject);
                }
                // Add steps
                foreach (var step in repairManual.Steps)
                {
                    var stepDisplay = Instantiate(stepDisplayPrefab);
                    stepDisplay.GetComponent<StepDisplay>().UpdateDisplayInformation(step.Id, step.Title,step.IsCompleted,repairManualDisplay.stepGroupParent);
                    var button = stepDisplay.GetComponent<PressableButton>();
                    button.OnClicked.AddListener(() =>
                    {
                        var imageURL = "";
                        if(step.Image != null && step.Image.Url != null)
                            imageURL = "Photos/"+step.Image.Url;
                        UpdateVisual(step.Title, imageURL);
                        MainMenuManager.Instance.mainMenuAesthetic.UpdateTaskDisplay(MainMenuManager.Instance.selectedJobListItem, task);
                        EnableCameraIcon(step.PhotoRequired);
                        UpdateFileButtons(step);
                    });
                    toggleCollection.Toggles.Add(button);
                }
            }
            toggleCollection.Toggles[0].ForceSetToggled(true,true);
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
                // Add caution and OEM PDF buttons
                for (int i = 0; i < step.ReferencedDocs.Count; i++)
                {
                    var referencedDoc = step.ReferencedDocs[i].Doc;
        
                    // Add caution PDF button
                    var cautionPdfButton = Instantiate(cautionPdfButtonPrefab, buttonsRoot);
                    cautionPdfButton.transform.localScale = Vector3.one;
                    cautionPdfButton.GetComponent<PressableButton>().OnClicked.AddListener(() =>
                    {
                        MainMenuManager.Instance.pdfLoader.LoadPdf(referencedDoc.Url);
                        Debug.Log(referencedDoc.Url);
                    });

                    // Add OEM PDF button
                    var oemPdfButton = Instantiate(oemPdfButtonPrefab, buttonsRoot);
                    oemPdfButton.transform.localScale = Vector3.one;
                    oemPdfButton.GetComponent<PressableButton>().OnClicked.AddListener(() =>
                    {
                        MainMenuManager.Instance.pdfLoader.LoadPdf(referencedDoc.Url);
                        Debug.Log(referencedDoc.Url);
                    });
                }
                
            }
            
        }

        public void UpdateVisual(string stepTitle, string imageURL)
        {
            if (imageURL != null)
            {
                string newString = imageURL;
                // Get image from inside folder
                if (imageURL.EndsWith(".png"))
                {
                    newString = imageURL.Substring(0, imageURL.LastIndexOf("."));
                }

                imageURL = newString;
                Debug.Log("imageURL: " + imageURL);
                Texture2D stepImage = Resources.Load<Texture2D>(imageURL);
                this.stepImageVisual.enabled = true;
                this.stepImageVisual.texture = stepImage;
            }
            else
            {
                //this.stepImageVisual.enabled = false;
            }
            
            taskTextAboveVisuals.text = stepTitle;
        }
        // Enable Camera Icon if the step.PhotoRequired is true
        public void EnableCameraIcon(bool enable)
        {
            cameraIcon.SetActive(enable);
        }
    }
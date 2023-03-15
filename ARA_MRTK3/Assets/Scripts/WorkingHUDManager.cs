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
        [Header("Steps")]
        [SerializeField] private Transform groupsRoot;
        [SerializeField] [AssetsOnly] private GameObject repairManualDisplayPrefab;
        [SerializeField] [AssetsOnly] private GameObject stepDisplayPrefab;
        [SerializeField] [SceneObjectsOnly] private GameObject cameraIcon;
        private List<RepairManual> repairManuals = new List<RepairManual>();
        [SerializeField] private VerticalFoldoutGroup verticalFoldoutGroup;
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
                    stepDisplay.GetComponent<PressableButton>().OnClicked.AddListener(() =>
                    {
                        var imageURL = "Photos/"+step.Image.Url;
                        UpdateVisual(step.Title, imageURL);
                        EnableCameraIcon(step.PhotoRequired);
                        UpdateFileButtons(step);
                    });
                    
                }
                // Expand the first repair manual
                break;
            }

        }

        private void UpdateFileButtons(ManualStep step)
        {
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

        public void UpdateVisual(string stepTitle, string imageURL)
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
            taskTextAboveVisuals.text = stepTitle;
            this.stepImageVisual.texture = stepImage;
        }
        // Enable Camera Icon if the step.PhotoRequired is true
        public void EnableCameraIcon(bool enable)
        {
            cameraIcon.SetActive(enable);
        }
    }
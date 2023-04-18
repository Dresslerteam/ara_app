using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Ara.Domain.Common;
using Ara.Domain.RepairManualManagement;
//using Packages.Rider.Editor.UnitTesting;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using UnityEngine.Windows.WebCam;
using System.IO;
using static UnityEngine.Windows.WebCam.PhotoCapture;
using Assets.Scripts.Common;

public class PhotoCaptureTool : MonoBehaviour
{
    private PhotoCapture photoCaptureObject;
    private CameraParameters cameraParameters = new CameraParameters();
    private Resolution cameraResolution;
    private string pendingFile;
    [SerializeField]
    [Range(0.01f, 1f)]
    private float photoSizeScaleMultiplier = 1f;
    [FormerlySerializedAs("TakePhotoMenu")]
    [Header("Menus")]
    [SerializeField]
    private GameObject TakePhotoButton;

    [SerializeField] private GameObject photoPreviewMenu;
    [SerializeField] private PhotoGallery photoGallery;
    public List<Texture2D> takenPhotos = new List<Texture2D>();

    [SerializeField] private bool useCustomFilePath = false;

    [ShowIf("useCustomFilePath")]
    [SerializeField]
    [FilePath]
    private string customFilePath;

    public string currentFilePath { get; private set; }

    private static PhotoCaptureTool _instance;
    private bool isTakingPhoto;

    const int defaultWidth = 1136;
    private const int defaultHeight = 640;

    private ManualStep currentStep;
    private RepairManual currentManual;
    private StepDisplay currentStepDisplay;
    private Texture2D _latestPhotoTexture;
    private bool _isPhotoModeActive;

    public PhotoModeTypes CurrentPhotoMode = PhotoModeTypes.JobPhoto;
    public static PhotoCaptureTool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PhotoCaptureTool>();
            }

            return _instance;
        }
    }

    private void OnEnable()
    {
        Debug.Log("<color=green>Photo ENABLE</color>");
        WorkingHUDManager.OnStepSelected -= OnStepSelected;
        WorkingHUDManager.OnStepSelected += OnStepSelected;
    }

    private void OnDisable()
    {
        Debug.Log("PhotoCaptureTool Disabled");

        //WorkingHUDManager.OnStepSelected -= OnStepSelected;
        if (photoCaptureObject != null)
        {
            photoCaptureObject?.Dispose();
            photoCaptureObject = null;
            _isPhotoModeActive = false;
        }
    }

    private void OnStepSelected(ManualStep step, RepairManual repairManual, StepDisplay selectedStepDisplay)
    {
        currentStep = step;
        currentManual = repairManual;
        currentStepDisplay = selectedStepDisplay;
    }

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

    private void Start()
    {
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        DeactivateMenus();
        SetCameraResolution();

        currentFilePath = useCustomFilePath ? customFilePath : Application.persistentDataPath;
        //photoGallery.LoadSavedPictures();
    }

    private void DeactivateMenus()
    {
        if (photoPreviewMenu != null) photoPreviewMenu.SetActive(false);
        if (TakePhotoButton != null) TakePhotoButton.SetActive(false);
    }

    private void SetCameraResolution()
    {
        // HoloLens 2 default resolution


        if (PhotoCapture.SupportedResolutions == null || !PhotoCapture.SupportedResolutions.Any())
        {
            Debug.LogWarning("No supported resolutions found, using HoloLens 2 default resolution");
            cameraResolution.width = (int)(defaultWidth);
            cameraResolution.height = (int)(defaultHeight);
        }
        else
        {
            cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        }

        cameraParameters = new CameraParameters
        {
            hologramOpacity = 0.0f,
            cameraResolutionWidth = (int)defaultWidth,
            cameraResolutionHeight = (int)(defaultHeight),
            pixelFormat = CapturePixelFormat.BGRA32
        };
    }

    public void ActivatePhotoMode()
    {
        MainMenuManager.Instance.SetToPhotoMode();
        if (photoPreviewMenu != null)
            photoPreviewMenu.SetActive(false);
        TakePhotoButton.SetActive(true);
    }

    public void ActivatePhotoModeFromPhotoRequiredModal()
    {
        MainMenuManager.Instance.SetToPhotoMode();
        MainMenuManager.Instance.workingHUDManager.photoRequiredModal.SetActive(false);
        if (photoPreviewMenu != null)
            photoPreviewMenu.SetActive(false);
        TakePhotoButton.SetActive(true);
    }

    public void SnapPhoto()
    {
        if (!isTakingPhoto)
        {
            //StartCoroutine(SnapPhotoCoroutine());
            isTakingPhoto = true;

            if (photoCaptureObject == null)
            {
                Debug.Log("CaptureObject was null");
                PhotoCapture.CreateAsync(true, captureObject =>
                {
                    photoCaptureObject = captureObject;
                    if (!_isPhotoModeActive)
                    {
                        photoCaptureObject.StartPhotoModeAsync(cameraParameters, result =>
                        {
                            _isPhotoModeActive = true;
                            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
                        });
                    }
                    else
                    {
                        photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
                    }
                });
            }
            else
            {
                if (!_isPhotoModeActive)
                {
                    photoCaptureObject.StartPhotoModeAsync(cameraParameters, result =>
                    {
                        _isPhotoModeActive = true;
                        photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
                    });
                }
                else
                {
                    photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
                }
            }
        }
    }


    private void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            Debug.Log("Photo captured to memory!");
            TakePhotoButton.SetActive(false);
            photoPreviewMenu.SetActive(true);
            photoCaptureObject.StopPhotoModeAsync((PhotoCaptureResult result) => { _isPhotoModeActive = false; });
            _latestPhotoTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
            photoCaptureFrame.UploadImageDataToTexture(_latestPhotoTexture);

            photoGallery.DisplayPhotoPreview(_latestPhotoTexture);
            takenPhotos.Add(_latestPhotoTexture);
        }
        else
        {
            Debug.LogError("Failed to capture photo to memory!");
        }

        isTakingPhoto = false;
    }

    public void SaveToDatabase(Ara.Domain.JobManagement.Photo.PhotoLabelType labelType)
    {
        var filename = Ara.Domain.JobManagement.Photo.GenerateUrl("jpg");
        pendingFile = System.IO.Path.Combine(currentFilePath, filename);

        Debug.Log("Saving File Started");
        byte[] jpgBytes = ImageConversion.EncodeToJPG(_latestPhotoTexture);
        if (_latestPhotoTexture == null)
            Debug.Log("_latest Photo Texture is null");
        File.WriteAllBytes(pendingFile, jpgBytes);
        Debug.Log("Saving File Completed");

        MainMenuManager.Instance.currentJob.AssignPhotoToStep((MainMenuManager.Instance.selectedTaskInfo.Id), currentManual.Id,
            currentStep.Id,
            pendingFile,
            labelType);

        DeactivateMenus();
    }

    public void CloseAndComplete()
    {
        MainMenuManager.Instance.headerManager?.cameraHeaderManager?.gameObject?.SetActive(false);
        MainMenuManager.Instance.workingHUDManager.takePicture.SetActive(false);
        MainMenuManager.Instance.workingHUDManager?.CameraSaverBanner?.SetActive(false);
        MainMenuManager.Instance.photoCaptureTool.gameObject.SetActive(false);
        MainMenuManager.Instance.SetWorkingView();
        MainMenuManager.Instance.stepsPage.SetActive(true);
        Debug.Log($"CloseAndComplete {CurrentPhotoMode}");
        if (CurrentPhotoMode == PhotoModeTypes.StepPhoto)
        {
            MainMenuManager.Instance.workingHUDManager.CompleteStepAndMoveToNext(currentStep, currentManual, currentStepDisplay);
            Debug.Log($"CompleteStepAndMoveToNext called step:{currentStep.Id}, manual:{currentManual.Id}");
        }

        Debug.Log($"Close and Complete was called on PhotoCaptureTool");
    }

}
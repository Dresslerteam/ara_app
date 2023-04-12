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
        WorkingHUDManager.OnStepSelected -= OnStepSelected;
        WorkingHUDManager.OnStepSelected += OnStepSelected;
    }

    private void OnDisable()
    {
        Debug.Log("PhotoCaptureTool Disabled");

        WorkingHUDManager.OnStepSelected -= OnStepSelected;
        if (photoCaptureObject != null)
        {
            photoCaptureObject?.Dispose();
            photoCaptureObject = null;
            _isPhotoModeActive = false;
        }
    }

    private void OnStepSelected(ManualStep step, RepairManual repairManual, StepDisplay selectedStepDisplay)
    {
        Debug.Log("<color=green>OnStepSelected</color>");
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

    public void SnapPhoto()
    {
        if (!isTakingPhoto)
        {
            StartCoroutine(SnapPhotoCoroutine());
        }
    }

    private IEnumerator SnapPhotoCoroutine()
    {
        isTakingPhoto = true;

        if (photoCaptureObject == null)
        {
            Debug.Log("CaptureObject was null");
            var createCompletionSource = new TaskCompletionSource<PhotoCapture>();
            PhotoCapture.CreateAsync(true, captureObject =>
            {
                photoCaptureObject = captureObject;
                createCompletionSource.SetResult(captureObject);
            });
            yield return new WaitUntil(() => createCompletionSource.Task.IsCompleted);
        }

        if (!_isPhotoModeActive)
        {
            var startCompletionSource = new TaskCompletionSource<bool>();
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, result =>
                {
                    if (result.success)
                    {
                        startCompletionSource.SetResult(true);
                        _isPhotoModeActive = true;
                    }
                    else
                    {
                        startCompletionSource.SetException(new Exception("Failed to start photo mode"));
                    }
                });
            yield return new WaitUntil(() => startCompletionSource.Task.IsCompleted);
        }

        photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);

        isTakingPhoto = false;
    }

    private void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            Debug.Log("Photo captured to memory!");
            TakePhotoButton.SetActive(false);
            photoPreviewMenu.SetActive(true);
            //photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
            _latestPhotoTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
            photoCaptureFrame.UploadImageDataToTexture(_latestPhotoTexture);

            photoGallery.DisplayPhotoPreview(_latestPhotoTexture);
            takenPhotos.Add(_latestPhotoTexture);
        }
        else
        {
            Debug.LogError("Failed to capture photo to memory!");
        }
    }

    public void SaveToDatabase(Ara.Domain.JobManagement.Photo.PhotoLabelType labelType)
    {
        var filename = Ara.Domain.JobManagement.Photo.GenerateUrl("jpg");
        pendingFile = System.IO.Path.Combine(currentFilePath, filename);

        MainMenuManager.Instance.currentJob.AssignPhotoToStep((MainMenuManager.Instance.selectedTaskInfo.Id), currentManual.Id,
            currentStep.Id,
            pendingFile,
            labelType);
        MainMenuManager.Instance.currentJob.CompleteStep((MainMenuManager.Instance.selectedTaskInfo.Id), currentManual.Id,
            currentStep.Id);
        currentStepDisplay.CompleteStep();

        Debug.Log("Saving File Started");
        byte[] jpgBytes = ImageConversion.EncodeToJPG(_latestPhotoTexture);
        if (_latestPhotoTexture == null)
            Debug.Log("_latest Photo TExture is null");
        File.WriteAllBytes(pendingFile, jpgBytes);
        Debug.Log("Saving File Completed");
        var nextStep = MainMenuManager.Instance.currentJob.GetNextStep(MainMenuManager.Instance.selectedTaskInfo.Id, currentManual.Id, currentStep.Id);

        DeactivateMenus();
    }


    public void DeletePicture()
    {
        if (!string.IsNullOrEmpty(pendingFile))
        {
            Debug.Log("Deleted photo: " + pendingFile);
            takenPhotos.Clear();
            System.IO.File.Delete(pendingFile);
        }

        pendingFile = null;
    }
}
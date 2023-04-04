using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ara.Domain.RepairManualManagement;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using UnityEngine.Windows.WebCam;

public class PhotoCaptureTool : MonoBehaviour
{
    private PhotoCapture photoCaptureObject;
    private CameraParameters cameraParameters = new CameraParameters();
    private Resolution cameraResolution;
    private string pendingFile;
    [SerializeField] [Range(0.01f,1f)] 
    private float photoSizeScaleMultiplier = 1f;
    [FormerlySerializedAs("TakePhotoMenu")] [Header("Menus")] [SerializeField]
    private GameObject TakePhotoButton;

    [SerializeField] private GameObject photoPreviewMenu;
    [SerializeField] private PhotoGallery photoGallery;
    public List<Texture2D> takenPhotos = new List<Texture2D>();

    [SerializeField] private bool useCustomFilePath = false;

    [ShowIf("useCustomFilePath")] [SerializeField] [FilePath]
    private string customFilePath;

    public string currentFilePath { get; private set; }

    private static PhotoCaptureTool _instance;
    private bool isTakingPhoto;
    
    const int defaultWidth = 2272;
    const int defaultHeight = 1278;
    
    private ManualStep currentStep;
    private RepairManual currentManual;
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
        WorkingHUDManager.OnStepSelected += OnStepSelected;
    }

    private void OnDisable()
    {
        WorkingHUDManager.OnStepSelected -= OnStepSelected;
    }

    private void OnStepSelected(ManualStep step, RepairManual repairManual)
    {
        Debug.Log("<color=green>OnStepSelected</color>");
        currentStep = step;
        currentManual = repairManual;
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
            cameraResolution.width = (int)(defaultWidth*photoSizeScaleMultiplier);
            cameraResolution.height = (int) (defaultHeight*photoSizeScaleMultiplier);
        }
        else
        {
            cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        }

        cameraParameters = new CameraParameters
        {
            hologramOpacity = 0.0f,
            cameraResolutionWidth = cameraResolution.width,
            cameraResolutionHeight = cameraResolution.height,
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
            PhotoCapture.CreateAsync(true, captureObject => {
                photoCaptureObject = captureObject;
                createCompletionSource.SetResult(captureObject);
            });
            yield return new WaitUntil(() => createCompletionSource.Task.IsCompleted);
        }

        var startCompletionSource = new TaskCompletionSource<bool>();
        photoCaptureObject.StartPhotoModeAsync(cameraParameters, result => {
            if (result.success)
            {
                startCompletionSource.SetResult(true);
            }
            else
            {
                startCompletionSource.SetException(new Exception("Failed to start photo mode"));
            }
        });
        yield return new WaitUntil(() => startCompletionSource.Task.IsCompleted);

        photoCaptureObject.TakePhotoAsync(OnPhotoModeStarted);

        isTakingPhoto = false;
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame pcf)
    {
        if (!result.success)
        {
            Debug.LogError("Unable to start photo mode!");
            return;
        }

        //string filename = $"CapturedImage {DateTime.Now:MM_dd_yyyy_HH_mm_ss}.png";
        var filename = Ara.Domain.JobManagement.Photo.GenerateUrl("jpg");
        pendingFile = System.IO.Path.Combine(currentFilePath, filename);

        photoCaptureObject.TakePhotoAsync(pendingFile, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
    }

    private void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("Saved Photo to disk!" + pendingFile);
            TakePhotoButton.SetActive(false);
            photoPreviewMenu.SetActive(true);
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
            GenerateTexture();
        }
        else
        {
            Debug.Log("Failed to save Photo to disk");
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }
    }

    private void GenerateTexture()
    {
        Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        byte[] pngBytes = System.IO.File.ReadAllBytes(pendingFile);
        Texture2D tex = new Texture2D(cameraResolution.width, cameraResolution.height);
        tex.LoadImage(pngBytes);

        photoGallery.DisplayPhotoPreview(tex);
    
        takenPhotos.Add(targetTexture);
        
        if (photoGallery != null)
        {
            //string[] photoData = filename.Split(' ');
            //photoGallery.AddPhotoToGallery(targetTexture, photoData[1]);
        }
    }
    
    public void SaveToDatabase(Ara.Domain.JobManagement.Photo.PhotoLabelType labelType)
    {
        Debug.Log("Current Task id: " + MainMenuManager.Instance.selectedTaskInfo.Id);
        Debug.Log("currentStep id: " + currentStep.Id);
        Debug.Log("currentManual id: " + currentManual.Id);
        MainMenuManager.Instance.currentJob.AssignPhotoToStep((MainMenuManager.Instance.selectedTaskInfo.Id), currentManual.Id,
            currentStep.Id, 
            pendingFile, 
            labelType);
        MainMenuManager.Instance.currentJob.CompleteStep((MainMenuManager.Instance.selectedTaskInfo.Id), currentManual.Id,
            currentStep.Id);
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

    private void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
}
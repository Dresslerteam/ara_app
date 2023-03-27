using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using UnityEngine.Windows.WebCam;

public class PhotoCaptureTool : MonoBehaviour
{
    private PhotoCapture photoCaptureObject;
    private CameraParameters cameraParameters = new CameraParameters();
    private Resolution cameraResolution;
    private string pendingFile;
    public delegate void PhotoCaptureCreatedDelegate();
    public event PhotoCaptureCreatedDelegate OnPhotoCaptureCreated;
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
        StartCoroutine(CreatePhotoCaptureObject());
        OnPhotoCaptureCreated += () =>
        {
            Debug.Log("PhotoCaptureObject is ready. You can now take a photo.");
        };
    }

    private IEnumerator CreatePhotoCaptureObject()
    {
        var createCaptureObject = new WaitForEndOfFrame();

        PhotoCapture.CreateAsync(true, captureObject =>
        {
            if (captureObject != null)
            {
                photoCaptureObject = captureObject;
                Debug.Log("PhotoCaptureObject successfully created.");
                OnPhotoCaptureCreated?.Invoke();
            }
            else
            {
                Debug.LogError("Failed to create PhotoCaptureObject.");
            }
        });

        yield return createCaptureObject;
    }
    private void InitializeSettings()
    {
        SetCameraResolution();

        currentFilePath = useCustomFilePath ? customFilePath : Application.persistentDataPath;

        photoGallery.LoadSavedPictures();
    }

    private void SetCameraResolution()
    {
        // HoloLens 2 default resolution
        int defaultWidth = 2272;
        int defaultHeight = 1278;

        if (PhotoCapture.SupportedResolutions == null || !PhotoCapture.SupportedResolutions.Any())
        {
            Debug.LogWarning("No supported resolutions found, using HoloLens 2 default resolution");
            cameraResolution.width = defaultWidth;
            cameraResolution.height = defaultHeight;
        }
        else
        {
            cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height)
                .First();
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
        if (photoPreviewMenu != null) photoPreviewMenu.SetActive(false);
        TakePhotoButton.SetActive(true);
    }

    public void SnapPhoto()
    {
        if (photoCaptureObject == null)
        {
            Debug.LogWarning("CaptureObject is null");
            return;
        }

        photoCaptureObject.StartPhotoModeAsync(cameraParameters, result => {
            if (result.success)
            {
                photoCaptureObject.TakePhotoAsync(pendingFile, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
            }
            else
            {
                Debug.LogError("Unable to start photo mode!");
            }
        });
    }

    private void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("Saved Photo to disk!" + pendingFile);
            TakePhotoButton.SetActive(false);
            photoPreviewMenu.SetActive(true);
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);

            GenerateTexture(pendingFile);
        }
        else
        {
            Debug.Log("Failed to save Photo to disk");
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }
    }

    private void GenerateTexture(string filename)
    {
        Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        byte[] pngBytes = System.IO.File.ReadAllBytes(filename);

        Texture2D tex = new Texture2D(2048, 1024);
        tex.LoadImage(pngBytes);

        photoGallery.DisplayPhotoPreview(tex);

        takenPhotos.Add(targetTexture);

        if (photoGallery != null)
        {
            string[] photoData = filename.Split(' ');
            photoGallery.AddPhotoToGallery(targetTexture, photoData[1]);
        }
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
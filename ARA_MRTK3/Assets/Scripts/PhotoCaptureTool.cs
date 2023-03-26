using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.Windows.WebCam;

public class PhotoCaptureTool : MonoBehaviour
{
    private PhotoCapture photoCaptureObject;
    private CameraParameters cameraParameters = new CameraParameters();
    private Resolution cameraResolution;
    private string pendingFile;

    [Header("Menus")]
    [SerializeField] private GameObject TakePhotoMenu;
    [SerializeField] private GameObject photoPreviewMenu;
    [SerializeField] private PhotoGallery photoGallery;
    public List<Texture2D> takenPhotos = new List<Texture2D>();

    [SerializeField] private bool useCustomFilePath = false;
    [ShowIf("useCustomFilePath")][SerializeField][FilePath] private string customFilePath;
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
    }

    private void InitializeSettings()
    {
        DeactivateMenus();
        SetCameraResolution();

        currentFilePath = useCustomFilePath ? customFilePath : Application.persistentDataPath;

        photoGallery.LoadSavedPictures();
    }

    private void DeactivateMenus()
    {
        if (photoPreviewMenu != null) photoPreviewMenu.SetActive(false);
        if (TakePhotoMenu != null) TakePhotoMenu.SetActive(false);
    }

    private void SetCameraResolution()
    {
        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
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
        TakePhotoMenu.SetActive(true);
    }

    public void SnapPhoto()
    {
        if (photoCaptureObject == null)
        {
            Debug.Log("CaptureObject was null");
            PhotoCapture.CreateAsync(true, captureObject => photoCaptureObject = captureObject);
            return;
        }

        photoCaptureObject.StartPhotoModeAsync(cameraParameters, result => photoCaptureObject.TakePhotoAsync(OnPhotoModeStarted));
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame pcf)
    {
        if (!result.success)
        {
            Debug.LogError("Unable to start photo mode!");
            return;
        }

        string filename = $"CapturedImage {DateTime.Now:MM_dd_yyyy_HH_mm_ss}.png";
        pendingFile = System.IO.Path.Combine(currentFilePath, filename);

        photoCaptureObject.TakePhotoAsync(pendingFile, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
    }

    private void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("Saved Photo to disk!" + pendingFile);
            TakePhotoMenu.SetActive(false);
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

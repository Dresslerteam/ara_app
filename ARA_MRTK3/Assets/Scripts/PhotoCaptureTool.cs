using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.Windows.WebCam;

public class PhotoCaptureTool : MonoBehaviour {
    
    PhotoCapture photoCaptureObject = null;
    CameraParameters cameraParameters = new CameraParameters();
    [Header("Menus")]
    
    [SerializeField] private GameObject TakePhotoMenu = null;
    [SerializeField] private GameObject photoPreviewMenu = null;

    [SerializeField] private PhotoGallery photoGallery;
    public List<Texture2D> takenPhotos = new List<Texture2D>();
    [SerializeField] private bool useCustomFilePath = false;
    [ShowIf("useCustomFilePath")][SerializeField][FilePath]private string customFilePath;
    public string currentFilePath { get; private set; }
    private Resolution cameraResolution;
    private PhotoCaptureFrame photoCaptureFrame;
    private string pendingFile;
    
    
    // Singleton 
    private static PhotoCaptureTool _instance;

    public static PhotoCaptureTool Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    
    private void Start()
    {
        if(photoPreviewMenu!=null)
            photoPreviewMenu.SetActive(false);
        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        
        cameraParameters = new CameraParameters();
        cameraParameters.hologramOpacity = 0.0f;
        cameraParameters.cameraResolutionWidth = cameraResolution.width;
        cameraParameters.cameraResolutionHeight = cameraResolution.height;
        cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

        CreatePhotoObject();
        if (useCustomFilePath)
        {
            currentFilePath = customFilePath;
        }
        else
        {
            currentFilePath = Application.persistentDataPath;
        }
        photoGallery.LoadSavedPictures();
    }

    private void OnEnable()
    {
        if(photoPreviewMenu!=null)
            photoPreviewMenu.SetActive(false);
        if(TakePhotoMenu!=null)
            TakePhotoMenu.SetActive(false);
    }

    public void ActivatePhotoMode()
    {
        if(photoPreviewMenu!=null)
            photoPreviewMenu.SetActive(false);
        TakePhotoMenu.SetActive(true);
    }
    private void CreatePhotoObject()
    {
        // Create a PhotoCapture object
        PhotoCapture.CreateAsync(true, delegate(PhotoCapture captureObject) { photoCaptureObject = captureObject; });
    }

    public void SnapPhoto()
    {
        if (photoCaptureObject == null)
        {
            Debug.Log("CaptureObject was null");
            CreatePhotoObject();
            return;
        }
        // Activate the camera
        photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
            // Take a picture
            photoCaptureObject.TakePhotoAsync(OnPhotoModeStarted);
            //photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToMemory);
        });
    }
    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame pcf)
    {
        if (result.success)
        {
            string filename = "";
            string filePath = "";
            
            filename = string.Format(@"CapturedImage {0}.png", DateTime.Now.Month+"_"+DateTime.Now.Day+"_"+DateTime.Now.Year+"_"+DateTime.Now.Hour+"_"+DateTime.Now.Minute+"_"+DateTime.Now.Second);
            filePath = System.IO.Path.Combine(currentFilePath, filename);
            pendingFile = filePath;
            photoCaptureFrame = pcf;
            photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
        }
        else
        {
            pendingFile = null;
            Debug.LogError("Unable to start photo mode!");
        }
    }



    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("Saved Photo to disk!"+pendingFile);
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
        // Copy the raw image data into our target texture
        
        //photoCaptureFrame.UploadImageDataToTexture(targetTexture);
        
        //Converts desired path into byte array
        byte[] pngBytes = System.IO.File.ReadAllBytes(filename);
 
        //Creates texture and loads byte array data to create image
        Texture2D tex = new Texture2D(2048,1024); //Todo: Way too high (2048x1024 at the time)
        tex.LoadImage(pngBytes);
        
        photoGallery.DisplayPhotoPreview(tex);
        // Do as we wish with the texture such as apply it to a material, etc.
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
            Debug.Log("Deleted photo: "+pendingFile);
            takenPhotos.Clear();
            System.IO.File.Delete(pendingFile);
        }

        pendingFile = null;

    }
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result) {
        // Shutdown the photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
}
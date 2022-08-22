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
    [SerializeField] private PhotoGallery photoGallery;
    public List<Texture2D> takenPhotos = new List<Texture2D>();
    [SerializeField] private bool useCustomFilePath = false;
    [ShowIf("useCustomFilePath")][SerializeField][FilePath]private string customFilePath;
    private string currentFilePath;
    private Resolution cameraResolution;

    private string pendingFile;
    private void Start()
    {
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
        photoGallery.LoadSavedPictures(currentFilePath);
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
    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            // Create our Texture2D for use and set the correct resolution
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
            // Copy the raw image data into our target texture
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);
            // Do as we wish with the texture such as apply it to a material, etc.
            takenPhotos.Add(targetTexture);
            Debug.Log("Got the pic");
        }
        else
        {
            Debug.Log("fail");
        }
        // Clean up
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }
    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            string filename = "";
            string filePath = "";
            
            filename = string.Format(@"CapturedImage {0}.png", DateTime.Now.Month+"_"+DateTime.Now.Day+"_"+DateTime.Now.Year+"_"+DateTime.Now.Hour+"_"+DateTime.Now.Minute);
            filePath = System.IO.Path.Combine(currentFilePath, filename);
            pendingFile = filePath;
            photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
            Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
            // Copy the raw image data into our target texture
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);
            photoGallery.DisplayPhotoPreview(targetTexture);
            // Do as we wish with the texture such as apply it to a material, etc.
            takenPhotos.Add(targetTexture);
            if (photoGallery != null)
            {
                string[] photoData = filename.Split(' ');
                photoGallery.AddPhotoToGallery(targetTexture,photoData[1]);
            }
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
            Debug.Log("Saved Photo to disk!");
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }
        else
        {
            Debug.Log("Failed to save Photo to disk");
        }
    }

    public void DeletePicture()
    {
        if (!string.IsNullOrEmpty(pendingFile))
        {
            System.IO.File.Delete(pendingFile);
        }

        pendingFile = "";
        photoGallery.ClearAllGalleryObjects();
        photoGallery.LoadSavedPictures(currentFilePath);
    }
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result) {
        // Shutdown the photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
}
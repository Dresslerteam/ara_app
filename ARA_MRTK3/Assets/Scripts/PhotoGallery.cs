using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

public class PhotoGallery : MonoBehaviour
{
    [SerializeField] [Required] private Transform gallery;

    [SerializeField] [Required] private GameObject photoGroupPrefab;
    [SerializeField] [Required] private GameObject photoButtonPrefab;
    
    [SerializeField] [Required] private Photo GalleryPreviewPhoto;
    
    private string[] filePaths;
    
    [SerializeField] private GameObject photoPreviewImage;

    [SerializeField] private Vector2 resolution = new Vector2(1024,512);

    private void OnEnable()
    {
        Photo.OnPhotoClicked += DisplayCurrentlySelectedPhoto;
    }

    private void OnDisable()
    {
        Photo.OnPhotoClicked -= DisplayCurrentlySelectedPhoto;
    }

    // Start is called before the first frame update
    void Start()
    {
        ClearAllGalleryObjects();
        
    }

    /// <summary>
    /// Loads saved photos from a specified folder
    /// </summary>
    /// <param name="filePath"></param>
    [Button]
    public void LoadSavedPictures()
    {
        
        ClearAllGalleryObjects();
        filePaths = Directory.GetFiles(PhotoCaptureTool.Instance.currentFilePath, "*.jpg"); // Get all files of type .png in this folder
        if(filePaths.Length<=1)
            return;
        for (int i = 0; i < filePaths.Length; i++)
        {
            //Converts desired path into byte array
            byte[] jpgBytes = System.IO.File.ReadAllBytes(filePaths[i]);
 
            //Creates texture and loads byte array data to create image
            Texture2D tex = new Texture2D((int)resolution.x,(int)resolution.y); //Todo: Way too high (2048x1024 at the time)
            tex.LoadImage(jpgBytes);
            string[] photoData = filePaths[i].Split(' ');
            //AddPhotoToGallery(tex,"Date");
        }
        
    }

    /// <summary>
    /// This is called for when you are taking a picture and want to see the preview. (This shouldn't be in this class.)
    /// </summary>
    /// <param name="texture"></param>
    public void DisplayPhotoPreview(Texture2D texture)
    {
        photoPreviewImage.SetActive(true);
        Photo photo = photoPreviewImage.GetComponent<Photo>();
        photo.image.texture = texture;
    }

    public void DisplayCurrentlySelectedPhoto(Photo photo)
    {
        GalleryPreviewPhoto.image.texture = photo.image.texture;
        GalleryPreviewPhoto.label.text = photo.label.text;
    }
    public void ClearAllGalleryObjects()
    {
        for (int i = 0; i < gallery.childCount; i++)
        {
            GameObject.Destroy(gallery.GetChild(i).gameObject);
        }
    }
    public void AddPhotoToGallery(Texture2D targetTexture, string date)
    {
        GameObject photograph = Instantiate(photoButtonPrefab, gallery);
        Photo data = photograph.GetComponent<Photo>();
        data.image.texture = targetTexture;
        data.label.text = date;
    }
}

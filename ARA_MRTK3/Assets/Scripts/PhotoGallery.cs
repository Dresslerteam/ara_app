using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

public class PhotoGallery : MonoBehaviour
{
    [SerializeField] [Required] private Transform gallery;

    [SerializeField] [Required] private GameObject photoPrefab;
    [SerializeField] [Required] private Photo GalleryPreviewPhoto;
    
    private string[] filePaths;
    
    [SerializeField] private GameObject photoPreviewImage;

    [SerializeField] private Vector2 resolution = new Vector2(2048f,1024f);

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
        filePaths = Directory.GetFiles(PhotoCaptureTool.Instance.currentFilePath, "*.png"); // Get all files of type .png in this folder
        if(filePaths.Length<=1)
            return;
        for (int i = 0; i < filePaths.Length; i++)
        {
            //Converts desired path into byte array
            byte[] pngBytes = System.IO.File.ReadAllBytes(filePaths[i]);
 
            //Creates texture and loads byte array data to create image
            Texture2D tex = new Texture2D((int)resolution.x,(int)resolution.y); //Todo: Way too high (2048x1024 at the time)
            tex.LoadImage(pngBytes);
            string[] photoData = filePaths[i].Split(' ');
            AddPhotoToGallery(tex,photoData[1]);
        }
        
    }

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
        GameObject photograph = Instantiate(photoPrefab, gallery);
        Photo data = photograph.GetComponent<Photo>();
        data.image.texture = targetTexture;
        data.label.text = date;
    }
}

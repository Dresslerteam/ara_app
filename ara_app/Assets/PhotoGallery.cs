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

    private string[] filePaths;
    
    [SerializeField] private GameObject photoPreview;
    // Start is called before the first frame update
    void Start()
    {
        ClearAllGalleryObjects();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Loads saved photos from a specified folder
    /// </summary>
    /// <param name="filePath"></param>
    public void LoadSavedPictures(string folderPath)
    {
        filePaths = Directory.GetFiles(folderPath, "*.png"); // Get all files of type .png in this folder
        if(filePaths.Length<=1)
            return;
        for (int i = 0; i < filePaths.Length; i++)
        {
            //Converts desired path into byte array
            byte[] pngBytes = System.IO.File.ReadAllBytes(filePaths[i]);
 
            //Creates texture and loads byte array data to create image
            Texture2D tex = new Texture2D(2048, 1024);
            tex.LoadImage(pngBytes);
            string[] photoData = filePaths[i].Split(' ');
            AddPhotoToGallery(tex,photoData[1]);
        }
        
    }

    public void DisplayPhotoPreview(Texture2D texture)
    {
        photoPreview.SetActive(true);
        Photo photo = photoPreview.GetComponentInChildren<Photo>();
        photo.image.texture = texture;
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

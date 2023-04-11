using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UX;
using Sirenix.OdinInspector;
using UnityEngine;

public class PhotoGallery : MonoBehaviour
{
    [SerializeField] [Required] private Transform gallery;

    [SerializeField] [Required] private GameObject photoGroupPrefab;
    [SerializeField] [Required] private GameObject photoButtonPrefab;
    
    [SerializeField] [Required] private Photo GalleryPreviewPhoto;
    [SerializeField] [Required] private ToggleCollection photoButtonToggleCollection;
    //private string[] filePaths;
    
    [SerializeField] private GameObject photoPreviewImage;

    [SerializeField] private Vector2 resolution = new Vector2(1024,512);
    [SerializeField] private MetadataDisplay metadataDisplay;

    private void OnEnable()
    {
        Photo.OnPhotoClicked += DisplayCurrentlySelectedPhoto;
        LoadSavedPictures();
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
        photoButtonToggleCollection.Toggles.Clear();
        ClearAllGalleryObjects();
        // GetJobGallery 
        var jobGallery = MainMenuManager.Instance.currentJob.GetJobGallery();
        if(jobGallery.Count<=0)
            return;
        foreach (var job in jobGallery)
        {
            // Iterate through the Photos enumerable and print the details
            foreach (var photo in job.Photos)
            {
                Console.WriteLine($"  Photo created on {photo.CreatedOn}, TaskId: {photo.TaskId}");
                //Converts desired path into byte array
                byte[] jpgBytes = System.IO.File.ReadAllBytes(photo.Url);
                //Creates texture and loads byte array data to create image
                Texture2D tex = new Texture2D((int)resolution.x,(int)resolution.y);
                tex.LoadImage(jpgBytes);
                
                AddPhotoToGallery(tex, photo.CreatedOn.ToString(), photo.Label, photo.TaskName, photo.RepairManualName, photo.StepName);
            }
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
        if(photo.image.texture != null)
            GalleryPreviewPhoto.image.texture = photo.image.texture;
        if(photo.label.text != null)
            GalleryPreviewPhoto.label.text = photo.label.text;
    }
    public void ClearAllGalleryObjects()
    {
        for (int i = 0; i < gallery.childCount; i++)
        {
            GameObject.Destroy(gallery.GetChild(i).gameObject);
        }
    }
    public void AddPhotoToGallery(Texture2D targetTexture, string jobDate, Ara.Domain.JobManagement.Photo.PhotoLabelType labelType, string taskName, string groupName, string stepName)
    {
        GameObject newPhotoButton = Instantiate(photoButtonPrefab, gallery);
        GalleryPhotoButtonDisplay galleryPhotoButton = newPhotoButton.GetComponent<GalleryPhotoButtonDisplay>();
        galleryPhotoButton.image.texture = targetTexture;
        galleryPhotoButton.label.text = jobDate;
        galleryPhotoButton.labelImage.sprite = metadataDisplay.GetLabelSprite(labelType);
        // Get PressableButton 
        var pressableButton = newPhotoButton.GetComponent<PressableButton>();
        photoButtonToggleCollection.Toggles.Add(pressableButton);
        pressableButton.ForceSetToggled(false);
        pressableButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
        pressableButton.OnClicked.AddListener(() =>
        {
            if (pressableButton.IsToggled == true)
            {
                if (pressableButton.ToggleMode != StatefulInteractable.ToggleType.Toggle)
                    pressableButton.ToggleMode = StatefulInteractable.ToggleType.Toggle;
                metadataDisplay.UpdateDisplay(jobDate, labelType, taskName, groupName, stepName);
                DisplayCurrentlySelectedPhoto(galleryPhotoButton);
                pressableButton.ForceSetToggled(true, true);
            }
            else if (pressableButton.IsToggled == false)
            {
                pressableButton.ForceSetToggled(false, true);
                return;
            }
        });
    }
}

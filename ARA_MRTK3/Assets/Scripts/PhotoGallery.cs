using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UX;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class PhotoGallery : MonoBehaviour
{
    [SerializeField][Required] private Transform gallery;

    [SerializeField][Required] private GameObject photoGroupPrefab;
    [SerializeField][Required] private GameObject photoButtonPrefab;
    [SerializeField][Required] private GameObject photoButtonSpacerPrefab;

    [SerializeField][Required] private Photo GalleryPreviewPhoto;
    [SerializeField][Required] private ToggleCollection photoButtonToggleCollection;
    //private string[] filePaths;

    [SerializeField] private GameObject photoPreviewImage;

    [SerializeField] private Vector2 resolution = new Vector2(1024, 512);
    [SerializeField] private MetadataDisplay metadataDisplay;

    private Texture2D _defaultGalleryPreviewPhoto = null;
    private bool debugPhotoMode = false;
    private void OnEnable()
    {
        if (_defaultGalleryPreviewPhoto == null)
        {
            GalleryPreviewPhoto.image.texture = null;
            string assetPath = "CollisionLogo";
            _defaultGalleryPreviewPhoto = Resources.Load<Texture2D>(assetPath);
        }
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
    private Ara.Domain.JobManagement.Photo CopyPhotoData(Ara.Domain.JobManagement.Photo photo)
    {
        Ara.Domain.JobManagement.Photo newPhoto = new Ara.Domain.JobManagement.Photo();
        newPhoto.CreatedOn = photo.CreatedOn;
        newPhoto.RepairManualId = photo.RepairManualId;
        newPhoto.RepairManualName = photo.RepairManualName;
        newPhoto.TaskName = photo.TaskName;
        newPhoto.TaskId = photo.TaskId;
        newPhoto.StepId = photo.StepId;
        newPhoto.StepName = photo.StepName;
        newPhoto.Label = photo.Label;
        newPhoto.Url = photo.Url;
        return newPhoto;

    }
    /// <summary>
    /// Loads saved photos from a specified folder
    /// </summary>
    /// <param name="filePath"></param>
    [Button]
    public void LoadSavedPictures()
    {
        GalleryPreviewPhoto.image.texture = _defaultGalleryPreviewPhoto;
        metadataDisplay.UpdateDisplay("00/00/0000 00:00:00", Ara.Domain.JobManagement.Photo.PhotoLabelType.Other, "[task name]", "[group name]", "[step name]");
        photoButtonToggleCollection.Toggles.Clear();
        ClearAllGalleryObjects();
        // GetJobGallery 
        var jobGallery = MainMenuManager.Instance.currentJob.GetJobGallery();
        if (jobGallery.Count <= 0)
            return;

       var AllPhotos = new List<Ara.Domain.JobManagement.Photo>();

        //for debugging add some more photos
        foreach (var job in jobGallery)
        {
            foreach (Ara.Domain.JobManagement.Photo photo in job.Photos)
            {
                AllPhotos.Add(photo);

                if (debugPhotoMode)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Ara.Domain.JobManagement.Photo nPhoto = CopyPhotoData(photo);
                        nPhoto.CreatedOn = photo.CreatedOn.Subtract(TimeSpan.FromDays(i));
                        AllPhotos.Add(nPhoto);
                    }
                }
            }
        }

        var orderedPhotos = new List<Ara.Domain.JobManagement.Photo>();

        // Iterate through the Photos and place them in order by date
        foreach (var photo in AllPhotos)
        {
            bool placed = false;
            for (int i = 0; i < orderedPhotos.Count; i++)
            {
                if (!placed && photo.CreatedOn < orderedPhotos[i].CreatedOn)
                {
                    orderedPhotos.Insert(i, photo);
                    placed = true;
                }
            }
            if(!placed) orderedPhotos.Add(photo);
        }
        
        orderedPhotos.Reverse();

        bool Today = false;
        bool pastToday = false;
        int days = 1;

        // Iterate through the Photos enumerable and print the details
        for (int i = 0; i < orderedPhotos.Count; i++)
        {
            var photo = orderedPhotos[i];
            Console.WriteLine($"  Photo created on {photo.CreatedOn}, TaskId: {photo.TaskId}");


            if(!Today && photo.CreatedOn > DateTime.Today)
            {
                Today = true;
                GameObject TodaySpacer = Instantiate(photoButtonSpacerPrefab, gallery);
                TodaySpacer.GetComponent<GalleryPhotoButtonDisplay>().label.text = "Today";
            }
            if (!pastToday && photo.CreatedOn < DateTime.Today)
            {
                pastToday = true;
                GameObject YesterDaySpacer = Instantiate(photoButtonSpacerPrefab, gallery);
                YesterDaySpacer.GetComponent<GalleryPhotoButtonDisplay>().label.text = "Yesterday";

            }

            if (photo.CreatedOn < (DateTime.Today - TimeSpan.FromDays(days)))
            {
                days++;
                GameObject newSpacer = Instantiate(photoButtonSpacerPrefab, gallery);
                newSpacer.GetComponent<GalleryPhotoButtonDisplay>().label.text = photo.CreatedOn.Date.ToString("dd MMMM");

            }




            //Converts desired path into byte array
            byte[] jpgBytes = System.IO.File.ReadAllBytes(photo.Url);
            //Creates texture and loads byte array data to create image
            Texture2D tex = new Texture2D((int)resolution.x, (int)resolution.y);
            tex.LoadImage(jpgBytes);

            AddPhotoToGallery(tex, photo.CreatedOn.ToString(), photo.Label, photo.TaskName, photo.RepairManualName, photo.StepName);
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
        if (photo.image.texture != null)
            GalleryPreviewPhoto.image.texture = photo.image.texture;
        if (photo.label.text != null)
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

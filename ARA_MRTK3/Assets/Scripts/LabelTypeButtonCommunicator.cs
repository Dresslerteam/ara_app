using System;
using UnityEngine;


    public class LabelTypeButtonCommunicator : MonoBehaviour
    {
        [SerializeField] private PhotoCaptureTool photoCaptureTool;
        [SerializeField] private Ara.Domain.JobManagement.Photo.PhotoLabelType labelType;
        private void Start()
        {
            if(photoCaptureTool == null)
                photoCaptureTool = FindObjectOfType<PhotoCaptureTool>();
        }
        
        public void SaveWithLabelType()
        {
            photoCaptureTool.SaveToDatabase(labelType);
        }
    }

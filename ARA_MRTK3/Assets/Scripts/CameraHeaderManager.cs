using Assets.Scripts.Common;
using Microsoft.MixedReality.Toolkit.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraHeaderManager : MonoBehaviour
    {
        [Header("Buttons")]
        public PressableButton galleryButton;
        public PressableButton closeAndCompleteButton;
        public PressableButton closeButton;


        public PhotoModeTypes currentPhotoMode;

        public void SetCurrentPhotoMode(PhotoModeTypes photoMode)
        {
            currentPhotoMode = photoMode;
            switch (currentPhotoMode)
            {
                case PhotoModeTypes.StepPhoto:
                    closeButton.enabled = false;
                    galleryButton.enabled = true;
                    closeAndCompleteButton.enabled = true;
                    break;
                case PhotoModeTypes.TaskPhoto:
                case PhotoModeTypes.JobPhoto:
                    closeAndCompleteButton.enabled = false;
                    galleryButton.enabled = true;
                    closeButton.enabled = true;
                    break;
            }
        }
    }
}

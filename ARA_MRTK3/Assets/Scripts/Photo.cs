using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Photo : MonoBehaviour
{
    public RawImage image;
    public TextMeshProUGUI label;
    public static Action<Photo> OnPhotoClicked;
    
    public void UpdateSelectedImage()
    {
        if (OnPhotoClicked != null)
            OnPhotoClicked.Invoke(this);
    }
}
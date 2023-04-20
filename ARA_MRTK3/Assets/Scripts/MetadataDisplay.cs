using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MetadataDisplay : MonoBehaviour
{
    [Header("Date")]
    [SerializeField] private TextMeshProUGUI dateText;
    [Header("Saved Group")]
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private Image labelImage;
    [SerializeField] private Sprite[] labelSprites;
    [Header("Assigned Group")]
    [SerializeField] private TextMeshProUGUI taskName;
    [SerializeField] private TextMeshProUGUI groupName;
    [SerializeField] private TextMeshProUGUI stepName;

    public void UpdateDisplay(string date, Ara.Domain.JobManagement.Photo.PhotoLabelType labelType, string task, string group, string step)
    {
        dateText.text = date;
        labelImage.sprite = GetLabelSprite(labelType);
        
        taskName.text = task;
        groupName.text = group;
        stepName.text = step;
        labelText.text = labelType.ToString();
    }

    public Sprite GetLabelSprite(Ara.Domain.JobManagement.Photo.PhotoLabelType labelType)
    {
        switch (labelType)
        {
            case Ara.Domain.JobManagement.Photo.PhotoLabelType.Reinstall:
                return labelSprites[0];
            case Ara.Domain.JobManagement.Photo.PhotoLabelType.Replace:
                return labelSprites[1];
            case Ara.Domain.JobManagement.Photo.PhotoLabelType.Repair:
                return labelSprites[2];
            case Ara.Domain.JobManagement.Photo.PhotoLabelType.Other:
                return labelSprites[3];
            default:
                Debug.LogError("No label sprite found for label type: " + labelType);
                return null;
        }
    }
}

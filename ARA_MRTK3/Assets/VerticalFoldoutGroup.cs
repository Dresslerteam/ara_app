using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalFoldoutGroup : MonoBehaviour
{
    [SerializeField] [Tooltip("The height of the button that will foldout.")] 
    private float baseVerticalOffset = 96f;

    [SerializeField] private float spacing = 8f;

    private List<ExpandablePressableButton> foldoutElements = new List<ExpandablePressableButton>();

    private void OnEnable()
    {
        UpdateFoldout();
    }
    
    public void UpdateFoldout()
    {
        PopulateFoldoutElementList();
        OffsetElements();
    }

    private void PopulateFoldoutElementList()
    {
        // Loop through each child of this object's transform
        foreach (Transform child in transform)
        {
            // Check if the child has a RectTransform component
            ExpandablePressableButton expandableButton = child.GetComponent<ExpandablePressableButton>();
            if (expandableButton != null)
            {
                // If the child has the component, add it to the list
                foldoutElements.Add(expandableButton);
            }
        }
    }

    private void OffsetElements()
    {
        // Loop through each index of the list
        for (int i = 1; i < foldoutElements.Count; i++)
        {

            LayoutRebuilder.ForceRebuildLayoutImmediate(foldoutElements[i-1].expandablePanel);
            // Get the panel size
            var panelHeight = foldoutElements[i-1].expandablePanel.rect.height;
            // Offset the Y Position of the next button by panelHeight+baseVerticalOffset+spacing
            float yPosition = (-panelHeight - baseVerticalOffset - spacing);
            RectTransform rectTransform = foldoutElements[i].expandablePanel;
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPosition);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

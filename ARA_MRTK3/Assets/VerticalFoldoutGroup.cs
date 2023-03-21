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
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return null;
        PopulateFoldoutElementList();
        OffsetElements();
        ExpandablePressableButton.OnExpand += OnExpandHandler;
    }

    private void OnDisable()
    {
        ExpandablePressableButton.OnExpand -= OnExpandHandler;
    }

    private void OnExpandHandler(bool isExpanded, ExpandablePressableButton expandedButton)
    {
        UpdateFoldout(isExpanded, expandedButton);
    }

    public void UpdateFoldout(bool isExpanded, ExpandablePressableButton expandedButton)
    {
        int expandedButtonIndex = foldoutElements.IndexOf(expandedButton);
        RectTransform expandedPanel = expandedButton.expandablePanel.GetComponent<RectTransform>();
        float panelHeight = expandedPanel.sizeDelta.y;

        for (int i = expandedButtonIndex + 1; i < foldoutElements.Count; i++)
        {
            RectTransform element = foldoutElements[i].GetComponent<RectTransform>();
            Vector2 anchoredPosition = element.anchoredPosition;

            if (isExpanded)
            {
                anchoredPosition.y -= panelHeight + spacing;
            }
            else
            {
                anchoredPosition.y += expandedButton.expandablePanelHeight + spacing;
            }

            element.anchoredPosition = anchoredPosition;
        }
    }

    private void PopulateFoldoutElementList()
    {
        foldoutElements.Clear();
        foreach (Transform child in transform)
        {
            ExpandablePressableButton expandableButton = child.GetComponent<ExpandablePressableButton>();
            if (expandableButton != null)
            {
                foldoutElements.Add(expandableButton);
            }
        }
    }

    private void OffsetElements()
    {
        float currentYOffset = -8f;

        for (int i = 0; i < foldoutElements.Count; i++)
        {
            RectTransform element = foldoutElements[i].GetComponent<RectTransform>();
            Vector2 anchoredPosition = element.anchoredPosition;
            anchoredPosition.y = currentYOffset;
            element.anchoredPosition = anchoredPosition;

            currentYOffset -= baseVerticalOffset + spacing;

            if (foldoutElements[i].IsExpanded())
            {
                RectTransform expandedPanel = foldoutElements[i].expandablePanel.GetComponent<RectTransform>();
                float panelHeight = expandedPanel.sizeDelta.y;
                currentYOffset -= panelHeight;
            }
        }
    }
}
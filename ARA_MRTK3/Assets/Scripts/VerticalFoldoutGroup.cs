using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class VerticalFoldoutGroup : MonoBehaviour
{
    [SerializeField] [Tooltip("The height of the button that will foldout.")]
    private float baseVerticalOffset = 96f;

    [SerializeField] private float spacing = 8f;
     private RectTransform container;

    private List<ExpandablePressableButton> foldoutElements = new List<ExpandablePressableButton>();

    private void OnEnable()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return null;
        container=this.GetComponent<RectTransform>();
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

        float panelHeight = expandedButton.expandablePanelHeight;

        Vector2 size = new Vector2(0, container.rect.height);

        size.y += isExpanded ? panelHeight : -panelHeight;

        container.sizeDelta = size;

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




    public void UpdateFoldoutBACKUP(bool isExpanded, ExpandablePressableButton expandedButton)
    {
        int expandedButtonIndex = foldoutElements.IndexOf(expandedButton);
        //RectTransform expandedPanel = expandedButton.expandablePanel.GetComponent<RectTransform>();
        //float panelHeight = expandedPanel.sizeDelta.y;
        float panelHeight = expandedButton.expandablePanelHeight;
        Debug.Log($"{(isExpanded ? "Open" : "Close")} panel {panelHeight}");
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandablePressableButton : MonoBehaviour
{
    public RectTransform expandablePanel = new RectTransform();

    // Property for the content size fitter
    public ContentSizeFitter contentSizeFitter;
    
    
    public static event Action<bool, ExpandablePressableButton> OnExpand;
    public float expandablePanelHeight;
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    void Awake()
    {
        contentSizeFitter = expandablePanel.GetComponent<ContentSizeFitter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleExpand(bool isExpanded)
    {
        if (contentSizeFitter == null)
            return;

        if (!isExpanded)
        {
            expandablePanelHeight = expandablePanel.sizeDelta.y;
            expandablePanel.sizeDelta = new Vector2(expandablePanel.sizeDelta.x, 0);
        }
        else
        {
            expandablePanel.sizeDelta = new Vector2(expandablePanel.sizeDelta.x, expandablePanelHeight);
        }

        contentSizeFitter.enabled = isExpanded;
        OnExpand?.Invoke(isExpanded, this);
    }
    
    public bool IsExpanded()
    {
        return contentSizeFitter != null && contentSizeFitter.enabled;
    }

    public float GetExpandedHeight()
    {
        return expandablePanel.sizeDelta.y;
    }
}

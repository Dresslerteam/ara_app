using Microsoft.MixedReality.GraphicsTools;
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
    private List<Collider> colliders = new List<Collider>();

    public RectTransform OpenArrowIcon;
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        colliders.Clear();
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
        if(colliders.Count == 0)
            colliders.AddRange(GetComponentsInChildren<Collider>());

        //expandablePanel.sizeDelta = new Vector2(expandablePanel.sizeDelta.x, expandablePanelHeight);

        expandablePanel.gameObject.SetActive(isExpanded);
        foreach (var collider in colliders)
        {
            collider.enabled = isExpanded;
        }
        OpenArrowIcon.localEulerAngles = new Vector3(0, 0, isExpanded ? 180 : 0);
        colliders[0].enabled = true;
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

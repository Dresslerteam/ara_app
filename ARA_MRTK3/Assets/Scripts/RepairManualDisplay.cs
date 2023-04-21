using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

public class RepairManualDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;

    public Transform stepGroupParent;
    public ToggleCollection stepToggleCollection;

    private StepScroller stepScroller;
    private RectTransform rect;
    private BoxCollider collider;

    public void UpdateDisplayInformation(string title)
    {
        titleText.text = title;
    }

    private void Start()
    {
        stepScroller = GetComponentInParent<StepScroller>();
        if (stepScroller != null) stepScroller.OnMove += UpdateVisibility;

        collider = GetComponent<BoxCollider>();
        rect = GetComponent<RectTransform>();
       
    }

    /// <summary>
    /// UpdateVisibility updates visibility if still on panel
    /// TODO add height of children buttons open as well
    /// </summary>
    private void UpdateVisibility()
    {
        if (stepScroller == null || collider == null || rect == null) return;

        bool enabled =
            stepScroller.transform.localPosition.y - (rect.rect.height/3f) < Mathf.Abs(rect.anchoredPosition.y) &&
            stepScroller.transform.localPosition.y + stepScroller.content.rect.height - (rect.rect.height / 3f) > Mathf.Abs(rect.anchoredPosition.y);

        collider.enabled = enabled;
    }
}

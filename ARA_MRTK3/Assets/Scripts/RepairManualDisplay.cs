using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

public class RepairManualDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI completionAmountText;

    public Transform stepGroupParent;
    public ToggleCollection stepToggleCollection;

    private StepScroller stepScroller;
    private RectTransform rect;
    private BoxCollider collider;
    private int CompletedTasksCount = 0;
    private int TotalTasksCount = 0;
    public void UpdateDisplayInformation(string title, int completedTasksCount, int totalTasksCount)
    {
        titleText.text = title;
        CompletedTasksCount=completedTasksCount;
        TotalTasksCount=totalTasksCount;
        completionAmountText.text = $"{completedTasksCount.ToString("0")}/{totalTasksCount.ToString("0")}";
    }

    public void OnStepChange(bool value)
    {

        CompletedTasksCount += value ? 1 : -1;
        completionAmountText.text = $"{CompletedTasksCount.ToString("0")}/{TotalTasksCount.ToString("0")}";

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
    /// </summary>
    public void UpdateVisibility()
    {
        if (stepScroller == null || collider == null || rect == null) return;
        //Debug.Log($"  {stepScroller.transform.localPosition.y}   {rect.rect.height}   {Mathf.Abs(rect.anchoredPosition.y)}");
        bool enabled =
            stepScroller.transform.localPosition.y - (rect.rect.height/3f) < Mathf.Abs(rect.anchoredPosition.y) &&
            stepScroller.transform.localPosition.y + stepScroller.content.rect.height - (rect.rect.height / 3f) > Mathf.Abs(rect.anchoredPosition.y);

        collider.enabled = enabled;
    }
}

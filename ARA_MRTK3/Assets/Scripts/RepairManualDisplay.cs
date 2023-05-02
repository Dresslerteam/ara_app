using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UX;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairManualDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI completionAmountText;

    public Transform stepGroupParent;
    public ToggleCollection stepToggleCollection;

    private StepScroller stepScroller;
    private RectTransform rect;
    private BoxCollider collider;
    private ScrollRect scrollRect;
    private RectTransform scrollerRect;
    private int CompletedTasksCount = 0;
    private int TotalTasksCount = 0;

    [SerializeField] private VisualSwitchController visualController;
    [SerializeField] private ExpandablePressableButton expandable;
    [SerializeField] private PressableButton toggle;
    bool isOpen = false;

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

        scrollRect = GetComponentInParent<ScrollRect>();
        scrollerRect = scrollRect.GetComponent<RectTransform>();
        if (scrollRect != null) scrollRect.onValueChanged.AddListener((Vector2 value) => { UpdateVisibility(); });

        collider = GetComponent<BoxCollider>();
        rect = GetComponent<RectTransform>();

        toggle.OnClicked.AddListener(()=>OnToggleSet(!isOpen));
    }

    private void OnToggleSet(bool value)
    {
        SetOpen(value);

    }
    public void SetOpen(bool _isOpen)
    {
        isOpen = _isOpen;
        visualController?.SetSelectedState(isOpen);
        expandable?.ToggleExpand(isOpen);
    }
    /// <summary>
    /// UpdateVisibility updates visibility if still on panel
    /// </summary>
    public void UpdateVisibility()
    {
        if (stepScroller == null || collider == null || rect == null) return;

        Vector3[] corners = new Vector3[4];
        scrollerRect.GetWorldCorners(corners);

        float top = 0;
        float bottom = float.MaxValue;
        foreach (Vector3 corner in corners)
        {
            if (top < corner.y) top = corner.y;
            if (bottom > corner.y) bottom = corner.y;
        }


        //Debug.Log($"  {stepScroller.transform.localPosition.y}   {rect.rect.height}   {Mathf.Abs(rect.anchoredPosition.y)}");
        bool enabled =
                    // stepScroller.transform.localPosition.y - (rect.rect.height/3f) < Mathf.Abs(rect.anchoredPosition.y) &&
                    //  stepScroller.transform.localPosition.y + stepScroller.content.rect.height - (rect.rect.height / 3f) > Mathf.Abs(rect.anchoredPosition.y);
                    top > (rect.position.y) &&
          bottom < (rect.position.y);



        collider.enabled = enabled;
    }
}

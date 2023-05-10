using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StepDisplay : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI stepIndex;
    [SerializeField] private TextMeshProUGUI stepText;
    [Header("Icons")]
    [SerializeField] private GameObject stepCompleteIcon;

    public Action<bool> OnStepChange;

    private RectTransform rect;
    private BoxCollider collider;
    private RectTransform scrollerRect;
    private ScrollRect _scrollRect;
    private StepScroller _stepScroller;

    private void Start()
    {
        _scrollRect = GetComponentInParent<ScrollRect>();

        scrollerRect = _scrollRect.GetComponent<RectTransform>();

        rect = GetComponent<RectTransform>();

        collider = GetComponent<BoxCollider>();

        _stepScroller = GetComponentInParent<StepScroller>();

        if (_stepScroller != null) _stepScroller.OnMove += UpdateColliders;
        if (_scrollRect != null) _scrollRect.onValueChanged.AddListener((Vector2 value) => { UpdateColliders(); });

    }

    public void UpdateDisplayInformation(int index, string text, bool isComplete, Transform parent)
    {
        stepIndex.text = index.ToString();
        stepText.text = text;
        stepCompleteIcon.SetActive(isComplete);
        this.transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }

    public void CompleteStep()
    {
        stepCompleteIcon.SetActive(true);
        if(OnStepChange!=null)OnStepChange.Invoke(true);
    }

    public void UnCompleteStep()
    {
        stepCompleteIcon.SetActive(false);
        if (OnStepChange != null) OnStepChange.Invoke(false);

    }

    public void UpdateColliders()
    {
        if (scrollerRect == null || collider == null || rect == null) return;

        Vector3[] corners = new Vector3[4];

        scrollerRect.GetWorldCorners(corners);

        float top = 0;

        float bottom = float.MaxValue;

        foreach (Vector3 corner in corners)
        {
            if (top < corner.y) top = corner.y;

            if (bottom > corner.y) bottom = corner.y;
        }


        bool enabled =
            top > (rect.position.y) &&
          bottom < (rect.position.y);

        collider.enabled = enabled;

    }
}

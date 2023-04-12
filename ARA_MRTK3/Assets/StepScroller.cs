using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UX;
using UnityEngine;
using UnityEngine.UI;

public class StepScroller : MonoBehaviour
{
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private RectTransform content;

    private void OnEnable()
    {
        NormalizedScroll(0);
    }

    public void UpdateFromSliderEventData(SliderEventData sliderEventData)
    {
        NormalizedScroll(sliderEventData.NewValue);
    }
    public void NormalizedScroll(float normalizedValue)
    {
        float y = Mathf.Lerp(minY, maxY, normalizedValue);
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, y);
    }
    
    public void SetMaxY(float maxY)
    {
        this.maxY = maxY;
    }
}

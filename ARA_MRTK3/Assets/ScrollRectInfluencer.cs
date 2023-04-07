using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UX;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectInfluencer : MonoBehaviour
{
    public ScrollRect myScrollRect;

    private void OnEnable()
    {
        UpdateScroll(new SliderEventData(0f,0f));
    }

    public void UpdateScroll(SliderEventData sliderEventData)
    {
        float flippedValue = 1 - sliderEventData.NewValue;
        // Set the vertical scrolling position to 0.5f
        myScrollRect.verticalNormalizedPosition = flippedValue;
    }
}

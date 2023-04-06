using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UX;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectInfluencer : MonoBehaviour
{
    public ScrollRect myScrollRect;

    public void UpdateScroll(SliderEventData sliderEventData)
    {
        
        // Set the vertical scrolling position to 0.5f
        myScrollRect.verticalNormalizedPosition = -sliderEventData.NewValue;;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SplashSkipper : MonoBehaviour
{
    public Slider slider;
    public float timeToLerp = 3;
    public UnityEvent onSliderValueReached;

    private void Start()
    {
        slider.value = 0;
        SkipSplash();
    }

    // Lerp slider value to 1 over 3 seconds and then trigger Unity Event
    public void SkipSplash()
    {
        StartCoroutine(LerpSliderValue());
    }
    public IEnumerator LerpSliderValue()
    {
        float time = 0;
        while (time < timeToLerp)
        {
            time += Time.deltaTime;
            slider.value = Mathf.Lerp(0, 100, time / timeToLerp);
            yield return null;
        }
        onSliderValueReached.Invoke();
    }
}

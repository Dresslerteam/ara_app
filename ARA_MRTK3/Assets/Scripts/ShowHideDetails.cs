using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideDetails : MonoBehaviour
{
    public float lerpTime = .5f;
    public float showSize = 0f;
    public float hideSize = 718f;
    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void ShowDetails()
    {
        StartCoroutine(LerpingSize(showSize));
    }
    public void HideDetails()
    {
        StartCoroutine(LerpingSize(hideSize));
    }
    // Lerp the rect of the object to 0 over 1 second
    public IEnumerator LerpingSize(float toSize)
    {
        float elapsedTime = 0;
        while (elapsedTime < lerpTime)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Mathf.Lerp(0, toSize, elapsedTime / lerpTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShowHideDetails : MonoBehaviour
{
    public float lerpTime = .5f;
    public float showSize = 0f;
    public float hideSize = 718f;
    private RectTransform rectTransform;
    
    public UnityEvent onShow;
    public UnityEvent onHide;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void ShowDetails()
    {
        StartCoroutine(LerpingSize(showSize));
        onShow.Invoke();
    }
    public void HideDetails()
    {
        StartCoroutine(LerpingSize(hideSize));
        onHide.Invoke();
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

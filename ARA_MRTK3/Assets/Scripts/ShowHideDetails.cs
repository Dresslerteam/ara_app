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
    private bool isMoving = false;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void ShowDetails()
    {
        if(isMoving) return;
        StartCoroutine(LerpingSize(showSize));
        onShow.Invoke();
    }
    public void HideDetails()
    {
        if(isMoving) return;
        StartCoroutine(LerpingSize(hideSize));
        onHide.Invoke();
    }
    // Lerp the rect of the object to target size
    public IEnumerator LerpingSize(float targetSize)
    {
        float initialSize = rectTransform.sizeDelta.y;
        float sizeDifference = Mathf.Abs(targetSize - initialSize);
        if (sizeDifference <= 0) yield break;

        while (Mathf.Abs(rectTransform.sizeDelta.y - targetSize) > 0.01f)
        {
            isMoving = true;
            float newSize = Mathf.MoveTowards(rectTransform.sizeDelta.y, targetSize, sizeDifference * Time.deltaTime / lerpTime);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newSize);
            yield return null;
        }

        // Ensure the final size is set correctly
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, targetSize);
        isMoving = false;
    }
    
}

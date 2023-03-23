using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class RawImageAutoResize : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform titleTextRectTransform;

    private RawImage rawImage;
    private RectTransform rectTransform;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        ResizeRawImage();
    }

    private void ResizeRawImage()
    {
        if (rawImage.texture == null)
            return;

        float imageAspectRatio = (float)rawImage.texture.width / rawImage.texture.height;
        float containerHeight = container.rect.height;
        float containerWidth = container.rect.width;

        float availableHeight = containerHeight - titleTextRectTransform.rect.height;
        float newWidth = availableHeight * imageAspectRatio;
        float newHeight = availableHeight;

        // Apply constraints
        if (newWidth > containerWidth)
        {
            newWidth = containerWidth;
            newHeight = containerWidth / imageAspectRatio;
        }

        rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class MaterialSwitch : MonoBehaviour
{
    [SerializeField] private Material highlightedMaterial;
    [SerializeField] private Material notHighlightedMaterial;
    [SerializeField] private Transform imageToChange;
    
    private Image image;
    private RawImage rawImage;

    void Awake()
    {
        image = imageToChange.GetComponent<Image>();
        rawImage = imageToChange.GetComponent<RawImage>();
    }

    public void UpdateMaterial(bool isHighlighted)
    {
        Material newMaterial = isHighlighted ? highlightedMaterial : notHighlightedMaterial;

        if (image != null)
        {
            image.material = newMaterial;
        }
        else if (rawImage != null)
        {
            rawImage.material = newMaterial;
        }
    }
}
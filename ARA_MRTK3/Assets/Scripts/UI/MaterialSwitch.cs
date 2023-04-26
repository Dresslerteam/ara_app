using Microsoft.MixedReality.GraphicsTools;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSwitch : MonoBehaviour
{
    [SerializeField] private Material selectedMaterial;

    [SerializeField] private Material highlightedMaterial;
    [SerializeField] private Material notHighlightedMaterial;
    [SerializeField] private Transform imageToChange;
    
    private Image image;
    private RawImage rawImage;
    private CanvasElementRoundedRect canvasElementRoundedRect;

    void Awake()
    {
        image = imageToChange.GetComponent<Image>();
        rawImage = imageToChange.GetComponent<RawImage>();
        canvasElementRoundedRect = imageToChange.GetComponent<CanvasElementRoundedRect>();
    }

    public void UpdateMaterial(bool isHighlighted)
    {
        UpdateMaterial(isHighlighted ? VisualInteractionState.Hover: VisualInteractionState.Default);
    }


    public void UpdateMaterial(VisualInteractionState state)
    {
        Material newMaterial = notHighlightedMaterial;
        if (state == VisualInteractionState.Hover)
        {
             newMaterial =  highlightedMaterial;
        }
        else if(state == VisualInteractionState.Clicked)
        {
            newMaterial = selectedMaterial;
        }

        if (image != null)
        {
            image.material = newMaterial;
        }
        else if (rawImage != null)
        {
            rawImage.material = newMaterial;
        }
        else if (canvasElementRoundedRect != null)
        {
            canvasElementRoundedRect.material = newMaterial;
        }




    }

}
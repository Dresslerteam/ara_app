using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorSwitch : UIInteractionSwitcher
{
    public enum GatheringType
    {
        Listed,
        FindInChildren,
        OnObject
    }

    public GatheringType switchType = GatheringType.FindInChildren;

    [SerializeField] private List<Image> images = new List<Image>();
    [SerializeField] private List<RawImage> rawImages = new List<RawImage>();


     List<Color> imagesDefaultColors = new List<Color>();

     List<Color> rawImagesDefaultColors = new List<Color>();


    public Color HighlightColor = Color.white;
    public Color SelectedColor = Color.white;

    // Start is called before the first frame update
    void Awake()
    {
        if (switchType == GatheringType.FindInChildren)
        {
            images = GetComponentsInChildren<Image>().ToList();
            rawImages = GetComponentsInChildren<RawImage>().ToList();
        }
        else if (switchType == GatheringType.OnObject)
        {
            images.Add(GetComponent<Image>());
            rawImages.Add(GetComponent<RawImage>());
        }

        StoreDefaults();


    }
    void StoreDefaults()
    {
        for (int i = 0; i < images.Count; i++)
        {
            imagesDefaultColors.Add(images[i].color);
        }
        for (int i = 0; i < rawImages.Count; i++)
        {
            rawImagesDefaultColors.Add(rawImages[i].color);
        }
    }


    public override void UpdateState(VisualInteractionState state)
    {
        switch (state)
        {
            case VisualInteractionState.Hover:
                foreach (Image t in images) t.color = HighlightColor;
                foreach (RawImage t in rawImages) t.color = HighlightColor;
                break;
            case VisualInteractionState.Clicked:
                foreach (Image t in images) t.color = SelectedColor;
                foreach (RawImage t in rawImages) t.color = SelectedColor;
                break;
            case VisualInteractionState.Default:
                for (int i = 0; i < images.Count; i++) images[i].color = imagesDefaultColors[i];
                for (int i = 0; i < rawImages.Count; i++) rawImages[i].color = rawImagesDefaultColors[i];
                break;
        }
    }
}

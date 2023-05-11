using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTextureSwitcher : UIInteractionSwitcher
{

    [SerializeField] private Image image;

    [SerializeField] private Sprite HoverSprite;

    [SerializeField] private Sprite SelectedSprite;

    private Sprite defaultSprite;

    public override void UpdateState(VisualInteractionState state)
    {
        if (image == null) return;

        switch (state)
        {
            case VisualInteractionState.Hover:
                image.sprite = HoverSprite;
                break;
            case VisualInteractionState.Clicked:
                image.sprite = SelectedSprite;

                break;
            case VisualInteractionState.Default:
                image.sprite = defaultSprite;

                break;
        }
    }
  

    // Start is called before the first frame update
    void Start()
    {
        if(image == null) image = GetComponent<Image>();
        if(image != null) defaultSprite = image.sprite;
    }

  

}

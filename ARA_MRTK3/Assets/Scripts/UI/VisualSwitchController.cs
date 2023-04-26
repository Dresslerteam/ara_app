using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSwitchController : MonoBehaviour
{
    [SerializeField] private List<FontColorSwitch> fonts = new List<FontColorSwitch>();
    [SerializeField] private List<MaterialSwitch> materials = new List<MaterialSwitch>();
    [SerializeField] private List<ImageColorSwitch> images = new List<ImageColorSwitch>();
    bool selected = false;
    private void Awake()
    {
        if(fonts.Count ==0 && materials.Count ==0 && images.Count == 0)
        {
            fonts.Add(GetComponent<FontColorSwitch>());
            materials.Add(GetComponent<MaterialSwitch>());
            images.Add(GetComponent<ImageColorSwitch>());
        }
    }

    public void UpdateState(bool isHighlighted)
    {
        UpdateState(isHighlighted ? VisualInteractionState.Hover : VisualInteractionState.Default);
    }
    public void UpdateState(VisualInteractionState state)
    {

        state = selected ? VisualInteractionState.Clicked : state;

        foreach (FontColorSwitch f in fonts) f.SetHighlitState(state);
        foreach (MaterialSwitch m in materials) m.UpdateMaterial(state);
        foreach (ImageColorSwitch m in images) m.SetHighlitState(state);
    }
    public void SetSelectedState(bool isSelected)
    {
        selected = isSelected;
        foreach (FontColorSwitch f in fonts) f.SetHighlitState(isSelected ? VisualInteractionState.Clicked : VisualInteractionState.Default);
        foreach (MaterialSwitch m in materials) m.UpdateMaterial(isSelected ? VisualInteractionState.Clicked : VisualInteractionState.Default);
        foreach (ImageColorSwitch m in images) m.SetHighlitState(isSelected ? VisualInteractionState.Clicked : VisualInteractionState.Default);
    }
}

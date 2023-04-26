using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSwitchController : MonoBehaviour
{
    [SerializeField] private List<FontColorSwitch> fonts = new List<FontColorSwitch>();
    [SerializeField] private List<MaterialSwitch> materials = new List<MaterialSwitch>();
    [SerializeField] private List<ImageColorSwitch> images = new List<ImageColorSwitch>();


    public void UpdateState(bool isHighlighted)
    {
        foreach (FontColorSwitch f in fonts) f.SetHighlitState(isHighlighted);
        foreach (MaterialSwitch m in materials) m.UpdateMaterial(isHighlighted);
        foreach (ImageColorSwitch m in images) m.SetHighlitState(isHighlighted);
    }


    }

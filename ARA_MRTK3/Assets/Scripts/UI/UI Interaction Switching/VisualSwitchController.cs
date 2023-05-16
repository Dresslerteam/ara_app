using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSwitchController : MonoBehaviour
{
    [SerializeField] private List<FontColorSwitch> fonts = new List<FontColorSwitch>();
    [SerializeField] private List<MaterialSwitch> materials = new List<MaterialSwitch>();
    [SerializeField] private List<ImageColorSwitch> images = new List<ImageColorSwitch>();
    [SerializeField] private List<UIInteractionSwitcher> switches = new List<UIInteractionSwitcher>();
    bool selected = false;

    public bool debug = false;
    private Action OnChange;
    private void Awake()
    {
       OnChange += () => { Debug.Log($"OnChange {selected}"); }; 
    }

    public void UpdateState(bool isHighlighted)
    {
        UpdateState(isHighlighted ? VisualInteractionState.Hover : VisualInteractionState.Default);
    }
    public void UpdateState(VisualInteractionState state)
    {

        state = selected ? VisualInteractionState.Clicked : state;
        SetSwitches(state);


    }
    public void SetSelectedState(bool isSelected)
    {
        selected = isSelected;
        SetSwitches(isSelected ? VisualInteractionState.Clicked : VisualInteractionState.Default);

    }
    public void ToggleSelected()
    {
        selected = !selected;
        SetSwitches(selected ? VisualInteractionState.Clicked : VisualInteractionState.Default);

    }

    private void SetSwitches(VisualInteractionState state)
    {
        if (debug) OnChange.Invoke();
        foreach (FontColorSwitch f in fonts) f.UpdateState(state);
        foreach (MaterialSwitch m in materials) m.UpdateState(state);
        foreach (ImageColorSwitch m in images) m.UpdateState(state);
        foreach (UIInteractionSwitcher m in switches) m.UpdateState(state);
    }
}

public interface ISwitchHandler
{
    public void UpdateState(VisualInteractionState state);
}
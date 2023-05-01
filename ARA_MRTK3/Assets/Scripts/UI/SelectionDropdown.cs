using Microsoft.MixedReality.Toolkit.UX;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SelectionDropdown : MonoBehaviour
{

    private int CurrentSelected = 0;
    [SerializeField] private GameObject OptionPrefab; 
    [SerializeField] private GameObject Container;

    public List<string> Options = new List<string>();
    
    private List<GameObject> optionObjects = new List<GameObject>();

    public Action<string> OnOptionSelected;
    

    
    public void Build(List<string> options)
    {
        int count = 0;
        foreach (string s in options)
        {
            GameObject go = Instantiate(OptionPrefab, Container.transform);
            string displayString = StringUtil.AddSpace(s).ToUpper();
            go.GetComponentInChildren<TextMeshProUGUI>().text = displayString;
            int selected = count;

            go.GetComponentInChildren<PressableButton>().OnClicked.AddListener(()=> {
               // Debug.Log($"Selection {selected} {displayString}");
                go.GetComponent<VisualSwitchController>().UpdateState(VisualInteractionState.Clicked);
                CurrentSelected = selected;
                HandleOnOptionSelected(displayString); 
            });
            count++;

            optionObjects.Add(go);
            Options.Add(displayString);

        }
        CurrentSelected = 0;

        optionObjects[CurrentSelected].GetComponent<VisualSwitchController>().SetSelectedState(true);

        SetOpen(false);
    }

    public void SetOpen(bool isOpen)
    {
        Container.SetActive(isOpen);
        if(isOpen && optionObjects.Count > 0)    optionObjects[CurrentSelected].GetComponent<VisualSwitchController>().UpdateState(VisualInteractionState.Clicked);
    }
  

    public void HandleOnOptionSelected(int option)
    {
        HandleOnOptionSelected(Options[option]);
    }
    public void HandleOnOptionSelected(string option)
    {
        foreach (GameObject obj in optionObjects)
        {
            obj.GetComponent<VisualSwitchController>().SetSelectedState(false);

        }
        CurrentSelected = Options.IndexOf(option);

        optionObjects[CurrentSelected].GetComponent<VisualSwitchController>().SetSelectedState(true);

        OnOptionSelected.Invoke(option);

        SetOpen(false);
    }

}

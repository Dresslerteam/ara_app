using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StepDisplay : MonoBehaviour
{
    [Header("Texts")]   
    [SerializeField] private TextMeshProUGUI stepIndex;
    [SerializeField] private TextMeshProUGUI stepText;
    [Header("Icons")]
    [SerializeField] private GameObject stepCompleteIcon;

    public void UpdateDisplayInformation(int index, string text, bool isComplete, Transform parent)
    {
        stepIndex.text = index.ToString();
        stepText.text = text;
        stepCompleteIcon.SetActive(isComplete);
        this.transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }
    
}

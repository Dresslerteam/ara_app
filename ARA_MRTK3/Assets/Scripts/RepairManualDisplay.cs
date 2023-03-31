using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

public class RepairManualDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;

    public Transform stepGroupParent;
    public ToggleCollection stepToggleCollection;
    public void UpdateDisplayInformation(string title)
    {
        titleText.text = title;
    }
}

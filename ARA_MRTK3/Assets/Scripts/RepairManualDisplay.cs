using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepairManualDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;

    public Transform stepGroupParent;
    public void UpdateDisplayInformation(string title)
    {
        titleText.text = title;
    }
}

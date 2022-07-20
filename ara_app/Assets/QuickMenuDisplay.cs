using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickMenuDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI vehicleTitleUi;

    [SerializeField] private TextMeshProUGUI vinUi;

    private void OnEnable()
    {
        
    }

    public void UpdateTopBanner(string vehicle, string vin)
    {
        vehicleTitleUi.text = vehicle;
        vinUi.text = vin;
    }
}

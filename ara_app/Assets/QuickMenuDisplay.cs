using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickMenuDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI vehicleTitleUi;

    [SerializeField] private TextMeshProUGUI vinUi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTopBanner(string vehicle, string vin)
    {
        vehicleTitleUi.text = vehicle;
        vinUi.text = vin;
    }
}

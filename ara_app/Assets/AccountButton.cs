using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountButton : MonoBehaviour
{
    public TextMeshProUGUI nameTextfield;
    public RawImage employeePhoto;

    public void SetupButton(string employeeName, Texture photo)
    {
        nameTextfield.text = employeeName;
        employeePhoto.texture = photo;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountButton : MonoBehaviour
{
    public TextMeshProUGUI nameTextField;
    public RawImage employeePhoto;

    public void SetupButton(string employeeName)
    {
        nameTextField.text = employeeName;
    }
}

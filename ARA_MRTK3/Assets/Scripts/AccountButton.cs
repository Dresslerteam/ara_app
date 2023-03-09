using System.Collections;
using System.Collections.Generic;
using Ara.Domain.ApiClients.Dtos;
using Microsoft.MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountButton : MonoBehaviour
{
    public TextMeshProUGUI nameTextField;
    //public RawImage employeePhoto;
    private PressableButton button;
    public void SetupButton(UserDto userDto)
    {
        button = GetComponent<PressableButton>();
        nameTextField.text = userDto.FirstName + " " + userDto.LastName;
        // Add Listener to button
        button.OnClicked.AddListener(() => LogInMenu._instance.SetEstimatorName(userDto.FirstName + " " + userDto.LastName));
        button.OnClicked.AddListener(() => LogInMenu._instance.AccountSelected(userDto));
    }
}

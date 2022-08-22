using System;
using System.Collections;
using System.Collections.Generic;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApplicationServices;
using UnityEngine;

public class LogInMenu : MonoBehaviour
{
    [SerializeField] private List<UserDto> employees = new List<UserDto>();
    [SerializeField] private GameObject accountButtonPrefab;
    [SerializeField] private Transform contentRoot;
    private void Start()
    {
        PopulateEmployeeGrid();
    }

    public void PopulateEmployeeGrid()
    {
        foreach (var employee in employees)
        {
            AccountButton employeeButton = Instantiate(accountButtonPrefab, contentRoot).GetComponent<AccountButton>();
            employeeButton.SetupButton(employee.FirstName + " " + employee.LastName, null);
        }
    }
}

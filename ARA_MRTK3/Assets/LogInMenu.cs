using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ara.Domain.ApiClients.Dtos;
using Ara.Domain.ApplicationServices;
using Microsoft.MixedReality.Toolkit.UX;
using Sirenix.OdinInspector;
using UnityEngine;

public class LogInMenu : MonoBehaviour
{
    [field: SerializeField] private List<UserDto> employees = new List<UserDto>();
    [SerializeField] private GameObject accountButtonPrefab;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private List<Employee> tempEmployeeList = new List<Employee>();

    [Header("Temp_Menus")]
    [SerializeField] private GameObject jobMenu;
    // Temporary
    public string estimatorName;
    
    [Serializable]
    public class Employee
    {
        public string FirstName;
        public string LastName;
        public Employee(string aFirstName, string aLastName)
        {
            FirstName = aFirstName;
            LastName = aLastName;
        }
        
    }

    private void OnEnable()
    {
        MainMenuManager.Instance.currentMenuPage = MenuPage.loginScreen;
    }

    private void Start()
    {
        //PopulateEmployeeGrid();
    }
    [Button]
    public void PopulateEmployeeGrid()
    {
        foreach (var employee in tempEmployeeList)
        {
            AccountButton employeeButton = Instantiate(accountButtonPrefab, contentRoot).GetComponent<AccountButton>();
            employeeButton.SetupButton(employee.FirstName + " " + employee.LastName);
        }
    }

    public async void AccountSelected()
    {
        await InitJobMenu();
    }

    private async Task InitJobMenu()
    {
        jobMenu.SetActive(true);
        this.gameObject.SetActive(false);
        await MainMenuManager.Instance.LoggedIn();
    }
    public void SetEstimatorName(string name)
    {
        estimatorName = "Estimator:" + name;
    }
}

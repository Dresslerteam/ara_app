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
    [AssetsOnly]
    [SerializeField] private GameObject accountButtonPrefab;
    [SerializeField] private Transform contentRoot;
    private UserService userService = new UserService();
    private List<UserDto> users = new List<UserDto>();
    [Header("Temp_Menus")]
    [SerializeField] private GameObject jobMenu;
    // Temporary
    public string estimatorName;
    
    public static Action OnAccountSelected;
    public static LogInMenu _instance;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnEnable()
    {
        MainMenuManager.Instance.currentMenuPage = MenuPage.loginScreen;
    }

    private void Start()
    {
        //PopulateEmployeeGrid();
        users = userService.GetAllUsers();
        
        if(users.Count > 0)
            PopulateEmployeeGrid();
    }

    private void PopulateEmployeeGrid()
    {
        foreach (var userDto in users)
        {
            AccountButton employeeButton = Instantiate(accountButtonPrefab,
                    contentRoot)
                .GetComponent<AccountButton>();
            employeeButton.SetupButton(userDto);
            
        }
    }

    public async void AccountSelected(UserDto selectedUser)
    {
        userService.Login(selectedUser.Email);
        await LoaderDisplay.Instance.ShowLoader(2f);
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
        
        estimatorName = "Estimator: " + name;
        OnAccountSelected?.Invoke();
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDropdown : MonoBehaviour
{
    public Button dropdownButton;
    public List<Button> buttonList;

    private bool isDropdownOpen = false;
    private RectTransform dropdownPanelTransform;
    private RectTransform dropdownButtonTransform;

    void Start()
    {
        dropdownPanelTransform = transform.GetChild(0).GetComponent<RectTransform>();
        dropdownButtonTransform = dropdownButton.GetComponent<RectTransform>();

        dropdownButton.onClick.AddListener(() =>
        {
            isDropdownOpen = !isDropdownOpen;
            dropdownPanelTransform.gameObject.SetActive(isDropdownOpen);
        });

        foreach (Button button in buttonList)
        {
            button.onClick.AddListener(() =>
            {
                dropdownButton.GetComponentInChildren<Text>().text = button.GetComponentInChildren<Text>().text;
                isDropdownOpen = false;
                dropdownPanelTransform.gameObject.SetActive(false);
            });
        }
    }

    void Update()
    {
        if (isDropdownOpen)
        {
            Vector3[] corners = new Vector3[4];
            dropdownButtonTransform.GetWorldCorners(corners);
            dropdownPanelTransform.position = corners[1];
            dropdownPanelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dropdownButtonTransform.rect.width);
        }
    }
}
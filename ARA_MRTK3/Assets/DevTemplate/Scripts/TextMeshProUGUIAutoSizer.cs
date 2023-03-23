using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextMeshProUGUIAutoSizer : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI textMeshProUGUI;
    private float width;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        width = rectTransform.sizeDelta.x;
    }

    void Start()
    {
        ResizeTextMeshProUGUI();
    }

    public void ResizeTextMeshProUGUI()
    {
        textMeshProUGUI.ForceMeshUpdate();

        Vector2 preferredSize = new Vector2(width, textMeshProUGUI.preferredHeight);
        rectTransform.sizeDelta = preferredSize;
    }
}

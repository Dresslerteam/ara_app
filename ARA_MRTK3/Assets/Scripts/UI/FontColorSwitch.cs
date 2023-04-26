using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FontColorSwitch : MonoBehaviour
{

    public enum TextGatheringType
    {
        Listed,
        FindInChildren,
        OnObject
    }

    public TextGatheringType switchType = TextGatheringType.FindInChildren;

    [SerializeField] private List<TextMeshProUGUI> textMeshPros = new List<TextMeshProUGUI>();
    [SerializeField] private List<Text> texts = new List<Text>();


    List<Color> textMeshProsDefaultColors = new List<Color>();

    List<Color> textsDefaultColors = new List<Color>();


    public Color HighlightColor = Color.white;

    // Start is called before the first frame update
    void Awake()
    {
        if(switchType == TextGatheringType.FindInChildren)
        {
            textMeshPros = GetComponentsInChildren<TextMeshProUGUI>().ToList();
            texts = GetComponentsInChildren<Text>().ToList();
        }else if (switchType == TextGatheringType.OnObject)
        {
            textMeshPros.Add(GetComponent<TextMeshProUGUI>());
            texts.Add(GetComponent<Text>());
        }


        for (int i = 0; i < textMeshPros.Count; i++)
        {
            textMeshProsDefaultColors.Add(textMeshPros[i].color);
        }
        for (int i = 0; i < texts.Count; i++)
        {
            textsDefaultColors.Add(texts[i].color);
        }
    }

   public void SetHighlitState(bool ishighlighted)
    {
        if (ishighlighted)
        {
            foreach (TextMeshProUGUI t in textMeshPros) t.color = HighlightColor;
            foreach (Text t in texts) t.color = HighlightColor;
        }
        else
        {

            for (int i = 0; i < textMeshPros.Count; i++)
            {
                textMeshPros[i].color = textMeshProsDefaultColors[i];
            }
            for (int i = 0; i < texts.Count; i++)
            {
                texts[i].color = textsDefaultColors[i];

            }
        }
    }
}

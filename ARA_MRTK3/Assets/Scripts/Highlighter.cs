using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Highlighter : MonoBehaviour
{
    // List of TextMeshPro objects to highlight
    public List<TextMeshProUGUI> textMeshes = new List<TextMeshProUGUI>();
    // List of Sprites to highlight
    [FormerlySerializedAs("sprites")] public List<RawImage> rawImages = new List<RawImage>();
    public Material highlightMaterial;
    public Material defaultMaterial;
    public Color highlightedColor = Color.black;
    public Color defaultColor = Color.white;
    
    
    private void Start()
    {
        // If no highlight material is assigned, use the default material
        if (highlightMaterial == null)
        {
            highlightMaterial = defaultMaterial;
        }
    }
    [Button]
    public void PopulateTextMeshes()
    {
        // Recursively get all TextMeshPro objects in the children of this object
        textMeshes.AddRange(GetComponentsInChildren<TextMeshProUGUI>(false));
    }
    [Button]
    public void PopulateSprites()
    {
        rawImages.AddRange(GetComponentsInChildren<RawImage>());
    }
    public void Highlight()
    {
        // Highlight all TextMeshPro objects
        foreach (TextMeshProUGUI textMesh in textMeshes)
        {
            textMesh.fontSharedMaterial = highlightMaterial;
            textMesh.color = Color.black;
            textMesh.ForceMeshUpdate();
        }
        
        // Highlight all Sprites
        foreach (RawImage sprite in rawImages)
        {
            sprite.material = highlightMaterial;
            sprite.color = highlightedColor;
        }
    }
    public void UnHighlight()
    {
        // Unhighlight all TextMeshPro objects
        foreach (TextMeshProUGUI textMesh in textMeshes)
        {
            textMesh.fontSharedMaterial = defaultMaterial;
            textMesh.color = defaultColor;
        }
        
        // Unhighlight all Sprites
        foreach (RawImage sprite in rawImages)
        {
            sprite.material = defaultMaterial;
            sprite.color = defaultColor;
        }
        Canvas.ForceUpdateCanvases();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Paroxe.PdfRenderer;
using Sirenix.OdinInspector;
using UnityEngine;

public class SantityCheck : MonoBehaviour
{

    public string filepath;
    public PDFViewer PdfViewer;
    [Button]
    public void LoadPDFFromResources()
    {
        PdfViewer.FileName = filepath;
        PdfViewer.LoadDocumentFromResources("Docs", filepath, 0);
    }
    
    [Button]
    public void ListAllResources()
    {
        // Get all resources in the "Resources" folder and its subfolders
        Object[] allResources = Resources.LoadAll("", typeof(TextAsset));

        // Filter the array to only include TextAssets with the ".bytes" extension
        TextAsset[] filteredResources = allResources.Where(x => x.name.EndsWith(".bytes")).Cast<TextAsset>().ToArray();

        // Print the paths of the filtered resources
        string[] resourcePaths = new string[filteredResources.Length];
        for (int i = 0; i < filteredResources.Length; i++)
        {
            resourcePaths[i] = filteredResources[i].name;
        }
        Debug.Log("Resources = " + string.Join(", ", resourcePaths));
    }
    [Button]
    public PDFAsset LoadSpecificPdf()
    {
        PDFAsset pdfTextAsset = Resources.Load<PDFAsset>(filepath);

        if (pdfTextAsset != null)
        {
            Debug.Log("Loaded PDF from Resources: " + filepath);
            return pdfTextAsset;
        }
        else
        {
            Debug.Log("Could not load PDF from Resources: " + filepath);
            return null;
        }
    }
}

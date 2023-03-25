using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Paroxe.PdfRenderer;
using Sirenix.OdinInspector;
using UnityEngine;

public class PDFLoader : MonoBehaviour
{
    [SerializeField] private PDFViewer pdfViewer;
    // Start is called before the first frame 
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void LoadPdf(string fileName)
    {
        if(!transform.gameObject.activeSelf)
            transform.gameObject.SetActive(true);
        pdfViewer.FileName = fileName;
        pdfViewer.LoadDocumentFromStreamingAssets("Docs", fileName+".bytes",0); 
    }
    public void HidePdf()
    {
        transform.gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using Paroxe.PdfRenderer;
using UnityEngine;

public class PDFLoader : MonoBehaviour
{
    [SerializeField] private PDFViewer pdfViewer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadPdf(string fileName)
    {
        pdfViewer.FileName = fileName;
        pdfViewer.LoadDocumentFromResources("Docs", fileName+".pdf.bytes",0); 
    }
}

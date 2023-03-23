using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Paroxe.PdfRenderer;
using Sirenix.OdinInspector;
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
        pdfViewer.LoadDocumentFromStreamingAssets("Docs", fileName+".bytes",0); 
    }
    public void HidePdf()
    {
        pdfViewer.gameObject.SetActive(false);
    }

}

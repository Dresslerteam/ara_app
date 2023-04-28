using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Paroxe.PdfRenderer;
using Sirenix.OdinInspector;
using UnityEngine;

public class PDFLoader : MonoBehaviour
{
    [SerializeField] private PDFViewer pdfViewer;
    bool initiated = false;
    // Start is called before the first frame 
    void Awake()
    {
        gameObject.SetActive(true);
    }
  
    public void LoadOrHide(bool _load, string fileName)
    {
        if (_load)
        {
            LoadPdf(fileName);
        }
        else
        {
            HidePdf();
        }
    }
    public void LoadPdf(string fileName)
    {
        transform.gameObject.SetActive(true);
        pdfViewer.FileName = fileName;
        pdfViewer.LoadDocumentFromStreamingAssets("Docs", fileName+".bytes",0); 
    }
    public void HidePdf()
    {
        transform.gameObject.SetActive(false);
    }

}

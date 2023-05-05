using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Paroxe.PdfRenderer;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.MixedReality.Toolkit.UX;

public class PDFLoader : MonoBehaviour
{
    [SerializeField] private PDFViewer pdfViewer;
    bool initiated = false;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private PressableButton pinButton;

    public string FileName = "";
    private PDFsManager manager;
 
    public void LoadPdf(PDFsManager _man, string fileName)
    {
        manager = _man;
        FileName = fileName;
        pdfViewer.FileName = fileName;
        pdfViewer.LoadDocumentFromStreamingAssets("Docs", fileName+".bytes",0);
        title.text = Path.GetFileName( fileName);
    }
    public void HidePdf()
    {
        transform.gameObject.SetActive(false);
        manager?.HidePdf(FileName);
    }
    public void Pin()
    {
       manager?.Pin(FileName);

    }
    public void UnPin()
    {
        manager?.Unpin(FileName);
    }

    public void ForceUnpin()
    {
        pinButton.ForceSetToggled(false,true);
    }
    public void ForcePin()
    {
        pinButton.ForceSetToggled(true, true);
    }
}

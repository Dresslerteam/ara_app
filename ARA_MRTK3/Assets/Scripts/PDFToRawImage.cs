using System.Collections;
using System.Collections.Generic;
using Paroxe.PdfRenderer;
using Paroxe.PdfRenderer.Examples;
using UnityEngine;

public class PDFToRawImage : MonoBehaviour
{
    void Start()
    {
        PDFDocument pdfDocument = new PDFDocument(PDFBytesSupplierExample.PDFSampleByteArray, "");
        int curPage = 0;
        if (pdfDocument.IsValid)
        {
            int pageCount = pdfDocument.GetPageCount();

            PDFRenderer renderer = new PDFRenderer();
            Texture2D tex = renderer.RenderPageToTexture(pdfDocument.GetPage(curPage % pageCount), 1024, 1024);

            tex.filterMode = FilterMode.Bilinear;
            tex.anisoLevel = 8;

            GetComponent<MeshRenderer>().material.mainTexture = tex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

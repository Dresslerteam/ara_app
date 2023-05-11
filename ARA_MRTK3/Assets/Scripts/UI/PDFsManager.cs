using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class PDFsManager : MonoBehaviour
{

    [SerializeField] private GameObject PDFViewPrefab;

    [SerializeField] private Transform container;

    [SerializeField] private Transform PinnedContainer;

    public List<PDFLoader> pinnedPDFObjects = new List<PDFLoader>();

    public List<PDFLoader> unpinnedPDFObjects = new List<PDFLoader>();


    private void Refresh()
    {
        for(int i = 0; i < unpinnedPDFObjects.Count; i++) PositionLoaderInContainerAtIndex(unpinnedPDFObjects[i], i);
        
    }

    public void LoadPdf(string fileName)
    {

        PDFLoader loader = FindPDF(name);

        if (loader == null)
        {

            GameObject obj = Instantiate(PDFViewPrefab);

            loader = obj.GetComponent<PDFLoader>();

            PositionLoaderLastInContainer(loader);

            loader.LoadPdf(this,fileName);

            unpinnedPDFObjects.Add(loader);
        }
        else
        {
            loader.ForceUnpin();
            Refresh();
        }
    }
 
    PDFLoader FindPDF(string name)
    {
        foreach (PDFLoader loader in pinnedPDFObjects)
            if (loader.FileName == name) return loader;

        foreach (PDFLoader loader in unpinnedPDFObjects)
            if (loader.FileName == name) return loader;

        return null;
    }


    public void Pin(string fileName)
    {
        PDFLoader loader = FindPDF(fileName);

        if(loader != null && unpinnedPDFObjects.Contains(loader))
        {
            unpinnedPDFObjects.Remove(loader);

            pinnedPDFObjects.Add(loader);

            loader.gameObject.transform.SetParent(PinnedContainer, true);
            Refresh();
        }
    }
    public void Unpin(string fileName)
    {
        PDFLoader loader = FindPDF(fileName);

        if (loader != null && pinnedPDFObjects.Contains(loader))
        {
            Unpin(loader);
        }
    }
    private void Unpin(PDFLoader loader)
    {
        unpinnedPDFObjects.Add(loader);

        pinnedPDFObjects.Remove(loader);

        PositionLoaderLastInContainer(loader);


    }

    private Vector3 NextPosition()
    {
        return findPosition(unpinnedPDFObjects.Count);
    }
    private Vector3 findPosition(int index)
    {
        return new Vector3(.1f, -.1f, -.1f) * index;
    }
    private void PositionLoaderLastInContainer(PDFLoader loader)
    {
        PositionLoaderInContainer(loader, NextPosition());
    }

    private void PositionLoaderInContainerAtIndex(PDFLoader loader, int index)
    {
        PositionLoaderInContainer(loader, findPosition(index));
    }

    private void PositionLoaderInContainer(PDFLoader loader, Vector3 position)
    {
        loader.gameObject.transform.SetParent(container);

        loader.transform.localScale = new Vector3(1.5f, 1.5f, 1f);

        loader.transform.localPosition = position;

        loader.transform.localRotation = Quaternion.identity;
    }


    public void HidePdf(string fileName)
    {
        PDFLoader loader = FindPDF(fileName);
        if(loader != null)
        {
            if (pinnedPDFObjects.Contains(loader)) pinnedPDFObjects.Remove(loader);
            if (unpinnedPDFObjects.Contains(loader)) unpinnedPDFObjects.Remove(loader);
            Destroy(loader.gameObject);
            Refresh();
        }
    }

    public void LoadOrHide(bool isActive,string fileName)
    {
        if (isActive)
        {
            LoadPdf(fileName);
        }
        else
        {
            HidePdf(fileName);
        }
    }

    
}

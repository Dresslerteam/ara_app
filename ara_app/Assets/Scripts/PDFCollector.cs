using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class PDFCollector : MonoBehaviour
{
    [SerializeField] private Transform gridRoot;
    [SerializeField] private GameObject pdfButtonPrefab;

    [SerializeField] private List<pdfData> pdfLinks = new List<pdfData>(); 
    // Start is called before the first frame update
    void Start()
    {
        PopulateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateButtons()
    {
        foreach (var pdfLink in pdfLinks)
        {
            GameObject newButton = (GameObject)Instantiate(pdfButtonPrefab);
            TextMeshProUGUI labelText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            //labelText.text = pdfLink.url;
            labelText.text = pdfLink.title;
            newButton.transform.parent = gridRoot;
            newButton.transform.localScale = new Vector3(1, 1, 1);
            Interactable buttonInteractable = newButton.GetComponent<Interactable>();
            buttonInteractable.OnClick.AddListener(AddLinkToButton(pdfLink.url));
        }
    }

    private UnityAction AddLinkToButton(string link)
    {
        UnityAction open = delegate { OpenLink(link); };
        return open;
    }
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
    
    /*
    IEnumerator GetText(string link) {
        UnityWebRequest www = UnityWebRequest.Get(link);
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
 
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }*/
}

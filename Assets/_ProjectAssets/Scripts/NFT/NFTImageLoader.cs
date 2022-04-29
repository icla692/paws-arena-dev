using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class NFTImageLoader : MonoBehaviour
{
    public string URL;

    private async void Start()
    {
        XmlDocument doc = new XmlDocument();
        string xmlString = await LoadXMLFromURL(URL);
        doc.LoadXml(xmlString);

        var nsMan = new XmlNamespaceManager(doc.NameTable);
        nsMan.AddNamespace("ns", "http://www.w3.org/2000/svg");
        var images = doc.ChildNodes[1].SelectNodes("//ns:g", nsMan);
        for(int i=0; i < images.Count; i++){
            var id = images[i].Attributes["id"];
            if (id != null)
            {
                Debug.Log("ID: " + id.Value);
            }
        }
    }

    private async UniTask<string> LoadXMLFromURL(string URL)
    {
        UnityWebRequest www = new UnityWebRequest(URL, "GET");
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        try
        {
            await www.SendWebRequest();
        }catch(UnityWebRequestException e)
        {
            Debug.LogWarning("Error on " + URL + ": " + www.downloadHandler.text);
        }

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Error on " + URL + " : " + www.error);
        }

        string rawText = www.downloadHandler.text;
        Debug.Log("Success: " + rawText);

        return rawText;
    }


}

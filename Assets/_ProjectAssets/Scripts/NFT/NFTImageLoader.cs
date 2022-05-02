using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Networking;
using static Unity.VectorGraphics.SVGParser;

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

    //Current SVG reader from unity doesn't read well base64 encoded png inside SVG
    //private void CreateSVG(string xmlTest)
    //{
    //    using (var reader = new StringReader(xmlTest)) {
    //        SceneInfo svg = SVGParser.ImportSVG(reader);
    //        VectorUtils.TessellationOptions tessellationOptions = new VectorUtils.TessellationOptions();
    //        var geoms = VectorUtils.TessellateScene(svg.Scene, tessellationOptions);

    //        var sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);
    //        sprite.name = "My SVG";
    //        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    //        spriteRenderer.sprite = sprite;
    //    }
    //}


}

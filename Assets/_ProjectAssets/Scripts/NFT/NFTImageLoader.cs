using Anura.ConfigurationModule.Managers;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static Unity.VectorGraphics.SVGParser;

public class NFTImageLoader
{
    public static async UniTask<XmlDocument> LoadSVGXML(string URL)
    {
        XmlDocument doc = new XmlDocument();
        string xmlString = await LoadXMLFromURL(URL);
        doc.LoadXml(xmlString);

        return doc;
    }
    public static Texture2D LoadNFT(XmlDocument doc)
    {
        var nsMan = new XmlNamespaceManager(doc.NameTable);
        nsMan.AddNamespace("ns", "http://www.w3.org/2000/svg");

        var images = doc.ChildNodes[0].SelectNodes("//ns:image", nsMan);

        Texture2D finalTex = new Texture2D(1000, 1000);

        for (int i = 0; i < images.Count; i++)
        {
            var id = images[i].Attributes["id"];

            if (id == null) continue;

            //Positioning
            int offsetX = 0, offsetY = 0;

            XmlNode xNode = images[i].Attributes["x"];
            if (xNode != null)
            {
                int.TryParse(images[i].Attributes["x"].Value, out offsetX);
            }

            XmlNode yNode = images[i].Attributes["y"];
            if (yNode != null)
            {
                int.TryParse(images[i].Attributes["y"].Value, out offsetY);
            }


            //Generate Tex
            Texture2D tex = ImageFromBase64(images[i].Attributes["href"].Value.Split(",")[1]);

            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    Color srcCol = tex.GetPixel(x, y);
                    if (srcCol.a > 0)
                    {
                        int destX = offsetX + x;
                        int destY = (1000 - tex.height) - offsetY + y;
                        Color destCol = finalTex.GetPixel(destX, destY);
                        Color blendedCol = Color.Lerp(destCol, srcCol, srcCol.a);
                        finalTex.SetPixel(destX, destY, blendedCol);
                    }
                }
            }
            finalTex.Apply();
        }

        return finalTex;
    }

    private static Texture2D ImageFromBase64(string base64String)
    {
        Texture2D tex = new Texture2D(2, 2);
        byte[] base64 = Convert.FromBase64String(base64String);
        tex.LoadImage(base64);
        return tex;
    }

    public static string GetFurType(XmlDocument doc)
    {
        var nsMan = new XmlNamespaceManager(doc.NameTable);
        nsMan.AddNamespace("ns", "http://www.w3.org/2000/svg");

        var images = doc.ChildNodes[0].SelectNodes("//ns:g", nsMan);
        for (int i = 0; i < images.Count; i++)
        {
            var id = images[i].Attributes["id"];
            if (id != null && id.Value.Contains("kittycolor"))
            {
                return id.Value;
            }
        }

        return "Unkown Fur";
    }

    public static List<string> GetIds(XmlDocument doc)
    {
        var nsMan = new XmlNamespaceManager(doc.NameTable);
        nsMan.AddNamespace("ns", "http://www.w3.org/2000/svg");
        var list = new List<string>();
        var images = doc.ChildNodes[0].SelectNodes("//ns:g", nsMan);
        for (int i = 0; i < images.Count; i++)
        {
            var id = images[i].Attributes["id"];
            if (id != null)
            {
                list.Add(id.Value);
            }
        }

        return list;

    }

    private static async UniTask<string> LoadXMLFromURL(string URL)
    {
        UnityWebRequest www = new UnityWebRequest(URL, "GET");
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        try
        {
            await www.SendWebRequest();
        }
        catch (UnityWebRequestException e)
        {
            Debug.LogWarning("Error on " + URL + ": " + www.downloadHandler.text);
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Error on " + URL + " : " + www.error);
        }

        string rawText = www.downloadHandler.text;
        if (ConfigurationManager.Instance.GameConfig.enableDevLogs)
        {
            Debug.Log("URL: " + URL);
            Debug.Log("Success: " + rawText);
        }

        return rawText;
    }


}

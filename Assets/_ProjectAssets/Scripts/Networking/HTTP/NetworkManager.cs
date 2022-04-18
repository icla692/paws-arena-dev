using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    public static async UniTask<Texture2D> GetImage(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.downloadHandler = new DownloadHandlerTexture();
        DownloadHandlerTexture dht = (DownloadHandlerTexture)www.downloadHandler;
        await www.SendWebRequest();
        Debug.Log($"<color=red> dht.error </color>");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error on {url} {www.error}");
            return null;
        }
        var tex = dht.texture;
        return tex;
    }



    public static async UniTask<Sprite> GetImageSprite(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.downloadHandler = new DownloadHandlerTexture();
        DownloadHandlerTexture dht = (DownloadHandlerTexture)www.downloadHandler;
        await www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Error on " + url + " : " + www.error);
            return null;
        }
        var tex = dht.texture;
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
        return sprite;
    }
}

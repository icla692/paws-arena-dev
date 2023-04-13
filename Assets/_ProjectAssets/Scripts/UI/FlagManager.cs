using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class FlagManager : MonoBehaviour
{
    [SerializeField] Image flagImageDisplay;
    [SerializeField] Image flagShineDisplay;
    [SerializeField] TextMeshProUGUI messageDisplay;
    [SerializeField] string imageUrl;
    [SerializeField] string detailsUrl;

    static Sprite flagSprite = null;
    static string message;

    private void Start()
    {
        if (flagSprite == null)
        {
            StartCoroutine(GetImageFromUrl());
            StartCoroutine(GetMessage());
        }
        else
        {
            flagImageDisplay.sprite = flagSprite;
            flagShineDisplay.sprite = flagSprite;
            messageDisplay.text = message;
        }
    }

    IEnumerator GetImageFromUrl()
    {
        UnityWebRequest _request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return _request.SendWebRequest();

        if (_request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Something went wrong while downloading flag image");
        }
        else
        {
            Texture2D _texture = ((DownloadHandlerTexture)_request.downloadHandler).texture;
            Rect _rect = new Rect(0, 0, _texture.width, _texture.height);
            Sprite _sprite = Sprite.Create(_texture, _rect, new Vector2(0.5f, 0.5f), 100);
            flagSprite = _sprite;
            flagImageDisplay.sprite = _sprite;
            flagShineDisplay.sprite = _sprite;
        }
    }

    IEnumerator GetMessage()
    {
        UnityWebRequest _request = UnityWebRequest.Get(detailsUrl);
        yield return _request.SendWebRequest();

        if (_request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Something went wrong while downloading tournament details");
        }
        else
        {
            var _jsonData = _request.downloadHandler.text;
            TournamentResponse _response = JsonConvert.DeserializeObject<TournamentResponse>(_jsonData);
            message = _response.Message;
            messageDisplay.text = message;
        }
    }
}

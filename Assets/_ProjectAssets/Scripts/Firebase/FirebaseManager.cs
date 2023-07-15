using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    [SerializeField] FirebaseTokenManager tokenManager;

    const string WEB_API_KEY = "AIzaSyCFi-0SrkVoZbzv3GzxacoJBtELJdeFTt8";

    string userLocalId;
    string userIdToken;
    string projectLink = "https://pawsarena-b8133-default-rtdb.firebaseio.com/";
    string userDataLink => $"{projectLink}/users/{userLocalId}/";
    string gameDataLink => $"{projectLink}/gameData/";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TryLoginAndGetData(string _principalId, Action<bool> _callBack)
    {
        if (!string.IsNullOrEmpty(userLocalId))
        {
            _callBack?.Invoke(true);
            return;
        }

        _principalId = _principalId.Replace("-","");
        string _loginParms = "{\"email\":\"" + _principalId + "@paws.arena\",\"password\":\"" + GeneratePassword(_principalId) + "\",\"returnSecureToken\":true}";
        StartCoroutine(Post("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + WEB_API_KEY, _loginParms, (_result) =>
        {
            SignInResponse _signInResponse = JsonConvert.DeserializeObject<SignInResponse>(_result);
            userIdToken = _signInResponse.IdToken;
            userLocalId = _signInResponse.LocalId;
            tokenManager.Setup(WEB_API_KEY, _signInResponse.RefreshToken);
            CollectGameData(_callBack);
        }, (_result) =>
        {
            Register(_callBack,_loginParms);
        }, false));
    }

    string GeneratePassword(string _principalId)
    {
        string _password = string.Empty;
        _password += _principalId[5];
        _password += _principalId[0];
        _password += _principalId[2];
        _password += _principalId[7];
        _password += _principalId[3];
        _password += _principalId[5];
        _password += _principalId[5];
        _password += _principalId[1];
        _password += _principalId[6];
        _password += _principalId[6];
        _password += _principalId[0];
        _password += _principalId[5];

        return _password;
    }

    void Register(Action<bool> _callBack,string _parms)
    {
        StartCoroutine(Post("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + WEB_API_KEY, _parms, (_result) =>
        {
            RegisterResponse _registerResult = JsonConvert.DeserializeObject<RegisterResponse>(_result);
            userIdToken = _registerResult.IdToken;
            userLocalId = _registerResult.LocalId;
            tokenManager.Setup(WEB_API_KEY, _registerResult.RefreshToken);
            DataManager.Instance.CreatePlayerDataEmpty();
            SetStartingData(_callBack);
        }, (_result) =>
        {
            Debug.Log("Register failed");
            _callBack?.Invoke(false);
        }));
    }

    void SetStartingData(Action<bool> _callBack)
    {
        string _data = JsonConvert.SerializeObject(DataManager.Instance.PlayerData);
        StartCoroutine(Put(userDataLink+"/.json", _data, (_result) =>
        {
            CollectGameData(_callBack);
        }, (_result) =>
        {
            _callBack?.Invoke(false);
        }));
    }

    void CollectGameData(Action<bool> _callBack)
    {
        StartCoroutine(Get(gameDataLink + "/.json", (_result) =>
        {
            DataManager.Instance.SetGameData(_result);
            CollectPlayerData(_callBack);
        }, (_result) =>
        {
            _callBack?.Invoke(false);
        }));
    }

    void CollectPlayerData(Action<bool> _callBack)
    {
        StartCoroutine(Get(userDataLink + "/.json", (_result) =>
        {
            DataManager.Instance.SetPlayerData(_result);
            _callBack?.Invoke(true);
        }, (_result) =>
        {
            _callBack?.Invoke(false);
        }));
    }

    public void SaveValue<T>(string _path, T _value)
    {
        string _valueString = "{\"" + _path + "\":" + _value + "}";
        StartCoroutine(Patch(userDataLink + ".json", _valueString, (_result) =>
        {

        }, (_result) =>
        {
            Debug.Log(_valueString);
            Debug.Log("Failed to update data, please try again later");
            Debug.Log(_result);
        }));
    }
    
    public void UpdateValue<T>(string _path, T _value)
    {
        string _valueString = "{\"" + _path + "\":" + _value + "}";

        StartCoroutine(Patch(userDataLink + ".json", _valueString, (_result) =>
        {

        }, (_result) =>
        {
            Debug.Log("Failed to update data, please try again later");
            Debug.Log(_result);
        }));
    }


    IEnumerator Get(string uri, Action<string> onSuccess, Action<string> onError)
    {
        if (userIdToken != null)
        {
            uri = $"{uri}?auth={userIdToken}";
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(webRequest.error);
                onError?.Invoke(webRequest.error);
            }

            webRequest.Dispose();
        }
    }

    IEnumerator Post(string uri, string jsonData, Action<string> onSuccess, Action<string> onError, bool _includeHeader = true)
    {
        if (userIdToken != null)
        {
            if (_includeHeader)
            {
                uri = $"{uri}?auth={userIdToken}";
            }
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, jsonData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(webRequest.error);
                onError?.Invoke(webRequest.error);
            }

            webRequest.uploadHandler.Dispose();
            webRequest.downloadHandler.Dispose();
            webRequest.Dispose();
        }
    }

    IEnumerator Put(string uri, string jsonData, Action<string> onSuccess, Action<string> onError)
    {
        // If the userIdToken is available, append it to the URI
        if (userIdToken != null)
        {
            uri = $"{uri}?auth={userIdToken}";
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, jsonData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(webRequest.error);
                onError?.Invoke(webRequest.error);
            }

            webRequest.uploadHandler.Dispose();
            webRequest.downloadHandler.Dispose();
            webRequest.Dispose();
        }
    }

    IEnumerator Patch(string uri, string jsonData, Action<string> onSuccess, Action<string> onError)
    {
        if (userIdToken != null)
        {
            uri = $"{uri}?auth={userIdToken}";
        }

        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "PATCH"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();


            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                onError?.Invoke(webRequest.error);
            }

            webRequest.uploadHandler.Dispose();
            webRequest.downloadHandler.Dispose();
            webRequest.Dispose();
        }
    }
}

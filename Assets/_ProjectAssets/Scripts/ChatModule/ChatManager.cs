using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private Button sendButton;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private ChatLine chatLine;
    [SerializeField] private RectTransform content;

    private void Awake()
    {
        sendButton.onClick.AddListener(() => Test());
    }

    private void Test()
    {
        if (inputField.text.Equals(string.Empty))
            return;

        GetComponent<PhotonView>().RPC("ChatMessage", RpcTarget.All, inputField.text);
    }

    [PunRPC]
    void ChatMessage(string message, PhotonMessageInfo info)
    {
        var line = Instantiate(chatLine, content.transform);
        line.SetText(message, info.Sender.IsLocal);
        if(info.Sender.IsLocal)
        {
            inputField.SetTextWithoutNotify(string.Empty);
        }
    }
}
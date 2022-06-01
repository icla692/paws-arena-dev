using Anura.ConfigurationModule.Managers;
using Anura.Extensions;
using Anura.Templates.MonoSingleton;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChatLinePool))]
public class ChatManager : MonoSingleton<ChatManager>
{
    [SerializeField] private Button openChat;
    [SerializeField] private Button sendButton;
    [SerializeField] private Button chatBackground;
    [SerializeField] private Button xChatButton;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private ChatLine chatLine;

    [SerializeField] private GameObject chatPanel;
    [SerializeField] private RectTransform content;

    private ChatLinePool chatLinePool;

    protected override void Awake()
    {
        base.Awake();
        openChat.onClick.AddListener(() => SetChatPanelBehaviour(!GetChatPanelStatus()));
        chatBackground.onClick.AddListener(() => SetChatPanelBehaviour(!GetChatPanelStatus()));
        xChatButton.onClick.AddListener(() => SetChatPanelBehaviour(!GetChatPanelStatus()));
        sendButton.onClick.AddListener(() => SendMessage());

        chatLinePool = GetComponent<ChatLinePool>();
        
        InitializeContent(ConfigurationManager.Instance.Config.GetNumberOfLines());
    }

    public bool GetChatPanelStatus()
    {
        return chatPanel.activeSelf;
    }

    private void SetChatPanelBehaviour(bool isActive)
    {
        chatPanel.SetActive(isActive);
        GameInputManager.Instance.GetPlayerActionMap().SetActivePlayerActionMap(!isActive);
    }

    private void InitializeContent(int numberOfLines)
    {
        for (int i = 0; i < numberOfLines; i++)
        {
            var line = Instantiate(chatLine, content.transform);
            chatLinePool.AddObjectToPool(line);
        }
    }

    private void SendMessage()
    {
        if (inputField.text.IsEmptyOrWhiteSpace())
            return;

        GetComponent<PhotonView>().RPC("ChatMessage", RpcTarget.All, inputField.text);
    }

    [PunRPC]
    void ChatMessage(string message, PhotonMessageInfo info)
    {
        var line = chatLinePool.GetObjectFromPool();
        chatLinePool.AddObjectToPool(line);
        line.SetText(message, info.Sender.IsLocal);
        if(info.Sender.IsLocal)
        {
            inputField.text = string.Empty;
        }

        content.anchoredPosition = content.anchoredPosition.WithY(ConfigurationManager.Instance.Config.GetHeightRefreshingChat());
    }
}
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EmojiChatManager : MonoBehaviour
{
    [SerializeField] Button emojiButton;
    [SerializeField] Button emojiPanel;
    [SerializeField] Transform emojisPreviewHolder;
    [SerializeField] EmojiPreviewDisplay emojiPreviewPrefab;
    [SerializeField] float emojiCooldown;
    [SerializeField] Image emojiCooldownDisplay;

    PhotonView photonView;
    bool canSendEmoji = true;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        SpawnEmojis();
        emojiButton.onClick.AddListener(ShowEmojis);
        emojiPanel.onClick.AddListener(Close);
        EmojiPreviewDisplay.ShowEmoji += ShowEmoji;
    }

    private void OnDestroy()
    {
        emojiButton.onClick.RemoveListener(ShowEmojis);
        emojiPanel.onClick.RemoveListener(Close);
        EmojiPreviewDisplay.ShowEmoji -= ShowEmoji;
    }

    void SpawnEmojis()
    {
        foreach (var _emoji in EmojiSO.GetEmojis())
        {
            EmojiPreviewDisplay _emojiPreviewDisplay = Instantiate(emojiPreviewPrefab, emojisPreviewHolder);
            _emojiPreviewDisplay.Setup(_emoji);
        }
    }

    void ShowEmojis()
    {
        if (!canSendEmoji)
        {
            return;
        }
        emojiPanel.gameObject.SetActive(true);
    }

    void ShowEmoji(int _emojiId)
    {
        StartCoroutine(EmojiCooldown());
        ShowEmoji(_emojiId, true);
        photonView.RPC("ShowEmoji", RpcTarget.Others, _emojiId, false);
        Close();
    }

    IEnumerator EmojiCooldown()
    {
        canSendEmoji = false;
        emojiCooldownDisplay.gameObject.SetActive(true);
        float _counter = 0;
        while (_counter < emojiCooldown)
        {
            if (_counter == 0)
            {
                emojiCooldownDisplay.fillAmount = 0;
            }
            else
            {
                emojiCooldownDisplay.fillAmount = 1 - (_counter / emojiCooldown);
            }
            yield return null;
            _counter += Time.deltaTime;
        }
        canSendEmoji = true;
        emojiCooldownDisplay.gameObject.SetActive(false);
    }

    [PunRPC]
    void ShowEmoji(int _emojiId, bool _showAboveMyHead)
    {
        Transform _emojiHolder = _showAboveMyHead ? PlayerManager.Instance.myPlayer.EmojiHolder : PlayerManager.Instance.OtherPlayerComponent.EmojiHolder;
        EmojiSO _emojiSO = EmojiSO.GetEmoji(_emojiId);

        EmojiVisual _emojiVisual = Instantiate(_emojiSO.Visual, _emojiHolder).GetComponent<EmojiVisual>();
        _emojiVisual.Setup(_emojiSO);
    }

    void Close()
    {
        emojiPanel.gameObject.SetActive(false);
    }
}

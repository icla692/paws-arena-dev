using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipScreen : MonoBehaviour
{
    public LobbyUIManager lobbyUIManager;
    public Transform playerPlatformParent;
    public GameObject playerPlatformPrefab;

    public EquipmentsConfig equipmentsConfig;

    public Transform content;
    public GameObject nftPrefab;

    [Header("Btns")]
    public ButtonHoverable eyeBtn;
    public ButtonHoverable headBtn;
    public ButtonHoverable mouthBtn;
    public ButtonHoverable bodyBtn;
    public ButtonHoverable tailBtn;

    private GameObject playerPlatform;
    private ButtonHoverable selectedBtn;

    private List<NFTImageSprite> equipments;
    private PlayerCustomization playerCustomization;
    private EquipmentType currentType;
    private NFTImageSprite selectedEquipment;

    private void OnEnable()
    {
        playerPlatform = GameObject.Instantiate(playerPlatformPrefab, playerPlatformParent);
        playerPlatform.transform.localPosition = Vector3.zero;

        playerCustomization = playerPlatform.GetComponent<PlayerPlatformBehaviour>().playerCustomization;

        equipments = new List<NFTImageSprite>();
        StartCoroutine(InitCoroutine());
    }

    private IEnumerator InitCoroutine()
    {
        yield return new WaitForEndOfFrame();
        PopulateEyes();
    }

    private void OnDisable()
    {
        if (playerPlatform != null)
        {
            Destroy(playerPlatform);
            playerPlatform = null;
        }
        DePopulate();
    }

    public void PopulateEyes()
    {
        Populate(EquipmentType.EYEWEAR, equipmentsConfig.eyes, eyeBtn);
    }
    public void PopulateHead()
    {
        Populate(EquipmentType.HAT, equipmentsConfig.head, headBtn);
    }
    public void PopulateMouth()
    {
        Populate(EquipmentType.MOUTH, equipmentsConfig.mouth, mouthBtn);
    }
    public void PopulateBody()
    {
        Populate(EquipmentType.BODY, equipmentsConfig.body, bodyBtn);
    }
    public void PopulateTail()
    {
        Populate(EquipmentType.TAIL, equipmentsConfig.tailsOverlay, tailBtn);
        Populate(EquipmentType.TAIL, equipmentsConfig.tailsFloating, tailBtn, false);
        Populate(EquipmentType.TAIL, equipmentsConfig.tailsAnimated, tailBtn, false);
    }

    private void Populate(EquipmentType eqType, object elements, ButtonHoverable targetBtn, bool shouldClean = true)
    {
        currentType = eqType;

        selectedEquipment = null;

        selectedBtn?.Deselect();
        selectedBtn = targetBtn;
        selectedBtn.Select();

        if (shouldClean)
        {
            foreach (Transform t in content)
            {
                Destroy(t.gameObject);
                equipments.Clear();
            }
        }

        Equipment equippedItem = null;
        if (playerCustomization.playerEquipmentConfig.ContainsKey(eqType))
        {
            equippedItem = playerCustomization.playerEquipmentConfig[eqType];
        }

        if (elements is List<Sprite> spriteEls)
        {
            Populate(equippedItem, spriteEls);
        }else if(elements is List<SpriteIdPair> spriteIdEls)
        {
            Populate(equippedItem, spriteIdEls);
        }

    }

    private void Populate(Equipment equippedItem, List<Sprite> elements)
    {
        foreach (Sprite el in elements)
        {
            var go = GameObject.Instantiate(nftPrefab, content);
            var nftImageSprite = go.GetComponent<NFTImageSprite>();
            nftImageSprite.mainImage.sprite = el;
            equipments.Add(nftImageSprite);

            if (equippedItem != null && equippedItem is SpriteEquipment spriteItem && el == spriteItem.sprite)
            {
                Debug.Log("Found match for " + spriteItem.sprite.name);
                nftImageSprite.Select();
                selectedEquipment = nftImageSprite;
            }

            int idx = equipments.Count - 1;
            equipments[idx].onClick += () => OnEquipmentSelected(idx);
        }
    }

    private void Populate(Equipment equippedItem, List<SpriteIdPair> elements)
    {
        foreach (SpriteIdPair el in elements)
        {
            var go = GameObject.Instantiate(nftPrefab, content);
            var nftImageSprite = go.GetComponent<NFTImageSprite>();
            nftImageSprite.mainImage.sprite = el.thumbnail;
            equipments.Add(nftImageSprite);

            //if (equippedItem != null && equippedItem is GameObjectEquipment goItem && el == spriteItem.sprite)
            //{
            //    Debug.Log("Found match for " + spriteItem.sprite.name);
            //    nftImageSprite.Select();
            //    selectedEquipment = nftImageSprite;
            //}

            int idx = equipments.Count - 1;
            equipments[idx].onClick += () => OnEquipmentSelected(idx, el);
        }
    }

    private void OnEquipmentSelected(int idx, SpriteIdPair idPair = null)
    {
        selectedEquipment?.Deselect();
        selectedEquipment = equipments[idx];
        selectedEquipment.Select();

        if (equipments[idx].mainImage.sprite.name != "none")
        {
            if (idPair != null)
            {
                playerCustomization.SetTail(idPair.id);
            }
            else
            {
                playerCustomization.SetEquipmentBySprite(currentType, equipments[idx].mainImage.sprite);
            }
        }
        else
        {
            playerCustomization.SetEquipmentBySprite(currentType, null);
        }
    }

    private void DePopulate()
    {
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
            equipments.Clear();
        }
    }

    public void SaveAndClose()
    {
        playerCustomization.SaveCustomConfig();
        lobbyUIManager.OpenGameMenu();
    }

    public void ResetConfig()
    {
        playerCustomization.ResetToOriginal();
    }
}

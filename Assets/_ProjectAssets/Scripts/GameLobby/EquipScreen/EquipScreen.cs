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
    public ButtonHoverable legsBtn;

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
    }

    private void Start()
    {
        equipments = new List<NFTImageSprite>();
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
        Populate(EquipmentType.TAIL, equipmentsConfig.tail, tailBtn);
    }
    public void PopulateLegs()
    {
        Populate(EquipmentType.LEGS, equipmentsConfig.legs, legsBtn);
    }

    private void Populate(EquipmentType eqType, List<Sprite> elements, ButtonHoverable targetBtn)
    {
        currentType = eqType;

        selectedEquipment = null;

        selectedBtn?.Deselect();
        selectedBtn = targetBtn;
        selectedBtn.Select();

        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
            equipments.Clear();
        }

        Equipment equippedItem = null;
        if (playerCustomization.playerEquipmentConfig.ContainsKey(eqType))
        {
            equippedItem = playerCustomization.playerEquipmentConfig[eqType];
        }

        foreach(Sprite el in elements)
        {
            var go = GameObject.Instantiate(nftPrefab, content);
            var nftImageSprite = go.GetComponent<NFTImageSprite>();
            nftImageSprite.mainImage.sprite = el;
            equipments.Add(nftImageSprite);

            if(equippedItem != null && equippedItem is SpriteEquipment spriteItem && 
                el == spriteItem.sprite)
            {
                Debug.Log("Found match for " + spriteItem.sprite.name);
                nftImageSprite.Select();
                selectedEquipment = nftImageSprite;
            }

            int idx = equipments.Count - 1;
            equipments[idx].onClick += ()=> OnEquipmentSelected(idx);
        }
    }

    private void OnEquipmentSelected(int idx)
    {
        selectedEquipment?.Deselect();
        selectedEquipment = equipments[idx];
        selectedEquipment.Select();
        playerCustomization.SetEquipmentBySprite(currentType, equipments[idx].mainImage.sprite);
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

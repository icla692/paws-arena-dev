using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipScreen : MonoBehaviour
{
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
        currentType = EquipmentType.EYEWEAR;
        Populate(equipmentsConfig.eyes, eyeBtn);
    }
    public void PopulateHead()
    {
        currentType = EquipmentType.HEAD;
        Populate(equipmentsConfig.head, headBtn);
    }
    public void PopulateMouth()
    {
        currentType = EquipmentType.MOUTH;
        Populate(equipmentsConfig.mouth, mouthBtn);
    }
    public void PopulateBody()
    {
        currentType = EquipmentType.BODY;
        Populate(equipmentsConfig.body, bodyBtn);
    }
    public void PopulateTail()
    {
        currentType = EquipmentType.TAIL;
        Populate(equipmentsConfig.tail, tailBtn);
    }
    public void PopulateLegs()
    {
        currentType = EquipmentType.LEGS;
        Populate(equipmentsConfig.legs, legsBtn);
    }

    private void Populate(List<Sprite> elements, ButtonHoverable eyeBtn)
    {
        selectedBtn?.Deselect();
        selectedBtn = eyeBtn;
        selectedBtn.Select();

        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
            equipments.Clear();
        }

        foreach(Sprite el in elements)
        {
            var go = GameObject.Instantiate(nftPrefab, content);
            go.GetComponent<NFTImageSprite>().mainImage.sprite = el;
            equipments.Add(go.GetComponent<NFTImageSprite>());

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
}

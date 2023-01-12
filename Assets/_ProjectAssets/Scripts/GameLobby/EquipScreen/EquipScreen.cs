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

    private void OnEnable()
    {
        playerPlatform = GameObject.Instantiate(playerPlatformPrefab, playerPlatformParent);
        playerPlatform.transform.localPosition = Vector3.zero;
    }

    private void Start()
    {
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
        Populate(equipmentsConfig.eyes, eyeBtn);
    }
    public void PopulateHead()
    {
        Populate(equipmentsConfig.head, headBtn);
    }
    public void PopulateMouth()
    {
        Populate(equipmentsConfig.mouth, mouthBtn);
    }
    public void PopulateBody()
    {
        Populate(equipmentsConfig.body, bodyBtn);
    }
    public void PopulateTail()
    {
        Populate(equipmentsConfig.tail, tailBtn);
    }
    public void PopulateLegs()
    {
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
        }

        foreach(Sprite el in elements)
        {
            var go = GameObject.Instantiate(nftPrefab, content);
            go.transform.Find("Image").GetComponent<Image>().sprite = el;
        }
    }

    private void DePopulate()
    {
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }
    }
}

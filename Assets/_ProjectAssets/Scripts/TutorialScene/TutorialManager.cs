using Anura.Templates.MonoSingleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoSingleton<TutorialManager>
{

    [Header("Data")]
    [TextArea]
    public List<string> messages;

    [Header("Elements")]
    public Image bg;
    public GameObject pascal;
    public GameObject pascalMainLocation;
    public GameObject pascalDownLocation;
    public GameObject pascalSecondaryPosition;
    public DummyBehaviour dummy;

    public List<GameObject> tutorialGameObjects;

    public GameObject textBox;
    public TMPro.TextMeshProUGUI textBoxContent;

    public Transform mainCanvas;
    public Transform overlayCanvas;

    public bool finished = false;

    [Header("Instructions")]
    public GameObject lowerLeftInstructions;
    public TMPro.TextMeshProUGUI lowerLeftInstructionsText;

    public GameObject upperRightInstructions;
    public TMPro.TextMeshProUGUI upperRightInstructionsText;
    public GameObject upperLeftCloseableInstructions;
    public TMPro.TextMeshProUGUI upperLeftCloseableInstructionsText;

    [Header("Stage 2")]
    public List<GameObject> stage2_objectsToActivate;
    public List<Transform> stage2_UIToHighlight;

    [Header("Stage 7")]
    public PlayerActionsBar playerActionsBar;
    public List<GameObject> weapons;
    public GameObject arrow_stage7;

    private bool enteredJumpCollider = false;
    private int idx = -1;
    private Transform oldParentWeapon0;

    private void Awake()
    {
        SetTutorialGameObjectsVisible(false);
    }

    private void Start()
    {
        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDestroy()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void OnStateUpdated(IRoomState state)
    {
        if (state is GamePausedState)
        {
            SetStage1();
        }
    }

    public void OnNext()
    {
        if(idx == 1)
        {
            idx++;
            SetStage2();
        }else if(idx == 2)
        {
            idx++;
            SetStage3();
        }else if(idx == 3)
        {
            idx = 3;
            StartCoroutine(SetStage4());
        }else if(idx == 6)
        {
            idx = 7;
            SetStage7();
        }
    }

    private void SetStage1()
    {
        textBox.transform.localScale = Vector3.zero;
        SetTutorialGameObjectsVisible(true);
        LeanTween.scale(textBox, Vector3.one, 0.5f).setEaseOutBack();
        textBoxContent.text = messages[0];

        idx = 1;
    }

    private void SetStage2()
    {
        PlayerManager.Instance.DirectDamage(45);
        textBoxContent.text = messages[1];
        foreach (Transform t in stage2_UIToHighlight)
        {
            t.parent = overlayCanvas;
        }
        foreach (GameObject go in stage2_objectsToActivate)
        {
            go.SetActive(true);
        }
    }

    private void SetStage3()
    {
        textBoxContent.text = messages[2];
    }

    private IEnumerator SetStage4()
    {
        idx = 4;
        foreach (Transform t in stage2_UIToHighlight)
        {
            t.parent = mainCanvas;
        }
        foreach (GameObject go in stage2_objectsToActivate)
        {
            go.SetActive(false);
        }

        Color col = bg.color;
        col.a = 0;
        bg.color = col;

        textBox.SetActive(false);

        CratesManager.Instance.SpawnCrate(new Vector3(31, 35), 100);
        lowerLeftInstructions.SetActive(true);
        lowerLeftInstructionsText.text = messages[3];

        GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions().Enable();

        yield return new WaitForSeconds(0.25f);
        pascal.transform.parent = pascalDownLocation.transform;

        Vector3 initPos = pascal.transform.localPosition;
        Vector3 initScale = pascal.transform.localScale;

        LeanTween.value(pascal, 0, 1, 2.5f).setOnUpdate(val =>
        {
            pascal.transform.localPosition = initPos + (Vector3.zero - initPos) * val;
            pascal.transform.localScale = initScale + (Vector3.one - initScale) * val;
        });

        while (!enteredJumpCollider)
        {
            yield return null;
        }

        SetStage5();
        yield return null;
    }

    private void SetStage5()
    {
        idx = 5;

        Vector3 initScale = lowerLeftInstructions.transform.localScale;
        LeanTween.scale(lowerLeftInstructions, Vector3.zero, 0.5f).setOnComplete(() =>
        {
            lowerLeftInstructionsText.text = messages[4];
            LeanTween.scale(lowerLeftInstructions, initScale, 0.5f);
        });

        PlayerManager.Instance.onHealthUpdated += SetStage6;
    }

    private void SetStage6(int newHP)
    {
        idx = 6;
        PlayerManager.Instance.onHealthUpdated -= SetStage6;

        Color col = bg.color;
        col.a = .5f;
        bg.color = col;

        pascal.transform.parent = pascalMainLocation.transform;
        Vector3 initPos = pascal.transform.localPosition;
        Vector3 initScale = pascal.transform.localScale;

        LeanTween.value(pascal, 0, 1, 2f).setOnUpdate(val =>
        {
            pascal.transform.localPosition = initPos + (Vector3.zero - initPos) * val;
            pascal.transform.localScale = initScale + (Vector3.one - initScale) * val;
        });

        LeanTween.scale(lowerLeftInstructions, Vector3.zero, 0.5f).setOnComplete(() =>
        {
            lowerLeftInstructions.SetActive(false);

            textBox.SetActive(true);
            textBox.transform.localScale = Vector3.zero;
            textBoxContent.text = messages[5];
            LeanTween.scale(textBox, Vector3.one, .5f);
        });

    }

    private void SetStage7()
    {
        dummy.onDummyHit -= SetStage9;
        dummy.onDummyMiss -= SetStage7;

        textBox.SetActive(false);
        LeanTween.scale(pascal, Vector3.zero, 2f).setEaseInBack().setOnComplete(()=>
        {

            GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions().Enable();
            Color col = bg.color;
            col.a = .5f;
            bg.color = col;

            upperRightInstructions.transform.localScale = Vector3.zero;
            upperRightInstructionsText.text = messages[6];
            upperRightInstructions.SetActive(true);

            LeanTween.scale(upperRightInstructions, Vector3.one, 1f).setEaseOutBack();
            pascal.transform.parent = pascalSecondaryPosition.transform;
            pascal.transform.localPosition = Vector3.zero;
            LeanTween.scale(pascal, Vector3.one, 1f).setEaseOutBack();


            arrow_stage7.SetActive(true);
            playerActionsBar.EnableWeaponsBar();
            
            oldParentWeapon0 = weapons[0].transform.parent;
            weapons[0].transform.parent = overlayCanvas;

            PlayerActionsBar.WeaponIndexUpdated += SetStage8;
        });

    }

    private void SetStage8(int weaponIdx)
    {
        PlayerActionsBar.WeaponIndexUpdated -= SetStage8;

        Color col = bg.color;
        col.a = 0;
        bg.color = col;

        weapons[0].transform.parent = oldParentWeapon0;
        arrow_stage7.SetActive(false);

        upperRightInstructionsText.text = messages[7];

        dummy.EnableDummy();
        dummy.onDummyHit += SetStage9;
        dummy.onDummyMiss += SetStage7;
    }


    private void SetStage9()
    {
        dummy.onDummyHit -= SetStage9;
        dummy.onDummyHit -= SetLastStage;
        dummy.onDummyMiss -= SetStage7;
        dummy.onDummyMiss -= SetStage9;

        StartCoroutine(Stage9Coroutine());
    }

    private IEnumerator Stage9Coroutine()
    {
        yield return new WaitForSeconds(2f);
        upperRightInstructionsText.text = messages[8];

        Color col = bg.color;
        col.a = 0.5f;
        bg.color = col;


        oldParentWeapon0 = weapons[0].transform.parent;
        for(int i=1; i<weapons.Count; i++)
        {
            weapons[i].transform.parent = overlayCanvas;
        }

        PlayerActionsBar.WeaponIndexUpdated += SetStage9Phase2;
    }

    private void SetStage9Phase2(int idx)
    {
        PlayerActionsBar.WeaponIndexUpdated -= SetStage9Phase2;

        GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions().Enable();

        Color col = bg.color;
        col.a = 0f;
        bg.color = col;

        for (int i = 1; i < weapons.Count; i++)
        {
            weapons[i].transform.parent = oldParentWeapon0;
        }

        dummy.onDummyHit += SetLastStage;
        dummy.onDummyMiss += SetStage9;
    }

    private void SetLastStage()
    {
        Color col = bg.color;
        col.a = 0f;
        bg.color = col;

        upperRightInstructions.gameObject.SetActive(false);

        upperLeftCloseableInstructions.gameObject.SetActive(true);
        upperLeftCloseableInstructionsText.text = messages[9];
        
        dummy.onDummyHit -= SetLastStage;
        bg.raycastTarget = false;

        finished = true;
        RoomStateManager.Instance.SetState(new MyTurnState());

        dummy.onDummyHit += dummy.Relocate;
    }

    public void OnCloseUpperLeft()
    {
        upperLeftCloseableInstructions.SetActive(false);
        SetTutorialGameObjectsVisible(false);
    }

    private void SetTutorialGameObjectsVisible(bool value)
    {
        foreach (GameObject go in tutorialGameObjects)
        {
            go.SetActive(value);
        }
    }

    public void OnEnteredJumpCollider(Collider2D collider)
    {
        if(collider.gameObject.layer == 8)
        {
            enteredJumpCollider = true;
        }
    }
}

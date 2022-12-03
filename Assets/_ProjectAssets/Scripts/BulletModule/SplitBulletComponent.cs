using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SplitBulletComponent : BulletComponent
{
    public int splitNumber = 3;
    public GameObject bulletPrefab;
    private GameInputActions.WeaponsActions playerActions;

    private List<GameObject> launchedRockets;

    protected override void HandleStart()
    {
        if (!isMultiplayer || photonView.IsMine)
        {
            playerActions = GameInputManager.Instance.GetWeaponsActionMap().GetWeaponsInputActions();
            playerActions.MainAction.performed += Split;
        }
    }

    private void OnDestroy()
    {
        if (!isMultiplayer || photonView.IsMine)
        {
            playerActions.MainAction.performed -= Split;
        }
    }

    public void Split(InputAction.CallbackContext args)
    {
        launchedRockets = new List<GameObject>();
        for (int i = 0; i < splitNumber; i++)
        {
            GameObject obj = SingleAndMultiplayerUtils.Instantiate("Bullets/" + bulletPrefab.name, transform.position, Quaternion.Euler(transform.rotation.eulerAngles));

            var bullet = obj.GetComponent<BulletComponent>();
            bullet.hasEnabledPositionTracking = false;
            obj.transform.parent = transform;

            launchedRockets.Add(obj);

            //Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            //rb.isKinematic = false;
            //rb.velocity = GetComponent<Rigidbody2D>().velocity;
        }

        GetComponent<Animator>().SetTrigger("Shoot");
    }

    public void AfterSplit()
    {
        float deviation = 10;
        for (int i = 0; i < splitNumber; i++)
        {

            float angle = deviation * (i - splitNumber / 2);
            Vector2 dir = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1)) * rb.velocity.normalized;
            launchedRockets[i].transform.parent = null;
            launchedRockets[i].GetComponent<BulletComponent>().Launch(dir, rb.velocity.magnitude);
        }

        SingleAndMultiplayerUtils.Destroy(gameObject);
    }
}

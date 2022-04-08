using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerMove : MonoBehaviour
{
    public float force;

    private Rigidbody2D _rigidbody;
    private PhotonView _photonView;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _photonView = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (_photonView.IsMine && PhotonNetwork.IsConnected)
        {
            float h = Input.GetAxis("Horizontal");
            _rigidbody.AddForce(new Vector2(h * force, 0));
        }
    }
}

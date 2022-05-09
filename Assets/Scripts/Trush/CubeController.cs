using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System;

public class CubeController : MonoBehaviour
{
    RaycastHit hit;
    PhotonView pw;
    int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        pw = GetComponent<PhotonView>();
        if (pw.IsMine)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pw.IsMine)
        {
            MoveCube();
            Shoot();
        }
    }

    void MoveCube()
    {
        float X = Input.GetAxis("Horizontal") * Time.deltaTime * 30f;
        float Y = Input.GetAxis("Vertical") * Time.deltaTime * 30f;
        transform.Translate(X, 0, Y);
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
            {
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("Kill", RpcTarget.All, 10);
            }
        }

    }
    [PunRPC]
    public void Kill(int healthValue)
    {
        health -= healthValue;
        Debug.Log(health);
        if (health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }
}

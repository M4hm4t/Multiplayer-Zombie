using Code;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    PlayerController controller;
    public bool isShield=false;
    PhotonView view;
   // public bool hasShield = false;
    public GameObject playerShield;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        controller = GetComponent<PlayerController>();
    }

    [PunRPC]
    public void SetShieldSkill()
    {

        if (!isShield)
        {
            playerShield.SetActive(true);
            Invoke("DeactiveShieldSkill", 3f);
            isShield = true;
        }

    }

    public void ShieldSkill()
    {
        view.RPC("SetShieldSkill", RpcTarget.All);
    }

    public void DeactiveShieldSkill()
    {
        playerShield.SetActive(false);
        isShield = false;
    }

    

}

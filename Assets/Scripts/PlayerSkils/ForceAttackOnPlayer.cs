using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code;
using Photon.Pun;

public class ForceAttackOnPlayer : MonoBehaviour
{
    PlayerController controller;
    PhotonView view;

    public bool canForce = true;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        controller = GetComponent<PlayerController>();
    }
    [PunRPC]
    public void SetForceSkill()
    {
        if (canForce)
        {
            var forcePreb = Resources.Load<ForceAttack>("Force");
            var forceAttack = Instantiate<ForceAttack>(forcePreb, controller.transform.position, Quaternion.identity);
            forceAttack.SetSkillController(this);
            canForce = false;
        }
    }

    public void ForceSkill ()
    {
        view.RPC("SetForceSkill", RpcTarget.All);
    }
}

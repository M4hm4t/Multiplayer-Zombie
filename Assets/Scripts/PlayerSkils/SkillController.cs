using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code;
using Photon.Pun;

public class SkillController : Singleton<SkillController>
{
    PlayerController controller;
   
    public void SetPlayer(PlayerController controller)
    {
        this.controller = controller;
    }



    public void DashBtn()
    {
        controller.GetComponent<Dash>().DashSkill();
    }
    public void ShildBtn()
    {
        controller.GetComponent<Shield>().ShieldSkill();
       
    }
    public void ForceAttackBtn()
    {
        controller.GetComponent<ForceAttackOnPlayer>().ForceSkill();
    }

}

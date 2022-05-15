using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code;
public class SkillController : MonoBehaviour
{
    PlayerController controller;


    public void SetPlayer(PlayerController controller)
    {
        this.controller = controller;
    }

    //public void SetPlayerShield(PlayerController controller)
    //{
    //    this.controller = controller;
    //}
    public void DashBtn()
    {
        controller.GetComponent<Dash>().DashSkill();
    }
    public void ShildBtn()
    {
        controller.GetComponent<Shield>().ShieldSkill();
    }
   
}

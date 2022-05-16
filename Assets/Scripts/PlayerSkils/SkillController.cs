using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code;
public class SkillController : Singleton<SkillController>
{
    PlayerController controller;
    public bool canForce = true;
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
        if (canForce)
        {
            var forcePreb = Resources.Load<ForceAttack>("Force");
            var forceAttack = Instantiate<ForceAttack>(forcePreb, controller.transform.position, Quaternion.identity);
            forceAttack.SetSkillController(this);
            canForce = false;
        }

    }

}

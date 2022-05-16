using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code;
using TMPro;
public class Dash : MonoBehaviour
{
    Vector3 dashPos;
    //public const float maxDashTime = 1.0f;
    public float dashDistance = 10;
    public float dashStoppingSpeed = 0.1f;
    //float currentDashTime = maxDashTime;
    float dashSpeed = 6;
    CharacterController controller;
    public bool canDash = true;
    public bool isDashing = false;
    public Transform PlayerTransform;




    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DashSkill();
        }
        if (Vector3.Distance(transform.position,dashPos)>1 && isDashing)
        {
            
            var moveDirection = dashPos - transform.position;
            controller.Move(moveDirection * Time.deltaTime * dashSpeed);
        }else
        {
            isDashing = false;
        }
    }
    public void SetDash()
    {
        isDashing=false;
        canDash = true;
    }

    public void DashSkill()
    {
        if (canDash)
        {
            isDashing=true;
            dashPos = transform.position + transform.forward * dashDistance;
            //currentDashTime = 0;
            canDash = false;
            Invoke("SetDash", 2f);
        }

    }

   
}

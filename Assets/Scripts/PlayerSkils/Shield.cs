using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    PlayerController controller;
    public bool isShield=false;
   // public bool hasShield = false;
    public GameObject playerShield;


    

    public void ShieldSkill()
    {
        if (!isShield)
        {
            playerShield.SetActive(true);
            Invoke("DeactiveShieldSkill", 3f);
            isShield = true;
        }

    }

    public void DeactiveShieldSkill()
    {
        playerShield.SetActive(false);
        isShield = false;
    }

    

}

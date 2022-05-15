using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    PlayerController controller;

    public GameObject PlayerShield;
   


    public void Awake()
    {

        //SetActiveShield();

    }
    private void Start()
    {
        
        //SetInactiveShield();
    }
    public void SetShield(GameObject playershield)
    {
        PlayerShield = playershield;
    }
    public void ShieldSkill()
    {
        PlayerShield.transform.GetChild(6).gameObject.SetActive(true);
        //PlayerShield.SetActive(false);
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    PlayerShield.transform.GetChild(4).gameObject.SetActive(true);
        //}

        // SetActiveShield();
    }

    //public void SetActiveShield ()
    //{
    //    GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
    //    foreach (GameObject go in objects)
    //    {
    //        if (go.name.Equals("PlayerShield"))
    //        {
    //            go.SetActive(true);
    //        }
    //    }
    //} 
    //public void SetInactiveShield ()
    //{
    //    GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
    //    foreach (GameObject go in objects)
    //    {
    //        if (go.name.Equals("PlayerShield"))
    //        {
    //            go.SetActive(false);
    //        }
    //    }
    //}


}

using Code;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyKillsText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SetEnemyKillText(this.GetComponent<TextMeshProUGUI>());
    }

}

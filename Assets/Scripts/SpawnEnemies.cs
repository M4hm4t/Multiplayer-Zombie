using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Code;

public class SpawnEnemies : MonoBehaviourPunCallbacks
{
    private PhotonView _view;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SEnemies();
        }
   
    }

   void SEnemies()
    {
        GameObject enemy = PhotonNetwork.Instantiate(enemyPrefab.name, enemySpawnPoint.position, enemySpawnPoint.rotation);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

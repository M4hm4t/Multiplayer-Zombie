using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;


public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Transform spawnPoint;
    PlayerFollow playerFollow;
    // Start is called before the first frame update
    private void Awake()
    {
        playerFollow = FindObjectOfType<PlayerFollow>();
    }
    private void Start()
    {
        SpawnPlayer();
    }
    void SpawnPlayer()
    {
        var newSpawnPos = spawnPoint.position + Random.onUnitSphere * 3;
        newSpawnPos.y = spawnPoint.position.y;
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, newSpawnPos, spawnPoint.rotation);
       // playerFollow.SetCameraTarget(player.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

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
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
        playerFollow.SetCameraTarget(player.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

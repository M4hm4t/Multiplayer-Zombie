using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Code;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviourPunCallbacks
{
    private static MatchManager instance;
    private int _enemyKillCount;
    private Player masterClientPlayer;



    public static MatchManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MatchManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(MatchManager).Name;
                    instance = obj.AddComponent<MatchManager>();
                }
            }
            return instance;
        }
    } //singelton


    private void Start()
    {

        SetEnemies();
    }

    [PunRPC]
    public void Display(float normalized)
    {
        BarController.Instance.Display(normalized);
    }
    public void SetDisplay(float normalized)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GetComponent<PhotonView>().RPC("Display", RpcTarget.AllBuffered, normalized);
        }
    }
    public void SetEnemies()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Enemy");
            Enemies();
        }
    }
    [PunRPC]
    public void Enemies()
    {
        for (int i = 0; i < 30; i++)
        {
            var pos = Random.insideUnitSphere * 30;
            pos.y = 1;
            var enemies = PhotonNetwork.InstantiateRoomObject("Enemy", pos, Quaternion.identity);// resorstan enemiyi herkes icin al?r
        }
    }
    public override void OnLeftRoom()
    {

        SceneManager.LoadScene("Lobby");
    }


    [PunRPC]
    public void EnemyKill(int enemyKillCount)
    {
        _enemyKillCount = enemyKillCount;
        GameManager.Instance.EnemyKilled(_enemyKillCount);
    }
    public void SetEnemyKill()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _enemyKillCount++;
            GetComponent<PhotonView>().RPC("EnemyKill", RpcTarget.AllBuffered, _enemyKillCount);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class NetManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Baglandi");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Lobiye Girildi");
       PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
       //PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Odaya girildi");
        GameObject obj = PhotonNetwork.Instantiate("Cube", Vector3.zero, Quaternion.identity, 0, null);
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Odadan cikildi");
    }
    public override void OnLeftLobby()
    {
        Debug.Log("Lobiden cikildi");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Error: Odaya girilemedi");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Error: Herhangi bir odaya girilemedi");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Error: Oda kurulamadi");
    }
    
}

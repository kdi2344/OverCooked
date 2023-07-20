using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class IngamePhtonManager : MonoBehaviourPunCallbacks
{
    PhotonView pv;
    private Hashtable CP;
    public Transform[] spawnPoints;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        
    }
    private void Start()
    {
        //플레이어 생성
        Vector3 pos = spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].position;
        Quaternion rot = spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].rotation;

        GameObject playerTemp = PhotonNetwork.Instantiate("Player", pos, rot, 0);
        CP = PhotonNetwork.LocalPlayer.CustomProperties;
        int colorNum = (int)CP["Color"];
        //Debug.Log("colornum " + colorNum);
        //PhotonNetwork.CurrentRoom.Players[PhotonNetwork.CurrentRoom.PlayerCount - 1].SetCustomProperties(new Hashtable { { "Color", PhotonNetwork.CurrentRoom.PlayerCount } });
        if (colorNum != -1)
        {
            //Debug.Log("Color Custom Properties = " + colorNum);
            playerTemp.GetComponent<PlayerController>().InitColor(colorNum);
        }
    }
}

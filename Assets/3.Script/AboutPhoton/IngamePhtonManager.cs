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
        if (!SoundManager.instance.isSingle)
        {
            CP = PhotonNetwork.LocalPlayer.CustomProperties;
            int colorNum = (int)CP["Color"];
            Vector3 pos = spawnPoints[colorNum].position;
            Quaternion rot = spawnPoints[colorNum].rotation;

            GameObject playerTemp = PhotonNetwork.Instantiate("Player", pos, rot, 0);
            Debug.Log(playerTemp.name);
            if (colorNum != -1)
            {
                playerTemp.GetComponent<PlayerController>().InitColor(colorNum);
            }

            PlayerController[] players = FindObjectsOfType<PlayerController>();
            if (players.Length > PhotonNetwork.CurrentRoom.PlayerCount)
            {
                PhotonNetwork.Destroy(players[players.Length - 1].GetComponent<PhotonView>());
            }
        }
        
        
    }
}

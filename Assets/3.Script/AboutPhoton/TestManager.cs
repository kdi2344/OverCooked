using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TestManager : MonoBehaviour
{
    public static TestManager instance = null;

    public PhotonView pv;

    private Hashtable CP;

    public bool isConnect = false;
    public Transform[] spawnPoints;
    private GameObject playerTemp;

    //public Sprite[] materials;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        CP = PhotonNetwork.LocalPlayer.CustomProperties;
        //StartCoroutine(CreatePlayer());
        CreatePlayer();
    }

    public void MoveMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Lobby");
        }
    }

    //[PunRPC]
    //private void SetPlayer()
    //{
    //    //playerTemp.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    //    playerTemp.GetComponent<PlayerController>().Speed = 5;
    //}

    private void CreatePlayer()
    {
        spawnPoints = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        if (!SoundManager.instance.alreadyPlayed)
        {
            Vector3 pos = spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount].position;
            Quaternion rot = spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount].rotation;

            playerTemp = PhotonNetwork.Instantiate("LobbyPlayer", pos, rot, 0);
            int colorNum = PhotonNetwork.CurrentRoom.PlayerCount;
            PhotonNetwork.CurrentRoom.Players[PhotonNetwork.CurrentRoom.PlayerCount].SetCustomProperties(new Hashtable { { "Color", PhotonNetwork.CurrentRoom.PlayerCount } });
            if (colorNum != -1)
            {
                playerTemp.GetComponent<LobbyPlayerController>().InitColor(colorNum - 1);
            }
        }
        else
        {
            Vector3 pos = spawnPoints[(int)PhotonNetwork.LocalPlayer.CustomProperties["Color"]+1].position;
            Quaternion rot = spawnPoints[(int)PhotonNetwork.LocalPlayer.CustomProperties["Color"]+1].rotation;
            playerTemp = PhotonNetwork.Instantiate("LobbyPlayer", pos, rot, 0);
            playerTemp.GetComponent<LobbyPlayerController>().InitColor((int)PhotonNetwork.LocalPlayer.CustomProperties["Color"]);
        }
    }
}

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

        Vector3 pos = spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount].position;
        Quaternion rot = spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount].rotation;

        playerTemp = PhotonNetwork.Instantiate("LobbyPlayer", pos, rot, 0);
        //pv.RPC(nameof(SetPlayer), RpcTarget.AllViaServer);

        int colorNum = PhotonNetwork.CurrentRoom.PlayerCount;
        //Debug.Log("colornum " + colorNum);
        PhotonNetwork.CurrentRoom.Players[PhotonNetwork.CurrentRoom.PlayerCount].SetCustomProperties(new Hashtable { { "Color", PhotonNetwork.CurrentRoom.PlayerCount } });
        if (colorNum != -1)
        {
            //Debug.Log("ColorNum = " + colorNum);
            playerTemp.GetComponent<LobbyPlayerController>().InitColor(colorNum-1);
        }
        
        
        //PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Color",  } });
        //CP = PhotonNetwork.LocalPlayer.CustomProperties;
        //playerTemp.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PlayerController>().colors[PhotonNetwork.CurrentRoom.PlayerCount-1];
        //Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

        //int colorNum = (int)CP["Color"];
        //Debug.Log($"colorNum : {colorNum}");
        //if (colorNum != -1)
        //{
        //    playerTemp.GetComponent<PlayerController>().InitColor(colorNum - 1);
        //}
    }
}

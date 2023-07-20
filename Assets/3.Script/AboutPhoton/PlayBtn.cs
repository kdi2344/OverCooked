using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayBtn : MonoBehaviour
{
    PhotonView pv;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }

    public void BtnPlay()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Map");
        }
        //FindObjectOfType<TestManager>().MoveMap();
    }
}

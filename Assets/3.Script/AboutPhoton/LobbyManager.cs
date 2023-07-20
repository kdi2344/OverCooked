using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviour
{
    private PhotonView pv;
    private Hashtable CP;

    //public Renderer player;
    //public Sprite[] playerMt;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Color", -1 } });
        CP = PhotonNetwork.LocalPlayer.CustomProperties;
        
    }
    //public void SetColorProperty(int num)
    //{
    //    CP["Color"] = num;
    //    //SetMT(num);
    //}
    //void SetMT(int num)
    //{
    //    player.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = playerMt[num - 1];
    //}
}

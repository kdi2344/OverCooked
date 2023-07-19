using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomData : MonoBehaviour
{
    private Text RoomInfoText;
    private RoomInfo roomInfo;
    public inputfield userIDText;

    public RoomInfo RoomInfo
    {
        get
        {
            return roomInfo;
        }
        set
        {
            roomInfo = value;
            //room_03 (1/2)
            RoomInfoText.text = $"{roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})";
            GetComponent<Button>().onClick.AddListener(() => OnEnterRoom(roomInfo.Name));
        }
    }
    private void Awake()
    {
        RoomInfoText = GetComponentInChildren<Text>();
        userIDText = GameObject.Find("EnterNickname").GetComponent<inputfield>();
    }
    private void OnEnterRoom(string roomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;
        PhotonNetwork.NickName = userIDText.transform.GetChild(1).GetComponent<Text>().text; //틀릴수도 있음
        PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
    }

}

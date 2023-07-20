using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomData : MonoBehaviour
{
    public Text RoomInfoText;
    public RoomInfo _roomInfo;
    public Text userIDText;

    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            //room_03 (1/2)
            RoomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
            GetComponent<Button>().onClick.AddListener(() => OnEnterRoom(_roomInfo.Name));
        }
    }
    private void Awake()
    {
        //RoomInfoText = GetComponentInChildren<Text>();
        RoomInfoText = transform.GetChild(0).GetComponent<Text>();
        userIDText = GameObject.FindGameObjectWithTag("nickname").transform.GetChild(2).GetComponent<Text>();
        SetText();
        
        GetComponent<Button>().onClick.AddListener(() => OnEnterRoom(_roomInfo.Name));
        //userIDText = GameObject.Find("EnterNickname").GetComponent<inputfield>();
        //Debug.Log("RoomInfoText = "+ RoomInfoText.transform.parent.name);
        //Debug.Log(RoomInfoText.transform.parent.name);
    }
    public void SetText()
    {
        if (_roomInfo != null)
        {
            RoomInfoText = transform.GetChild(0).GetComponent<Text>();
            //userIDText = GameObject.FindGameObjectWithTag("nickname").transform.GetChild(2).GetComponent<Text>();
            RoomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
        }
    }
    private void OnEnterRoom(string roomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;
        userIDText = GameObject.FindGameObjectWithTag("nickname").transform.GetChild(2).GetComponent<Text>();
        PhotonNetwork.NickName = userIDText.text; //틀릴수도 있음
        Debug.Log("OnEnterRoom Nickname : " + PhotonNetwork.NickName);
        PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
    }

}

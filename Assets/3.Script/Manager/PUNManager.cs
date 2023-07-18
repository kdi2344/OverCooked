using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PUNManager : MonoBehaviourPunCallbacks
{
    //서버 접속부터 Master Server -> Lobby -> Room
    private readonly string gameversion = "1";
    public ServerSettings setting = null;
    [SerializeField] Button ServerBtn;

    public InputField NickName_i; //가지고 있다가 플레이어가 생성되면 부여

    //플레이어 프리팹
    public GameObject playerPrefabs;

    private void Start()
    {
        Connect();
    }
    private void OnApplicationQuit()
    {
        Disconnect();
    }
    #region 서버 관련 콜백 함수들
    //ConnecttoMaster
    public void Connect()
    {
        PhotonNetwork.GameVersion = gameversion;
        //setting.AppSettings.Server = DBManager.instance.ServerIP;

        //Master 서버에 연결
        PhotonNetwork.ConnectToMaster(setting.AppSettings.Server, setting.AppSettings.Port, "");
        Debug.Log("Connect to Master Server");
        ServerBtn.onClick.AddListener(JoinRandomRoomORCreateRoom); //onClick 이벤트 넣기
    }
    public void JoinRandomRoomORCreateRoom() //랜덤 룸에 들어가거나 없으면 만들기
    {
        if (NickName_i.text.Equals(string.Empty))
        {
            Debug.Log("Empty");
            return;
        }
        string nickname = NickName_i.text;
        Debug.Log($"{nickname} Start Random Matching");
        PhotonNetwork.LocalPlayer.NickName = nickname;

        RoomOptions option = new RoomOptions();

        option.MaxPlayers = 2;
        //실질적으로 사용할 커스텀 프로퍼티 객체 생성
        //option.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        //{
        //    { "MaxTime", i_maxtime}
        //};
        ////로비에서 커스텀 프로퍼티 등록 -> 게임에서 필터링 가능
        //option.CustomRoomPropertiesForLobby = new string[] { "MaxTime" };

        //방 참가 시도하고 실패하면 생성해서 방에 참가해야함
        PhotonNetwork.JoinRandomOrCreateRoom
        (
            expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "MaxTime", option.CustomRoomProperties["MaxTime"] } },
            expectedMaxPlayers: (byte)option.MaxPlayers,  //안될 수 있음 유의
            roomOptions: option
        );
        //btn.onClick.RemoveAllListeners();
        ServerBtn.transform.GetChild(0).GetComponent<Text>().text = "Cancel";
        ServerBtn.onClick.AddListener(CancelMatching);
    }
    public void CancelMatching()
    {
        Debug.Log("Cancel Matching");
        Debug.Log("Left Room");
        PhotonNetwork.LeaveRoom();
        ServerBtn.onClick.RemoveAllListeners();
        ServerBtn.transform.GetChild(0).GetComponent<Text>().text = "Join";
        ServerBtn.onClick.AddListener(JoinRandomRoomORCreateRoom);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect(); //포톤 서버와 연결 끊기
    }

    //콜백 함수
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connect to Master Server");
        PhotonNetwork.JoinLobby(); //대기실로 이동
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Enter to Lobby");
        base.OnJoinedLobby();
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("No empty Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 5 });
        Debug.Log("Make Room");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room");
        //PhotonNetwork.Instantiate(playerPrefabs.name, Vector3.zero, Quaternion.identity);
        UpdatePlayer();
    }

    public void UpdatePlayer()
    {
        //UserCountText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        //이벤트 -> 현재 방에 있는 Player의 수와 max 플레이어가 같다면 실질적인 플레이를 생성하고, 플레이를 가능하게 만듦
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.Instantiate(playerPrefabs.name, Vector3.zero, Quaternion.identity);
            //MatchPanel.SetActive(false);
        }
    }

    #endregion
}
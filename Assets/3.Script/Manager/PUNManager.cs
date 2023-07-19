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
    public ServerSettings setting = null; //서버 세팅 끌어서 연결
    private string userId = "dan";

    [SerializeField] Button ServerBtn;

    //public InputField NickName_i; //가지고 있다가 플레이어가 생성되면 부여
    public InputField userID_i;
    public InputField roomname_i;

    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    public GameObject roomPrefab;
    public Transform scrollContent;

    //플레이어 프리팹
    public GameObject playerPrefabs;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameversion;
    }

    private void Start()
    {
        Debug.Log("PUN 매니저 시작");
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(0, 100):00}");
        userID_i.text = userId;
        PhotonNetwork.NickName = userId;
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
        //setting.AppSettings.Server = DBManager.instance.ServerIP;

        //Master 서버에 연결
        PhotonNetwork.ConnectToMaster(setting.AppSettings.Server, setting.AppSettings.Port, "");
        Debug.Log("Connect to Master Server");
        //ServerBtn.onClick.AddListener(JoinRandomRoomORCreateRoom); //onClick 이벤트 넣기
    }
    public void Disconnect()
    {
        PhotonNetwork.Disconnect(); //포톤 서버와 연결 끊기
    }

    public void JoinRandomRoomORCreateRoom() //랜덤 룸에 들어가거나 없으면 만들기
    {
        if (userID_i.text.Equals(string.Empty))
        {
            Debug.Log("Empty");
            return;
        }
        string nickname = userID_i.text;

        Debug.Log($"{nickname} Start Random Matching");
        PhotonNetwork.LocalPlayer.NickName = nickname;

        //string maxplayer_s = MaxPlayer_i.text;
        //string maxtime_s = MaxTime_i.text;

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
            expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "MaxTime", 100 } },
            expectedMaxPlayers: (byte)2,  //안될 수 있음 유의
            roomOptions: option
        );
        //btn.onClick.RemoveAllListeners();
        //ServerBtn.transform.GetChild(0).GetComponent<Text>().text = "Cancel";
        //ServerBtn.onClick.AddListener(CancelMatching);
    }
    public void CancelMatching()
    {
        Debug.Log("Cancel Matching");
        Debug.Log("Left Room");
        PhotonNetwork.LeaveRoom();
        ServerBtn.onClick.RemoveAllListeners();
        //ServerBtn.transform.GetChild(0).GetComponent<Text>().text = "Join";
        ServerBtn.onClick.AddListener(JoinRandomRoomORCreateRoom);
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
        //PhotonNetwork.JoinRandomRoom();
    }

    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    base.OnJoinRandomFailed(returnCode, message);
    //    Debug.Log("No empty Room");
    //    PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 5 });
    //    Debug.Log("Make Room");
    //}

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room");
        //PhotonNetwork.Instantiate(playerPrefabs.name, Vector3.zero, Quaternion.identity);
        UpdatePlayer();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Lobby");
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //어떤 new Player가 들어왔을때 콜백되는 함수
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"{newPlayer.NickName} Entered Room");
        UpdatePlayer();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //어떤 player가 나갔을때 
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"{otherPlayer.NickName} Left Room");
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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;
        foreach(var room in roomList)
        {
            //룸이 삭제된 경우
            if (room.RemovedFromList)
            {
                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                roomDict.Remove(room.Name);
            }
            //룸 정보가 변경된 경우
            else
            {
                //룸 처음 생성
                if (!roomDict.ContainsKey(room.Name))
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    _room.GetComponent<RoomData>().RoomInfo = room;
                    roomDict.Add(room.Name, _room);
                }
                else //룸 정보 갱신
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;
                }
            }
        }
    }

    public void BtnRandom()
    {
        if (string.IsNullOrEmpty(userID_i.text))
        {
            //랜덤아이디 주기
            userId = $"USER_{Random.Range(0, 100):00}";
            userID_i.text = userId;
        }
        //PlayerPrefs.SetString("USER_ID", userID_i.text);
        //PhotonNetwork.NickName = userID_i.text;
        PhotonNetwork.JoinRandomRoom();
    }

    public void BtnMakeRoom()
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;
        //인풋필드가 비어있으면
        if (string.IsNullOrEmpty(roomname_i.text))
        {
            //랜덤 룸 이름
            roomname_i.text = $"ROOM_{Random.Range(1, 100):000}";
        }
        PhotonNetwork.CreateRoom(roomname_i.text, ro);
    }

    #endregion
}
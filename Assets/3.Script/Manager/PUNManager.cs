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
        Connect();
    }

    private void Start()
    {
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(0, 100):00}");
        userID_i.text = userId;
        PhotonNetwork.NickName = userId;
    }
    private void OnApplicationQuit()
    {
        Disconnect();
    }
    #region 서버 관련 콜백 함수들
    public void Connect()
    {
        //Master 서버에 연결
        PhotonNetwork.ConnectToMaster(setting.AppSettings.Server, setting.AppSettings.Port, "");
        Debug.Log("Connect to Master Server");
    }
    public void Disconnect()
    {
        PhotonNetwork.Disconnect(); //포톤 서버와 연결 끊기
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("랜덤 룸 접속 실패");
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;
        roomname_i.text = $"Room_{Random.Range(1, 100):000}";
        PhotonNetwork.CreateRoom("room_1", ro);
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

        RoomOptions option = new RoomOptions();

        option.MaxPlayers = 2;

        PhotonNetwork.JoinRandomOrCreateRoom
        (
            expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "MaxTime", 100 } },
            expectedMaxPlayers: (byte)2, 
            roomOptions: option
        );
    }
    public void CancelMatching()
    {
        Debug.Log("Cancel Matching");
        Debug.Log("Left Room");
        PhotonNetwork.LeaveRoom();
        ServerBtn.onClick.RemoveAllListeners();
        ServerBtn.onClick.AddListener(JoinRandomRoomORCreateRoom);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 완료");
    }

    //콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect to Master Server");
        PhotonNetwork.JoinLobby(); //대기실로 이동
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Enter to Lobby");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");
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
        for (int i = 0; i < roomList.Count; i++)
        //foreach(var room in roomList)
        {
            var room = roomList[i];
            //룸이 삭제된 경우
            if (room.RemovedFromList == true)
            {
                Debug.Log("삭제");
                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                roomDict.Remove(room.Name);
            }
            //룸 정보가 변경된 경우
            else
            {
                Debug.Log("변경");
                //룸 처음 생성
                if (roomDict.ContainsKey(room.Name) == false)
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    _room.GetComponent<RoomData>()._roomInfo = room;
                    _room.GetComponent<RoomData>().SetText();
                    roomDict.Add(room.Name, _room);
                }
                else //룸 정보 갱신
                {
                    Debug.Log("정보 갱신");
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;
                    tempRoom.GetComponent<RoomData>().SetText();
                }
            }
        }
    }
    public void BtnFight()
    {
        if (userID_i.transform.childCount > 2) userID_i.transform.GetChild(2).GetComponent<Text>().text = string.Empty;
    }

    public void BtnRandom()
    {
        if (string.IsNullOrEmpty(userID_i.text))
        {
            //랜덤아이디 주기
            userId = $"USER_{Random.Range(0, 100):00}";
            userID_i.text = userId;
        }
        PlayerPrefs.SetString("USER_ID", userID_i.text);
        PhotonNetwork.NickName = userID_i.text;
        PhotonNetwork.JoinRandomRoom();
    }

    public void BtnMakeRoom()
    {
        if (string.IsNullOrEmpty(userID_i.text))
        {
            //랜덤아이디 주기
            userId = $"USER_{Random.Range(0, 100):00}";
            userID_i.text = userId;
        }
        PlayerPrefs.SetString("USER_ID", userID_i.text);
        PhotonNetwork.NickName = userID_i.text;
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
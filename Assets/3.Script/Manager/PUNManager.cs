using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PUNManager : MonoBehaviourPunCallbacks
{
    //���� ���Ӻ��� Master Server -> Lobby -> Room
    private readonly string gameversion = "1";
    public ServerSettings setting = null; //���� ���� ��� ����
    private string userId = "dan";

    [SerializeField] Button ServerBtn;

    public InputField userID_i;
    public InputField roomname_i;

    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    public GameObject roomPrefab;
    public Transform scrollContent;

    //�÷��̾� ������
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
    #region ���� ���� �ݹ� �Լ���
    public void Connect()
    {
        //Master ������ ����
        PhotonNetwork.ConnectToMaster(setting.AppSettings.Server, setting.AppSettings.Port, "");
        Debug.Log("Connect to Master Server");
    }
    public void Disconnect()
    {
        PhotonNetwork.Disconnect(); //���� ������ ���� ����
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("���� �� ���� ����");
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;
        roomname_i.text = $"Room_{Random.Range(1, 100):000}";
        PhotonNetwork.CreateRoom("room_1", ro);
    }

    public void JoinRandomRoomORCreateRoom() //���� �뿡 ���ų� ������ �����
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
        Debug.Log("�� ���� �Ϸ�");
    }

    //�ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect to Master Server");
        PhotonNetwork.JoinLobby(); //���Ƿ� �̵�
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Enter to Lobby");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� �Ϸ�");
        UpdatePlayer();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Lobby");
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //� new Player�� �������� �ݹ�Ǵ� �Լ�
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"{newPlayer.NickName} Entered Room");
        UpdatePlayer();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //� player�� �������� 
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"{otherPlayer.NickName} Left Room");
        UpdatePlayer();
    }

    public void UpdatePlayer()
    {
        //�̺�Ʈ -> ���� �濡 �ִ� Player�� ���� max �÷��̾ ���ٸ� �������� �÷��̸� �����ϰ�, �÷��̸� �����ϰ� ����
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
            //���� ������ ���
            if (room.RemovedFromList == true)
            {
                Debug.Log("����");
                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                roomDict.Remove(room.Name);
            }
            //�� ������ ����� ���
            else
            {
                Debug.Log("����");
                //�� ó�� ����
                if (roomDict.ContainsKey(room.Name) == false)
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    _room.GetComponent<RoomData>()._roomInfo = room;
                    _room.GetComponent<RoomData>().SetText();
                    roomDict.Add(room.Name, _room);
                }
                else //�� ���� ����
                {
                    Debug.Log("���� ����");
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
            //�������̵� �ֱ�
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
            //�������̵� �ֱ�
            userId = $"USER_{Random.Range(0, 100):00}";
            userID_i.text = userId;
        }
        PlayerPrefs.SetString("USER_ID", userID_i.text);
        PhotonNetwork.NickName = userID_i.text;
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;
        //��ǲ�ʵ尡 ���������
        if (string.IsNullOrEmpty(roomname_i.text))
        {
            //���� �� �̸�
            roomname_i.text = $"ROOM_{Random.Range(1, 100):000}";
        }
        PhotonNetwork.CreateRoom(roomname_i.text, ro);
    }

    #endregion
}
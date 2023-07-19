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

    //public InputField NickName_i; //������ �ִٰ� �÷��̾ �����Ǹ� �ο�
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
    }

    private void Start()
    {
        Debug.Log("PUN �Ŵ��� ����");
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(0, 100):00}");
        userID_i.text = userId;
        PhotonNetwork.NickName = userId;
        Connect();
    }
    private void OnApplicationQuit()
    {
        Disconnect();
    }
    #region ���� ���� �ݹ� �Լ���
    //ConnecttoMaster
    public void Connect()
    {
        //setting.AppSettings.Server = DBManager.instance.ServerIP;

        //Master ������ ����
        PhotonNetwork.ConnectToMaster(setting.AppSettings.Server, setting.AppSettings.Port, "");
        Debug.Log("Connect to Master Server");
        //ServerBtn.onClick.AddListener(JoinRandomRoomORCreateRoom); //onClick �̺�Ʈ �ֱ�
    }
    public void Disconnect()
    {
        PhotonNetwork.Disconnect(); //���� ������ ���� ����
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

        //string maxplayer_s = MaxPlayer_i.text;
        //string maxtime_s = MaxTime_i.text;

        RoomOptions option = new RoomOptions();

        option.MaxPlayers = 2;
        //���������� ����� Ŀ���� ������Ƽ ��ü ����
        //option.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        //{
        //    { "MaxTime", i_maxtime}
        //};
        ////�κ񿡼� Ŀ���� ������Ƽ ��� -> ���ӿ��� ���͸� ����
        //option.CustomRoomPropertiesForLobby = new string[] { "MaxTime" };

        //�� ���� �õ��ϰ� �����ϸ� �����ؼ� �濡 �����ؾ���
        PhotonNetwork.JoinRandomOrCreateRoom
        (
            expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "MaxTime", 100 } },
            expectedMaxPlayers: (byte)2,  //�ȵ� �� ���� ����
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

    //�ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connect to Master Server");
        PhotonNetwork.JoinLobby(); //���Ƿ� �̵�
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
        //UserCountText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
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
        foreach(var room in roomList)
        {
            //���� ������ ���
            if (room.RemovedFromList)
            {
                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                roomDict.Remove(room.Name);
            }
            //�� ������ ����� ���
            else
            {
                //�� ó�� ����
                if (!roomDict.ContainsKey(room.Name))
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    _room.GetComponent<RoomData>().RoomInfo = room;
                    roomDict.Add(room.Name, _room);
                }
                else //�� ���� ����
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
            //�������̵� �ֱ�
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
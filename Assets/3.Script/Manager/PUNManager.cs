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
    public ServerSettings setting = null;
    [SerializeField] Button ServerBtn;

    public InputField NickName_i; //������ �ִٰ� �÷��̾ �����Ǹ� �ο�

    //�÷��̾� ������
    public GameObject playerPrefabs;

    private void Start()
    {
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
        PhotonNetwork.GameVersion = gameversion;
        //setting.AppSettings.Server = DBManager.instance.ServerIP;

        //Master ������ ����
        PhotonNetwork.ConnectToMaster(setting.AppSettings.Server, setting.AppSettings.Port, "");
        Debug.Log("Connect to Master Server");
        ServerBtn.onClick.AddListener(JoinRandomRoomORCreateRoom); //onClick �̺�Ʈ �ֱ�
    }
    public void JoinRandomRoomORCreateRoom() //���� �뿡 ���ų� ������ �����
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
            expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "MaxTime", option.CustomRoomProperties["MaxTime"] } },
            expectedMaxPlayers: (byte)option.MaxPlayers,  //�ȵ� �� ���� ����
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
        PhotonNetwork.Disconnect(); //���� ������ ���� ����
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
        //�̺�Ʈ -> ���� �濡 �ִ� Player�� ���� max �÷��̾ ���ٸ� �������� �÷��̸� �����ϰ�, �÷��̸� �����ϰ� ����
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.Instantiate(playerPrefabs.name, Vector3.zero, Quaternion.identity);
            //MatchPanel.SetActive(false);
        }
    }

    #endregion
}
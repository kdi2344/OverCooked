using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PUNManager : MonoBehaviourPunCallbacks
{
    //���� ���Ӻ��� Master Server -> Lobby -> Room
    private readonly string gameversion = "1";
    public ServerSettings setting = null;

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
        PhotonNetwork.Instantiate(playerPrefabs.name, Vector3.zero, Quaternion.identity);
    }

    #endregion
}
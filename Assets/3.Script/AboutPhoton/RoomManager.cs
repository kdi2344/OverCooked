using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void CancelMatching()
    {
        Debug.Log("Cancel Matching");
        Debug.Log("Left Room");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LeaveRoom();
        }
        
        
        //ServerBtn.onClick.RemoveAllListeners();
        //ServerBtn.transform.GetChild(0).GetComponent<Text>().text = "Join";
        //ServerBtn.onClick.AddListener(JoinRandomRoomORCreateRoom);
    }
    public override void OnLeftRoom()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            SceneManager.LoadScene("IntroScene");
            return;
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
            Debug.Log("update player");
            //PhotonNetwork.Instantiate(playerPrefabs.name, Vector3.zero, Quaternion.identity);
            //MatchPanel.SetActive(false);
        }
    }
}

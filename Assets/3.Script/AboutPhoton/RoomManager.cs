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
            Debug.Log("update player");
            //PhotonNetwork.Instantiate(playerPrefabs.name, Vector3.zero, Quaternion.identity);
            //MatchPanel.SetActive(false);
        }
    }
}

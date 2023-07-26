using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ResultScript : MonoBehaviour
{
    [SerializeField] GameObject FirstLine;
    [SerializeField] GameObject SecondLine;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        FirstLine.transform.GetChild(0).GetComponent<Text>().text = PhotonNetwork.PlayerList[0].NickName + " 점수 ";
        FirstLine.transform.GetChild(1).GetComponent<Text>().text = PhotonNetwork.PlayerList[0].CustomProperties["OppositeMoney"].ToString();

        SecondLine.transform.GetChild(0).GetComponent<Text>().text = PhotonNetwork.PlayerList[1].NickName + " 점수 ";
        SecondLine.transform.GetChild(1).GetComponent<Text>().text = PhotonNetwork.PlayerList[1].CustomProperties["OppositeMoney"].ToString();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    PhotonNetwork.LoadLevel("Lobby");
        //}
    }
}

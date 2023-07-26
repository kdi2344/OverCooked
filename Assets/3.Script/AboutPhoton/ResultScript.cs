using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ResultScript : MonoBehaviour
{
    [SerializeField] GameObject FirstLine;
    [SerializeField] GameObject SecondLine;
    [SerializeField] Text Winner;
    //bool isGone = false;

    private void Awake()
    {
        SoundManager.instance.PlayBGM("IntroScene");
        SoundManager.instance.alreadyPlayed = true;
        Time.timeScale = 1;
        PhotonNetwork.AutomaticallySyncScene = true;
        FirstLine.transform.GetChild(0).GetComponent<Text>().text = PhotonNetwork.PlayerList[0].NickName + " 점수 ";
        FirstLine.transform.GetChild(1).GetComponent<Text>().text = PhotonNetwork.PlayerList[0].CustomProperties["OppositeMoney"].ToString();

        SecondLine.transform.GetChild(0).GetComponent<Text>().text = PhotonNetwork.PlayerList[1].NickName + " 점수 ";
        SecondLine.transform.GetChild(1).GetComponent<Text>().text = PhotonNetwork.PlayerList[1].CustomProperties["OppositeMoney"].ToString();

        if ((int)PhotonNetwork.PlayerList[0].CustomProperties["OppositeMoney"] > (int)PhotonNetwork.PlayerList[1].CustomProperties["OppositeMoney"])
        {
            Winner.text = PhotonNetwork.PlayerList[0].NickName;
        }
        else if ((int)PhotonNetwork.PlayerList[0].CustomProperties["OppositeMoney"] < (int)PhotonNetwork.PlayerList[1].CustomProperties["OppositeMoney"])
        {
            Winner.text = PhotonNetwork.PlayerList[1].NickName;
        }
        else
        {
            Winner.text = "무승부";
        }
        Invoke("Return", 5f);
    }

    private void Return()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
    
}

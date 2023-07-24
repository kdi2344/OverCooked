using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class IngamePhtonManager : MonoBehaviourPunCallbacks
{
    PhotonView pv;

    public int player1Money;
    public int player2Money;
    [SerializeField] private GameObject OppositeMoney;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if (!SoundManager.instance.isSingle)
        {
            OppositeMoney.SetActive(true);
        }
    }
}

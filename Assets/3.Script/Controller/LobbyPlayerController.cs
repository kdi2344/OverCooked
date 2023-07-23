using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyPlayerController : MonoBehaviourPunCallbacks
{
    private PhotonView pv;

    public bool isMoving = false;
    [SerializeField] private bool canActive = false; //�� �տ� ���̺� ������ �ִ���
    public bool isHolding = false;
    private bool isDash = false;
    private Rigidbody rigid;

    public Animator anim;
    public float Speed = 10.0f;

    public float rotateSpeed = 10.0f;       // ȸ�� �ӵ�

    float h, v;

    [Header("�÷��̾� ����")]
    [SerializeField] private GameObject idleR;
    [SerializeField] private GameObject idleL;
    [SerializeField] private GameObject GribR;
    [SerializeField] private GameObject GribL;
    [SerializeField] private GameObject Knife;

    public Sprite[] colors;
    private int colorIndex = -1;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        pv.RPC("SetHand", RpcTarget.All);
    }
    private void Update()
    {
        if (pv.IsMine)
        {
            rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
            if (Input.GetKeyDown(KeySetting1.keys[KeyAction1.RUN]) && !isDash && isMoving)
            {
                pv.RPC("Dash", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void Dash()
    {
        SoundManager.instance.PlayEffect("dash");
        isDash = true;
        Rigidbody playerRigid = GetComponent<Rigidbody>();
        playerRigid.AddForce(transform.TransformDirection(new Vector3(0, 0, 30)), ForceMode.Impulse);
        StartCoroutine(ControlSpeed());
        Invoke("OffIsDash", 0.5f);
    }
    IEnumerator ControlSpeed()
    {
        Speed = 20;
        while (Speed > 10)
        {
            Speed -= Time.deltaTime * 20;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return null;
        Speed = 10f;
    }
    private void OffIsDash()
    {
        isDash = false;
    }

    void FixedUpdate()
    {
        if (pv.IsMine)
        {
            anim.SetBool("isWalking", isMoving);
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");


            Vector3 dir = new Vector3(h, 0, v);

            // �ٶ󺸴� �������� ȸ�� �� �ٽ� ������ �ٶ󺸴� ������ ���� ���� ����
            if (!(h == 0 && v == 0))
            {
                isMoving = true;
                // �̵��� ȸ���� �Բ� ó��
                transform.position += dir * Speed * Time.deltaTime;
                // ȸ���ϴ� �κ�. Point 1.
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
            }
            else
            {
                isMoving = false;
            }
        }
    }

    [PunRPC]
    private void SetHand()
    {
        if (isHolding) //�� �����ٸ� �� ����
        {
            Knife.SetActive(false);
            idleL.SetActive(false);
            idleR.SetActive(false);
            GribL.SetActive(true);
            GribR.SetActive(true);
        }
        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("New_Chef@Chop")) //������ ���̸�
            {
                Knife.SetActive(true);
                idleL.SetActive(false);
                idleR.SetActive(false);
                GribL.SetActive(true);
                GribR.SetActive(true);
            }
            else
            {
                Knife.SetActive(false);
                idleL.SetActive(true);
                idleR.SetActive(true);
                GribL.SetActive(false);
                GribR.SetActive(false);
            }
        }
    }

    public void InitColor(int num)
    {
        pv.RPC(nameof(SetMT), RpcTarget.AllViaServer, num);
    }

    [PunRPC]
    private void SetMT(int index)
    {
        PhotonNetwork.CurrentRoom.Players[PhotonNetwork.CurrentRoom.PlayerCount].SetCustomProperties(new Hashtable { { "Color", index } });
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = colors[index];
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (pv.IsMine && colorIndex != -1)
        {
            pv.RPC(nameof(SetMT), newPlayer, colorIndex);
        }
    }
}

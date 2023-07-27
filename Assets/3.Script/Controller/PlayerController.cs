using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{
    //private PhotonView pv;

    public bool isMoving = false;
    [SerializeField] private bool canActive = false; //�� �տ� ���̺� ������ �ִ���
    public bool isHolding = false;
    private bool isDash = false;
    private Rigidbody rigid;

    public Animator anim;
    public float Speed = 10.0f;
    public GameObject activeObject;
    public Object activeObjectOb;
    public GameObject nextActiveObject;

    [SerializeField] private GameObject CountDown;

    [SerializeField] private Vector3 throwPower;
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
    }
    private void Update()
    {
        rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
        SetHand(); //�� ����
        if (Input.GetKeyDown(KeySetting1.keys[KeyAction1.ACTIVE]) && activeObject != null) //��� ������ ��ü�� �ְ� Space �����ٸ�
        {
            if (activeObjectOb.type == Object.ObjectType.CounterTop || activeObjectOb.type == Object.ObjectType.Board) //counterTop�� ��ȣ�ۿ�
            {
                if (canActive && activeObject.GetComponent<Object>().onSomething && !isHolding) //���ݿ� ���� �ִٸ� ����
                {
                    GameObject handleThing = activeObject.transform.parent.GetChild(2).gameObject;
                    if (handleThing.CompareTag("Ingredient") && activeObjectOb.type == Object.ObjectType.Board && activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().cookingBar.IsActive())
                    {
                        //�������� ��� �������� ����
                    }
                    else
                    {
                        SoundManager.instance.PlayEffect("take");
                        activeObjectOb.onSomething = false;
                        isHolding = true;
                        anim.SetBool("isHolding", isHolding);
                        if (handleThing.CompareTag("Ingredient"))
                        {
                            handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = false;
                            handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(transform, handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().hand);
                        }
                        else
                        {
                            handleThing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                            handleThing.GetComponent<Handle>().isOnDesk = false; //����
                            handleThing.GetComponent<Handle>().PlayerHandle(transform);
                        }
                    }
                }
                else if (canActive && activeObjectOb.onSomething && activeObject.transform.parent.childCount > 2 && activeObject.transform.parent.GetChild(2).GetComponent<Handle>().hand == Handle.HandleType.Plate && isHolding && transform.GetChild(1).GetChild(0).childCount > 0 && transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Handle>().isCooked)
                {
                    //���ݿ� �ִ°� �����̰� ���� ������ ��Ḧ ����ִٸ� ����
                    if (activeObject.transform.parent.GetChild(2).GetComponent<Plates>().AddIngredient(transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Handle>().hand))
                    {
                        SoundManager.instance.PlayEffect("put");
                        activeObject.transform.parent.GetChild(2).GetComponent<Plates>().InstantiateUI();
                        Destroy(transform.GetChild(1).gameObject);
                        isHolding = false;
                        anim.SetBool("isHolding", false);
                    }
                }
                else if (canActive && !activeObjectOb.onSomething && isHolding) //���ݿ� ���µ� ���� ���� ��� �ִٸ� ����
                {
                    SoundManager.instance.PlayEffect("put");
                    GameObject handleThing = transform.GetChild(1).gameObject;
                    if (handleThing.CompareTag("Ingredient"))
                    {
                        activeObjectOb.onSomething = true;
                        isHolding = false;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = true;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition, handleThing.transform.GetChild(0).GetChild(0).GetComponent<Handle>().hand);
                    }
                    else //����
                    {
                        if (activeObjectOb.type != Object.ObjectType.Board)
                        {
                            activeObjectOb.onSomething = true;
                            isHolding = false;
                            handleThing.GetComponent<Handle>().isOnDesk = true;
                            handleThing.GetComponent<Handle>().PlayerHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition);
                        }

                    }
                    anim.SetBool("isHolding", isHolding);
                }
                else if (canActive && isHolding && activeObjectOb.onSomething) //���� �� ����ְ� ��ȣ�ۿ� ������ �͵� �ִµ� ������ �׳� �ٴڿ� ����
                {
                    SoundManager.instance.PlayEffect("put");
                    if (transform.GetChild(1).CompareTag("Plate"))
                    {
                        transform.GetChild(1).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    }
                    else if (transform.GetChild(1).CompareTag("Ingredient"))
                    {
                        transform.GetChild(1).GetChild(0).GetComponent<MeshCollider>().isTrigger = false;
                        transform.GetChild(1).GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    }
                    transform.GetChild(1).SetParent(transform.parent);
                    isHolding = false;
                    anim.SetBool("isHolding", isHolding);
                }
            }
            else if (activeObjectOb.type == Object.ObjectType.Craft) //Crate�� ��ȣ�ۿ�
            {
                if (!activeObjectOb.onSomething && !isHolding) //Crate���� ���� ���� ���� �ȵ�� ������
                {
                    SoundManager.instance.PlayEffect("take");
                    activeObject.GetComponent<Craft>().OpenCraftPlayer1(); //������
                    activeObjectOb.onSomething = false;
                    isHolding = true;
                    anim.SetBool("isHolding", isHolding);
                }
                else if (!activeObjectOb.onSomething && isHolding) //Craft���� ���� ���� ���� ���������
                {
                    GameObject handleThing = transform.GetChild(1).gameObject;
                    SoundManager.instance.PlayEffect("put");
                    if (handleThing.CompareTag("Ingredient"))
                    {
                        activeObjectOb.onSomething = true;
                        isHolding = false;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = true;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition, handleThing.transform.GetChild(0).GetChild(0).GetComponent<Handle>().hand);
                    }
                    else //����
                    {
                        if (activeObjectOb.type != Object.ObjectType.Board)
                        {
                            activeObjectOb.onSomething = true;
                            isHolding = false;
                            handleThing.GetComponent<Handle>().isOnDesk = true;
                            handleThing.GetComponent<Handle>().PlayerHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition);
                        }

                    }
                    anim.SetBool("isHolding", isHolding);
                    activeObjectOb.onSomething = true;
                    //transform.GetChild(1).gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = true; //��������
                    //transform.GetChild(1).gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition); //��������
                }
                else if (activeObjectOb.onSomething && !isHolding) //crate���� ���� �ְ� ���� �ȵ�������� ����
                {
                    SoundManager.instance.PlayEffect("take");
                    activeObjectOb.onSomething = false;
                    isHolding = true;
                    anim.SetBool("isHolding", isHolding);
                    GameObject handleThing = activeObject.transform.parent.GetChild(2).gameObject;
                    if (handleThing.CompareTag("Ingredient"))
                    {
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = false;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(transform, handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().hand);
                    }
                    else //����
                    {
                        handleThing.GetComponent<Handle>().isOnDesk = false;
                        handleThing.GetComponent<Handle>().PlayerHandle(transform);
                    }
                }
            }
            else if (!isHolding && activeObject.CompareTag("Ingredient")) //������ ��� �ݱ�
            {
                SoundManager.instance.PlayEffect("take");
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
                GameObject handleThing = activeObject.transform.parent.gameObject;
                handleThing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                handleThing.transform.GetChild(0).GetComponent<Handle>().IngredientHandle(transform, handleThing.transform.GetChild(0).GetComponent<Handle>().hand);
            }
            else if (activeObject.CompareTag("Bin")) //��������� ��ȣ�ۿ�
            {
                SoundManager.instance.PlayEffect("bin");
                if (isHolding && transform.GetChild(1).gameObject.GetComponent<Handle>() != null && transform.GetChild(1).gameObject.GetComponent<Handle>().hand == Handle.HandleType.Plate) //���� ��� �������� �������� �ϸ�
                {
                    transform.GetChild(1).gameObject.GetComponent<Plates>().ClearIngredient();
                }
                else if (transform.childCount > 1) //��� �� ��ä���
                {
                    Destroy(transform.GetChild(1).gameObject); //����ִ¾� ����
                    isHolding = false;
                    anim.SetBool("isHolding", isHolding);
                }
            }
            else if (activeObject.CompareTag("Station")) //�׸� ����
            {
                if (isHolding && transform.GetChild(1).gameObject.GetComponent<Handle>() != null && transform.GetChild(1).gameObject.GetComponent<Handle>().hand == Handle.HandleType.Plate)
                {
                    if (GameManager.instance.CheckMenu(transform.GetChild(1).gameObject.GetComponent<Plates>().containIngredients))
                    {
                        SoundManager.instance.PlayEffect("right");
                        GameManager.instance.MakeOrder();
                    }
                    else
                    {
                        SoundManager.instance.PlayEffect("no");
                        //�����ϸ� ���ñ� ������ �ҵ����� �� �׷��� �Լ� ���� ó���ϱ�
                    }
                    Destroy(transform.GetChild(1).gameObject); //���� ��°�� ���� (�ð� �Ǹ� ��Ȱ������ �ٲٱ�)
                    isHolding = false;
                    anim.SetBool("isHolding", isHolding);
                    GameManager.instance.PlateReturn();
                }
            }
            else if (!isHolding && activeObject.CompareTag("Plate")) //������ plate �ݱ�
            {
                SoundManager.instance.PlayEffect("take");
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
                GameObject handleThing = activeObject.transform.parent.gameObject;
                handleThing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                handleThing.GetComponent<Handle>().PlayerHandle(transform);
            }
            else if (activeObject.CompareTag("Return") && activeObject.GetComponent<Object>().onSomething && !isHolding) //��ȯ ���� ��������
            {
                SoundManager.instance.PlayEffect("take");
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
                GameObject handleThing = activeObject.GetComponent<Return>().TakePlate(); //�ϳ� ������
                handleThing.GetComponent<Handle>().isOnDesk = false;
                handleThing.GetComponent<Handle>().PlayerHandle(transform);
            }
            else if (activeObject.CompareTag("Return") && activeObject.GetComponent<Object>().onSomething && isHolding)
            {
                if (activeObject.CompareTag("Ingredient") && activeObject.GetComponent<Handle>().isCooked) { }
                {
                    SoundManager.instance.PlayEffect("add");
                    FindObjectOfType<Return>().returnPlates[FindObjectOfType<Return>().returnPlates.Count - 1].GetComponent<Plates>().AddIngredient(transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Handle>().hand);
                    FindObjectOfType<Return>().returnPlates[FindObjectOfType<Return>().returnPlates.Count - 1].GetComponent<Plates>().InstantiateUI();
                    isHolding = false;
                    anim.SetBool("isHolding", false);
                    Destroy(transform.GetChild(1).gameObject);
                }
            }
            else
            {
                if (activeObject != null && activeObject.CompareTag("Ingredient") && isHolding) //�տ� �ƹ��͵� ���� �ٴڿ� �α� (����ó��)
                {
                    SoundManager.instance.PlayEffect("put");
                    transform.GetChild(1).GetChild(0).GetComponent<MeshCollider>().isTrigger = false;
                    transform.GetChild(1).GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    transform.GetChild(1).SetParent(transform.parent);
                    isHolding = false;
                    anim.SetBool("isHolding", isHolding);
                }
                else
                {
                    Debug.Log(activeObject.tag);
                }
            }
        }
        else if (Input.GetKeyDown(KeySetting1.keys[KeyAction1.ACTIVE]) && isHolding) //�տ� �ƹ��͵� ���µ� �� ����ִ� -> �ٴڿ� ������
        {
            SoundManager.instance.PlayEffect("add");
            if (transform.GetChild(1).CompareTag("Plate"))
            {
                transform.GetChild(1).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            else if (transform.GetChild(1).CompareTag("Ingredient"))
            {
                transform.GetChild(1).GetChild(0).GetComponent<MeshCollider>().isTrigger = false;
                transform.GetChild(1).GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            transform.GetChild(1).SetParent(transform.parent);
            isHolding = false;
            anim.SetBool("isHolding", isHolding);
        }

        if (Input.GetKeyDown(KeySetting1.keys[KeyAction1.CUT]) && activeObjectOb.type == Object.ObjectType.Board && activeObject.transform.parent.childCount > 2 && !activeObject.transform.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Handle>().isCooked && !isHolding)
        {//�ڸ���
            if (activeObject.transform.GetChild(0).GetComponent<CuttingBoard>()._CoTimer == null) //�ѹ��� ���� �ȵȰŸ� ���� ����
            {
                anim.SetTrigger("startCut");
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().Pause = false;
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().CuttingTime = 0;
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().StartCutting1();
            }
            else if (activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().Pause) //����Ǵ� ���Ŷ��
            {
                anim.SetTrigger("startCut");
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().PauseSlider(false);
            }
        }

        if (Input.GetKeyDown(KeySetting1.keys[KeyAction1.THROW]) && isHolding && transform.GetChild(1).GetComponent<Handle>() == null)
        { //ingredient ������ ����
            SoundManager.instance.PlayEffect("throw");
            Vector3 dir = transform.TransformDirection(throwPower);
            anim.SetTrigger("throw");
            isHolding = false;
            anim.SetBool("isHolding", isHolding);
            transform.GetChild(1).GetChild(0).transform.localPosition += new Vector3(0, 0.3f, 0);
            Rigidbody ingreRigid = transform.GetChild(1).GetChild(0).GetComponent<Rigidbody>();
            ingreRigid.AddForce(dir, ForceMode.Impulse);
            transform.GetChild(1).GetChild(0).GetComponent<MeshCollider>().isTrigger = false;
            transform.GetChild(1).GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            transform.GetChild(1).SetParent(transform.parent);
        }

        if (Input.GetKeyDown(KeySetting1.keys[KeyAction1.RUN]) && !isDash && isMoving)
        {
            SoundManager.instance.PlayEffect("dash");
            isDash = true;
            Rigidbody playerRigid = GetComponent<Rigidbody>();
            playerRigid.AddForce(transform.TransformDirection(new Vector3(0, 0, 30)), ForceMode.Impulse);
            StartCoroutine(ControlSpeed());
            Invoke("OffIsDash", 0.5f);
        }
    }
    IEnumerator ControlSpeed()
    {
        Speed = 20;
        while(Speed > 10)
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
        anim.SetBool("isWalking", isMoving);
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");


        Vector3 dir = new Vector3(h, 0, v);

        //�ٶ󺸴� �������� ȸ�� �� �ٽ� ������ �ٶ󺸴� ������ ���� ���� ����
        if (!(h == 0 && v == 0))
        {
            isMoving = true;
            //�̵��� ȸ���� �Բ� ó��
            transform.position += dir * Speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
        }
        else
        {
            isMoving = false;
        }
    }
    private void DieRespawn()
    {
        if (isHolding && transform.GetChild(1).CompareTag("Plate"))
        {
            Destroy(transform.GetChild(1).gameObject);
            GameManager.instance.PlateReturn();
        }
        else if (isHolding && transform.GetChild(1).CompareTag("Ingredient"))
        {
            Destroy(transform.GetChild(1).gameObject);
        }
        isHolding = false;
        ShowCount();
        gameObject.SetActive(false);
        Invoke("Respawn", 5);
    }
    private void ShowCount()
    {
        CountDown.transform.GetChild(1).GetComponent<Text>().text = "5";
        CountDown.SetActive(true);
        Invoke("ShowCount1", 1f);
    }
    private void ShowCount1()
    {
        CountDown.transform.GetChild(1).GetComponent<Text>().text = "4";
        Invoke("ShowCount2", 1f);
    }
    private void ShowCount2()
    {
        CountDown.transform.GetChild(1).GetComponent<Text>().text = "3";
        Invoke("ShowCount3", 1f);
    }
    private void ShowCount3()
    {
        CountDown.transform.GetChild(1).GetComponent<Text>().text = "2";
        Invoke("ShowCount4", 1f);
    }
    private void ShowCount4()
    {
        CountDown.transform.GetChild(1).GetComponent<Text>().text = "1";
        Invoke("ShowCount5", 1f);
    }
    private void ShowCount5()
    {
        CountDown.SetActive(false);
    }
    private void Respawn()
    {
        SoundManager.instance.PlayEffect("respawn");
        gameObject.transform.localPosition = new Vector3(20.6200008f, 1.33632302f, -6.61000013f);
        gameObject.transform.localRotation = new Quaternion(0, 1, 0, 0);
        gameObject.SetActive(true);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("deadZone"))
        {
            SoundManager.instance.PlayEffect("fall");
            DieRespawn();
            return;
        }
        if (activeObject != null && activeObject.CompareTag("Ingredient") && isHolding)
        {
            activeObject.GetComponent<Object>().OffHighlight(activeObject.GetComponent<Handle>().isCooked);
            activeObject = null;
            activeObjectOb = null;
            return;
        }
        if (other == activeObject) //�ߺ�üũ
        {
            return;
        }
        if (other.CompareTag("Ingredient"))
        {
            if (activeObject == null && !other.GetComponent<Handle>().isOnDesk)
            {
                activeObject = other.gameObject; //�Ƕ��� ��Ḹ �ִ°� �ݴ´ٸ�
                activeObjectOb = activeObject.GetComponent<Object>();
                other.GetComponent<Object>().OnHighlight(other.GetComponent<Handle>().isCooked);
                return;
            }
            else //å������ ��� �ݴ´ٸ�
            {
                return;
            }
        }

        canActive = true;
        if (activeObject == null && other.GetComponent<Object>() != null)
        {
            activeObject = other.gameObject;
            activeObjectOb = activeObject.GetComponent<Object>();
            other.GetComponent<Object>().canActive = true; //Object ��ũ��Ʈ �� �־��ָ� ���� �ȳ�
            if (other.CompareTag("Ingredient"))
            {
                other.GetComponent<Object>().OnHighlight(other.GetComponent<Handle>().isCooked);
            }
            else
            {
                other.GetComponent<Object>().OnHighlight(other.GetComponent<Object>().onSomething);
            }
        }
        else
        {
            if (other.GetComponent<Object>()!=null && !other.GetComponent<Object>().onSomething)
            {
                nextActiveObject = other.gameObject;
            }
        }
        if (activeObject!=null && activeObject.GetComponent<Object>().type == Object.ObjectType.Board)
        {
            anim.SetBool("canCut", true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ingredient") && isHolding)
        {
            return;
        }
        if (other.CompareTag("Plate") && isHolding)
        {
            return;
        }
        if (activeObject != null && activeObject.CompareTag("Ingredient") && isHolding)
        {
            if (nextActiveObject != null)
            {
                activeObject.GetComponent<Object>().OffHighlight(activeObject.GetComponent<Handle>().isCooked);
                activeObject = nextActiveObject;
                activeObjectOb = activeObject.GetComponent<Object>();
                activeObject.GetComponent<Object>().OnHighlight(activeObject.GetComponent<Handle>().isCooked);
                nextActiveObject = null;
            }
            else
            {
                activeObject.GetComponent<Object>().OffHighlight(activeObject.GetComponent<Handle>().isCooked);
                activeObject = null;
                activeObjectOb = null;
            }
        }
        if (activeObject == null)
        {
            canActive = true;
            if (other.CompareTag("Ingredient") && isHolding)
            {
                return;
            }
            if (other.CompareTag("Plate") && isHolding)
            {
                return;
            }
            if (other.GetComponent<Object>() != null)
            {
                activeObject = other.gameObject;
                activeObjectOb = activeObject.GetComponent<Object>();
                other.GetComponent<Object>().canActive = true;
                if (other.CompareTag("Ingredient"))
                {
                    other.GetComponent<Object>().OnHighlight(activeObject.GetComponent<Handle>().isCooked);
                }
                else
                {
                    other.GetComponent<Object>().OnHighlight(true);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (activeObject != null && nextActiveObject == null)
        {
            if (activeObject.GetComponent<Object>().type == Object.ObjectType.Board)
            {
                anim.SetBool("canCut", false);
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().PauseSlider(true);
            }
            canActive = false;
            if (activeObject.GetComponent<Object>() != null)
            {
                if (activeObject.CompareTag("Ingredient"))
                {
                    activeObject.GetComponent<Object>().OffHighlight(activeObject.GetComponent<Handle>().isCooked);
                }
                else
                {
                    activeObject.GetComponent<Object>().OffHighlight(true);
                }
            }
            activeObject = null;
            activeObjectOb = null;
        }
        else
        {
            if (activeObject == null)
            {
                anim.SetBool("canCut", false);
                return;
            }
            if (other.gameObject == activeObject)
            {
                anim.SetBool("canCut", false);
                if (activeObject.CompareTag("Ingredient"))
                {
                    activeObject.GetComponent<Object>().OffHighlight(activeObject.GetComponent<Handle>().isCooked);
                }
                else
                {
                    activeObject.GetComponent<Object>().OffHighlight(true);
                }
                activeObject = nextActiveObject;
                if (activeObject.CompareTag("Ingredient"))
                {
                    activeObject.GetComponent<Object>().OnHighlight(activeObject.GetComponent<Handle>().isCooked);
                }
                else
                {
                    activeObject.GetComponent<Object>().OnHighlight(true);
                }
                activeObjectOb = activeObject.GetComponent<Object>();
                nextActiveObject = null;
            }
            else
            {
                nextActiveObject = null;
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
}

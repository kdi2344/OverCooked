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
    [SerializeField] private bool canActive = false; //내 앞에 테이블 같은거 있는지
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
    public float rotateSpeed = 10.0f;       // 회전 속도

    float h, v;

    [Header("플레이어 관련")]
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
        SetHand(); //손 세팅
        if (Input.GetKeyDown(KeySetting1.keys[KeyAction1.ACTIVE]) && activeObject != null) //사용 가능한 물체가 있고 Space 눌렀다면
        {
            if (activeObjectOb.type == Object.ObjectType.CounterTop || activeObjectOb.type == Object.ObjectType.Board) //counterTop과 상호작용
            {
                if (canActive && activeObject.GetComponent<Object>().onSomething && !isHolding) //선반에 무언가 있다면 집기
                {
                    GameObject handleThing = activeObject.transform.parent.GetChild(2).gameObject;
                    if (handleThing.CompareTag("Ingredient") && activeObjectOb.type == Object.ObjectType.Board && activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().cookingBar.IsActive())
                    {
                        //손질중인 재료 못가져가 히히
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
                            handleThing.GetComponent<Handle>().isOnDesk = false; //접시
                            handleThing.GetComponent<Handle>().PlayerHandle(transform);
                        }
                    }
                }
                else if (canActive && activeObjectOb.onSomething && activeObject.transform.parent.childCount > 2 && activeObject.transform.parent.GetChild(2).GetComponent<Handle>().hand == Handle.HandleType.Plate && isHolding && transform.GetChild(1).GetChild(0).childCount > 0 && transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Handle>().isCooked)
                {
                    //선반에 있는게 접시이고 내가 손질된 재료를 들고있다면 놓기
                    if (activeObject.transform.parent.GetChild(2).GetComponent<Plates>().AddIngredient(transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Handle>().hand))
                    {
                        SoundManager.instance.PlayEffect("put");
                        activeObject.transform.parent.GetChild(2).GetComponent<Plates>().InstantiateUI();
                        Destroy(transform.GetChild(1).gameObject);
                        isHolding = false;
                        anim.SetBool("isHolding", false);
                    }
                }
                else if (canActive && !activeObjectOb.onSomething && isHolding) //선반에 없는데 내가 뭔가 들고 있다면 놓기
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
                    else //접시
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
                else if (canActive && isHolding && activeObjectOb.onSomething) //내가 뭘 들고있고 상호작용 가능한 것도 있는데 누르면 그냥 바닥에 버림
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
            else if (activeObjectOb.type == Object.ObjectType.Craft) //Crate와 상호작용
            {
                if (!activeObjectOb.onSomething && !isHolding) //Crate위에 뭐가 없고 나도 안들고 있으면
                {
                    SoundManager.instance.PlayEffect("take");
                    activeObject.GetComponent<Craft>().OpenCraftPlayer1(); //꺼내기
                    activeObjectOb.onSomething = false;
                    isHolding = true;
                    anim.SetBool("isHolding", isHolding);
                }
                else if (!activeObjectOb.onSomething && isHolding) //Craft위에 뭐가 없고 내가 들고있으면
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
                    else //접시
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
                    //transform.GetChild(1).gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = true; //내려놓기
                    //transform.GetChild(1).gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition); //내려놓기
                }
                else if (activeObjectOb.onSomething && !isHolding) //crate위에 뭐가 있고 내가 안들고있으면 집기
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
                    else //접시
                    {
                        handleThing.GetComponent<Handle>().isOnDesk = false;
                        handleThing.GetComponent<Handle>().PlayerHandle(transform);
                    }
                }
            }
            else if (!isHolding && activeObject.CompareTag("Ingredient")) //떨어진 재료 줍기
            {
                SoundManager.instance.PlayEffect("take");
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
                GameObject handleThing = activeObject.transform.parent.gameObject;
                handleThing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                handleThing.transform.GetChild(0).GetComponent<Handle>().IngredientHandle(transform, handleThing.transform.GetChild(0).GetComponent<Handle>().hand);
            }
            else if (activeObject.CompareTag("Bin")) //쓰레기통과 상호작용
            {
                SoundManager.instance.PlayEffect("bin");
                if (isHolding && transform.GetChild(1).gameObject.GetComponent<Handle>() != null && transform.GetChild(1).gameObject.GetComponent<Handle>().hand == Handle.HandleType.Plate) //접시 들고 쓰레기통 버리려고 하면
                {
                    transform.GetChild(1).gameObject.GetComponent<Plates>().ClearIngredient();
                }
                else if (transform.childCount > 1) //재료 그 잡채라면
                {
                    Destroy(transform.GetChild(1).gameObject); //들고있는애 삭제
                    isHolding = false;
                    anim.SetBool("isHolding", isHolding);
                }
            }
            else if (activeObject.CompareTag("Station")) //그릇 제출
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
                        //실패하면 뭐시기 빨간색 불들어오고 뭐 그런거 함수 만들어서 처리하기
                    }
                    Destroy(transform.GetChild(1).gameObject); //접시 통째로 삭제 (시간 되면 재활용으로 바꾸기)
                    isHolding = false;
                    anim.SetBool("isHolding", isHolding);
                    GameManager.instance.PlateReturn();
                }
            }
            else if (!isHolding && activeObject.CompareTag("Plate")) //떨어진 plate 줍기
            {
                SoundManager.instance.PlayEffect("take");
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
                GameObject handleThing = activeObject.transform.parent.gameObject;
                handleThing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                handleThing.GetComponent<Handle>().PlayerHandle(transform);
            }
            else if (activeObject.CompareTag("Return") && activeObject.GetComponent<Object>().onSomething && !isHolding) //반환 접시 가져오기
            {
                SoundManager.instance.PlayEffect("take");
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
                GameObject handleThing = activeObject.GetComponent<Return>().TakePlate(); //하나 꺼내기
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
                if (activeObject != null && activeObject.CompareTag("Ingredient") && isHolding) //앞에 아무것도 없고 바닥에 두기 (예외처리)
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
        else if (Input.GetKeyDown(KeySetting1.keys[KeyAction1.ACTIVE]) && isHolding) //앞에 아무것도 없는데 뭘 들고있다 -> 바닥에 버리기
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
        {//자르기
            if (activeObject.transform.GetChild(0).GetComponent<CuttingBoard>()._CoTimer == null) //한번도 실행 안된거면 시작 가능
            {
                anim.SetTrigger("startCut");
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().Pause = false;
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().CuttingTime = 0;
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().StartCutting1();
            }
            else if (activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().Pause) //실행되다 만거라면
            {
                anim.SetTrigger("startCut");
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().PauseSlider(false);
            }
        }

        if (Input.GetKeyDown(KeySetting1.keys[KeyAction1.THROW]) && isHolding && transform.GetChild(1).GetComponent<Handle>() == null)
        { //ingredient 던지기 가능
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

        //바라보는 방향으로 회전 후 다시 정면을 바라보는 현상을 막기 위해 설정
        if (!(h == 0 && v == 0))
        {
            isMoving = true;
            //이동과 회전을 함께 처리
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
        if (other == activeObject) //중복체크
        {
            return;
        }
        if (other.CompareTag("Ingredient"))
        {
            if (activeObject == null && !other.GetComponent<Handle>().isOnDesk)
            {
                activeObject = other.gameObject; //맨땅에 재료만 있는거 줍는다면
                activeObjectOb = activeObject.GetComponent<Object>();
                other.GetComponent<Object>().OnHighlight(other.GetComponent<Handle>().isCooked);
                return;
            }
            else //책상위에 재료 줍는다면
            {
                return;
            }
        }

        canActive = true;
        if (activeObject == null && other.GetComponent<Object>() != null)
        {
            activeObject = other.gameObject;
            activeObjectOb = activeObject.GetComponent<Object>();
            other.GetComponent<Object>().canActive = true; //Object 스크립트 다 넣어주면 오류 안남
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
        if (isHolding) //뭘 집었다면 손 접기
        {
            Knife.SetActive(false);
            idleL.SetActive(false);
            idleR.SetActive(false);
            GribL.SetActive(true);
            GribR.SetActive(true);
        }
        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("New_Chef@Chop")) //다지는 중이면
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

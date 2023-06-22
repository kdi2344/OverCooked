using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isMoving = false;
    [SerializeField] private bool canActive = false; //�� �տ� ���̺� ������ �ִ���
    public bool isHolding = false;

    public Animator anim;
    public float Speed = 10.0f;
    public GameObject activeObject;
    public Object activeObjectOb;
    public GameObject nextActiveObject;

    [SerializeField] private Vector3 throwPower;

    public float rotateSpeed = 10.0f;       // ȸ�� �ӵ�

    float h, v;

    [Header("�÷��̾� ����")]
    [SerializeField] private GameObject idleR;
    [SerializeField] private GameObject idleL;
    [SerializeField] private GameObject GribR;
    [SerializeField] private GameObject GribL;
    [SerializeField] private GameObject Knife;

    private void Update()
    {
        SetHand(); //�� ����
        if (Input.GetKeyDown(KeyCode.Space) && activeObject != null) //��� ������ ��ü�� �ְ� Space �����ٸ�
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
                else if (canActive && activeObjectOb.onSomething && activeObject.transform.parent.childCount>2 && activeObject.transform.parent.GetChild(2).GetComponent<Handle>().hand == Handle.HandleType.Plate && isHolding && transform.GetChild(1).GetChild(0).childCount > 0 && transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Handle>().isCooked)
                {
                    //���ݿ� �ִ°� �����̰� ���� ������ ��Ḧ ����ִٸ� ����
                    if (activeObject.transform.parent.GetChild(2).GetComponent<Plates>().AddIngredient(transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Handle>().hand))
                    {
                        activeObject.transform.parent.GetChild(2).GetComponent<Plates>().InstantiateUI();
                        Destroy(transform.GetChild(1).gameObject);
                        isHolding = false;
                        anim.SetBool("isHolding", false);
                    }
                }
                else if (canActive && !activeObjectOb.onSomething && isHolding) //���ݿ� ���µ� ���� ���� ��� �ִٸ� ����
                {
                    GameObject handleThing = transform.GetChild(1).gameObject;
                    if (handleThing.CompareTag("Ingredient"))
                    {
                        activeObjectOb.onSomething = true;
                        isHolding = false;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = true;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition);
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
            }
            else if (activeObjectOb.type == Object.ObjectType.Craft) //Crate�� ��ȣ�ۿ�
            {
                if (!activeObjectOb.onSomething && !isHolding) //Crate���� ���� ���� ���� �ȵ�� ������
                {
                    activeObject.GetComponent<Craft>().OpenCraft(); //������
                    activeObjectOb.onSomething = false;
                    isHolding = true;
                    anim.SetBool("isHolding", isHolding);
                }
                else if (!activeObjectOb.onSomething && isHolding) //Craft���� ���� ���� ���� ���������
                {
                    GameObject handleThing = transform.GetChild(1).gameObject;
                    if (handleThing.CompareTag("Ingredient"))
                    {
                        activeObjectOb.onSomething = true;
                        isHolding = false;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = true;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition);
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
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
                GameObject handleThing = activeObject.transform.parent.gameObject;
                handleThing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                handleThing.transform.GetChild(0).GetComponent<Handle>().IngredientHandle(transform, handleThing.transform.GetChild(0).GetComponent<Handle>().hand);
            }
            else if (activeObject.CompareTag("Bin")) //��������� ��ȣ�ۿ�
            {
                if (isHolding && transform.GetChild(1).gameObject.GetComponent<Handle>()!=null && transform.GetChild(1).gameObject.GetComponent<Handle>().hand == Handle.HandleType.Plate) //���� ��� �������� �������� �ϸ�
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
                        Debug.Log("�ִ� �޴�");
                        GameManager.instance.MakeOrder();
                    }
                    else
                    {
                        Debug.Log("���� �޴�");
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
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
                GameObject handleThing = activeObject.transform.parent.gameObject;
                handleThing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                handleThing.GetComponent<Handle>().PlayerHandle(transform);
            }
            else if (activeObject.CompareTag("Return") && activeObject.GetComponent<Object>().onSomething && !isHolding) //��ȯ ���� ��������
            {
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
                GameObject handleThing = activeObject.GetComponent<Return>().TakePlate(); //�ϳ� ������
                handleThing.GetComponent<Handle>().isOnDesk = false;
                handleThing.GetComponent<Handle>().PlayerHandle(transform);
            }
            else
            {
                if (activeObject != null && activeObject.CompareTag("Ingredient") && isHolding) //�տ� �ƹ��͵� ���� �ٴڿ� �α� (����ó��)
                {
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
        else if (Input.GetKeyDown(KeyCode.Space) && isHolding) //�տ� �ƹ��͵� ���µ� �� ����ִ� -> �ٴڿ� ������
        {
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

        if (Input.GetKeyDown(KeyCode.C) && activeObjectOb.type == Object.ObjectType.Board && activeObject.transform.parent.childCount > 2 && !activeObject.transform.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Handle>().isCooked && !isHolding)
        {//�ڸ���
            if (activeObject.transform.GetChild(0).GetComponent<CuttingBoard>()._CoTimer == null) //�ѹ��� ���� �ȵȰŸ� ���� ����
            {
                anim.SetTrigger("startCut");
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().Pause = false;
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().CuttingTime = 0;
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().StartCutting();
            }
            else if (activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().Pause) //����Ǵ� ���Ŷ��
            {
                anim.SetTrigger("startCut");
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().PauseSlider(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && isHolding && transform.GetChild(1).GetComponent<Handle>() == null)
        { //ingredient ������ ����
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
    }

    void FixedUpdate()
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

    private void OnTriggerEnter(Collider other)
    {
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
            else
            {
                //activeObject = other.gameObject;
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
                Debug.Log("�ܾʉ�");
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

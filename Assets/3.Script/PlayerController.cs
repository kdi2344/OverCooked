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
    public GameObject nextActiveObject;

    public float rotateSpeed = 10.0f;       // ȸ�� �ӵ�

    float h, v;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && activeObject != null)
        {
            if (activeObject.GetComponent<Object>().type == Object.ObjectType.CounterTop || activeObject.GetComponent<Object>().type == Object.ObjectType.Board) //counterTop�� ��ȣ�ۿ�
            {
                if (canActive && activeObject.GetComponent<Object>().onSomething && !isHolding) //���ݿ� ���� �ִٸ� ����
                {
                    activeObject.GetComponent<Object>().onSomething = false;
                    isHolding = true;
                    anim.SetBool("isHolding", isHolding);
                    GameObject handleThing = activeObject.transform.parent.GetChild(2).gameObject;
                    if (handleThing.CompareTag("Ingredient"))
                    {
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = false;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(transform);
                    }
                    else
                    {
                        handleThing.GetComponent<Handle>().isOnDesk = false; //����
                        handleThing.GetComponent<Handle>().PlayerHandle(transform);
                    }
                }
                else if (canActive && !activeObject.GetComponent<Object>().onSomething && isHolding) //���ݿ� ���µ� ���� ���� ��� �ִٸ� ����
                {
                    GameObject handleThing = transform.GetChild(1).gameObject;
                    if (handleThing.CompareTag("Ingredient"))
                    {
                        activeObject.GetComponent<Object>().onSomething = true;
                        isHolding = false;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = true;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition);
                    }
                    else //����
                    {
                        if (activeObject.GetComponent<Object>().type != Object.ObjectType.Board)
                        {
                            activeObject.GetComponent<Object>().onSomething = true;
                            isHolding = false;
                            handleThing.GetComponent<Handle>().isOnDesk = true;
                            handleThing.GetComponent<Handle>().PlayerHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition);
                        }
                        
                    }
                    anim.SetBool("isHolding", isHolding);
                }
            }
            else if (activeObject.GetComponent<Object>().type == Object.ObjectType.Craft) //Crate�� ��ȣ�ۿ�
            {
                if (!activeObject.GetComponent<Object>().onSomething && !isHolding) //crate���� ���� ���� ���� �ȵ�� ������
                {
                    activeObject.GetComponent<Craft>().OpenCraft(); //������
                    activeObject.GetComponent<Object>().onSomething = false;
                    isHolding = true;
                    anim.SetBool("isHolding", isHolding);
                }
                else if (!activeObject.GetComponent<Object>().onSomething && isHolding) //craft���� ���� ���� ���� ���������
                {
                    activeObject.GetComponent<Object>().onSomething = true;
                    transform.GetChild(1).gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = true; //��������
                    transform.GetChild(1).gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition); //��������
                    isHolding = false;
                    anim.SetBool("isHolding", false);
                }
                else if (activeObject.GetComponent<Object>().onSomething && !isHolding) //crate���� ���� �ְ� ���� �ȵ�������� ����
                {
                    activeObject.GetComponent<Object>().onSomething = false;
                    isHolding = true;
                    anim.SetBool("isHolding", isHolding);
                    GameObject handleThing = activeObject.transform.parent.GetChild(2).gameObject;
                    if (handleThing.CompareTag("Ingredient"))
                    {
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = false;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(transform);
                    }
                    else //����
                    {
                        handleThing.GetComponent<Handle>().isOnDesk = false;
                        handleThing.GetComponent<Handle>().PlayerHandle(transform);
                    }
                }
            }
            else if (activeObject.CompareTag("Ingredient")) //���� ��ȣ�ۿ�
            {

            }
            else
            {

            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !canActive && isHolding) //�տ� �ƹ��͵� ���µ� �� ����ִ� -> �ٴڿ� ������
        {
            isHolding = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && activeObject.GetComponent<Object>().type == Object.ObjectType.Board && canActive && !activeObject.transform.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Handle>().isCooked)
        {
            if (activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().Pause) //����Ǵ� ���Ŷ��
            {
                anim.SetTrigger("startCut");
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().PauseSlider(false);
            }
            else if (activeObject.transform.GetChild(0).GetComponent<CuttingBoard>()._CoTimer == null) //�ѹ��� ���� �ȵȰŸ� ���� ����
            {
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().Pause = false;
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().CuttingTime = 0;
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().StartCutting();
            }
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
        if (other == activeObject) //�ߺ�üũ
        {
            return;
        }
        if (other.CompareTag("Ingredient"))
        {
            if (activeObject == null && !other.GetComponent<Handle>().isOnDesk)
            {
                activeObject = other.gameObject; //�Ƕ��� ��Ḹ �ִ°� �ݴ´ٸ�
                other.GetComponent<Object>().OnHighlight(false);
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
            other.GetComponent<Object>().canActive = true; //Object ��ũ��Ʈ �� �־��ָ� ���� �ȳ�
            other.GetComponent<Object>().OnHighlight(other.GetComponent<Object>().onSomething);
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
        if (activeObject == null)
        {
            canActive = true;
            if (other.CompareTag("Ingredient") && isHolding)
            {
                return;
            }
            if (other.GetComponent<Object>() != null)
            {
                activeObject = other.gameObject;
                other.GetComponent<Object>().canActive = true;
                other.GetComponent<Object>().OnHighlight(other.GetComponent<Object>().onSomething);
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
                activeObject.GetComponent<Object>().OffHighlight();
            }
            activeObject = null;

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
                activeObject.GetComponent<Object>().OffHighlight();
                activeObject = nextActiveObject;
                activeObject.GetComponent<Object>().OnHighlight(other.GetComponent<Object>().onSomething);
                nextActiveObject = null;
            }
            else
            {
                nextActiveObject = null;
            }
        }
    }
}

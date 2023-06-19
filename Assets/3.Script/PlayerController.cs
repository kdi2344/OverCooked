using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isMoving = false;
    [SerializeField] private bool canActive = false; //�� �տ� ���̺� ������ �ִ���
    public bool isHolding = false;

    [SerializeField] private Animator anim;
    public float Speed = 10.0f;
    public GameObject activeObject;
    public GameObject nextActiveObject;

    public float rotateSpeed = 10.0f;       // ȸ�� �ӵ�

    float h, v;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && activeObject != null && activeObject.CompareTag("Craft"))
        {
            if (!activeObject.GetComponent<Object>().onSomething && !isHolding) //craft���� ���� ���� ���� �ȵ�� ������
            {
                activeObject.GetComponent<Craft>().OpenCraft(); //������
                activeObject.GetComponent<Object>().onSomething = false;
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
            }
            else if(!activeObject.GetComponent<Object>().onSomething && isHolding) //craft���� ���� ���� ���� ���������
            {
                activeObject.GetComponent<Object>().onSomething = true;
                transform.GetChild(1).gameObject.GetComponent<Handle>().PlayerHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition); //��������
                isHolding = false;
                anim.SetBool("isHolding", false);
            }
            else if (activeObject.GetComponent<Object>().onSomething && !isHolding) //craft���� ���� �ְ� ���� �ȵ�������� ����
            {
                activeObject.GetComponent<Object>().onSomething = false;
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
                GameObject handleThing = activeObject.transform.parent.GetChild(2).gameObject;
                handleThing.GetComponent<Handle>().PlayerHandle(transform);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Space) && activeObject.CompareTag("CounterTop") && canActive && activeObject.GetComponent<Object>().onSomething && !isHolding) //���ݿ� ���� �ִٸ� ����
        {
            activeObject.GetComponent<Object>().onSomething = false;
            isHolding = true;
            anim.SetBool("isHolding", isHolding);
            GameObject handleThing = activeObject.transform.parent.GetChild(2).gameObject;
            handleThing.GetComponent<Handle>().PlayerHandle(transform);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && activeObject.CompareTag("CounterTop") && canActive && !activeObject.GetComponent<Object>().onSomething && isHolding) //���ݿ� ���µ� ���� ���� ��� �ִٸ� ����
        {
            activeObject.GetComponent<Object>().onSomething = true;
            isHolding = false;
            GameObject handleThing = transform.GetChild(1).gameObject;
            handleThing.GetComponent<Handle>().PlayerHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition);
            anim.SetBool("isHolding", isHolding);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !canActive && isHolding) //�տ� �ƹ��͵� ���µ� �� ����ִ� -> �ٴڿ� ������
        {
            isHolding = false;

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
        canActive = true;
        if (activeObject == null)
        {
            activeObject = other.gameObject;
            other.GetComponent<Object>().canActive = true;
            other.GetComponent<Object>().OnHighlight();
        }
        else
        {
            nextActiveObject = other.gameObject;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        canActive = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (activeObject != null && nextActiveObject == null)
        {
            canActive = false;
            if (activeObject.GetComponent<Object>() != null)
            {
                activeObject.GetComponent<Object>().OffHighlight();
            }
            activeObject = null;
        }
        else
        {
            if (other.gameObject == activeObject)
            {
                activeObject.GetComponent<Object>().OffHighlight();
                activeObject = nextActiveObject;
                activeObject.GetComponent<Object>().OnHighlight();
                nextActiveObject = null;
            }
            else
            {
                nextActiveObject = null;
            }
        }
    }
}

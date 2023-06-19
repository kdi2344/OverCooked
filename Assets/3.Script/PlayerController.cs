using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isMoving = false;
    [SerializeField] private bool canActive = false; //내 앞에 테이블 같은거 있는지
    public bool isHolding = false;

    [SerializeField] private Animator anim;
    public float Speed = 10.0f;
    public GameObject activeObject;
    public GameObject nextActiveObject;

    public float rotateSpeed = 10.0f;       // 회전 속도

    float h, v;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && activeObject != null && activeObject.CompareTag("Craft"))
        {
            if (!activeObject.GetComponent<Object>().onSomething && !isHolding) //craft위에 뭐가 없고 나도 안들고 있으면
            {
                activeObject.GetComponent<Craft>().OpenCraft(); //꺼내기
                activeObject.GetComponent<Object>().onSomething = false;
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
            }
            else if(!activeObject.GetComponent<Object>().onSomething && isHolding) //craft위에 뭐가 없고 내가 들고있으면
            {
                activeObject.GetComponent<Object>().onSomething = true;
                transform.GetChild(1).gameObject.GetComponent<Handle>().PlayerHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition); //내려놓기
                isHolding = false;
                anim.SetBool("isHolding", false);
            }
            else if (activeObject.GetComponent<Object>().onSomething && !isHolding) //craft위에 뭐가 있고 내가 안들고있으면 집기
            {
                activeObject.GetComponent<Object>().onSomething = false;
                isHolding = true;
                anim.SetBool("isHolding", isHolding);
                GameObject handleThing = activeObject.transform.parent.GetChild(2).gameObject;
                handleThing.GetComponent<Handle>().PlayerHandle(transform);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Space) && activeObject.CompareTag("CounterTop") && canActive && activeObject.GetComponent<Object>().onSomething && !isHolding) //선반에 무언가 있다면 집기
        {
            activeObject.GetComponent<Object>().onSomething = false;
            isHolding = true;
            anim.SetBool("isHolding", isHolding);
            GameObject handleThing = activeObject.transform.parent.GetChild(2).gameObject;
            handleThing.GetComponent<Handle>().PlayerHandle(transform);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && activeObject.CompareTag("CounterTop") && canActive && !activeObject.GetComponent<Object>().onSomething && isHolding) //선반에 없는데 내가 뭔가 들고 있다면 놓기
        {
            activeObject.GetComponent<Object>().onSomething = true;
            isHolding = false;
            GameObject handleThing = transform.GetChild(1).gameObject;
            handleThing.GetComponent<Handle>().PlayerHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition);
            anim.SetBool("isHolding", isHolding);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !canActive && isHolding) //앞에 아무것도 없는데 뭘 들고있다 -> 바닥에 버리기
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

        // 바라보는 방향으로 회전 후 다시 정면을 바라보는 현상을 막기 위해 설정
        if (!(h == 0 && v == 0))
        {
            isMoving = true;
            // 이동과 회전을 함께 처리
            transform.position += dir * Speed * Time.deltaTime;
            // 회전하는 부분. Point 1.
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

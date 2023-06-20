using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isMoving = false;
    [SerializeField] private bool canActive = false; //내 앞에 테이블 같은거 있는지
    public bool isHolding = false;

    public Animator anim;
    public float Speed = 10.0f;
    public GameObject activeObject;
    public GameObject nextActiveObject;

    public float rotateSpeed = 10.0f;       // 회전 속도

    float h, v;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && activeObject != null)
        {
            if (activeObject.GetComponent<Object>().type == Object.ObjectType.CounterTop || activeObject.GetComponent<Object>().type == Object.ObjectType.Board) //counterTop과 상호작용
            {
                if (canActive && activeObject.GetComponent<Object>().onSomething && !isHolding) //선반에 무언가 있다면 집기
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
                        handleThing.GetComponent<Handle>().isOnDesk = false; //접시
                        handleThing.GetComponent<Handle>().PlayerHandle(transform);
                    }
                }
                else if (canActive && !activeObject.GetComponent<Object>().onSomething && isHolding) //선반에 없는데 내가 뭔가 들고 있다면 놓기
                {
                    GameObject handleThing = transform.GetChild(1).gameObject;
                    if (handleThing.CompareTag("Ingredient"))
                    {
                        activeObject.GetComponent<Object>().onSomething = true;
                        isHolding = false;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = true;
                        handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition);
                    }
                    else //접시
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
            else if (activeObject.GetComponent<Object>().type == Object.ObjectType.Craft) //Crate와 상호작용
            {
                if (!activeObject.GetComponent<Object>().onSomething && !isHolding) //crate위에 뭐가 없고 나도 안들고 있으면
                {
                    activeObject.GetComponent<Craft>().OpenCraft(); //꺼내기
                    activeObject.GetComponent<Object>().onSomething = false;
                    isHolding = true;
                    anim.SetBool("isHolding", isHolding);
                }
                else if (!activeObject.GetComponent<Object>().onSomething && isHolding) //craft위에 뭐가 없고 내가 들고있으면
                {
                    activeObject.GetComponent<Object>().onSomething = true;
                    transform.GetChild(1).gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().isOnDesk = true; //내려놓기
                    transform.GetChild(1).gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandleOff(activeObject.transform.parent, activeObject.transform.parent.GetChild(1).localPosition); //내려놓기
                    isHolding = false;
                    anim.SetBool("isHolding", false);
                }
                else if (activeObject.GetComponent<Object>().onSomething && !isHolding) //crate위에 뭐가 있고 내가 안들고있으면 집기
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
                    else //접시
                    {
                        handleThing.GetComponent<Handle>().isOnDesk = false;
                        handleThing.GetComponent<Handle>().PlayerHandle(transform);
                    }
                }
            }
            else if (activeObject.CompareTag("Ingredient")) //재료와 상호작용
            {

            }
            else
            {

            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !canActive && isHolding) //앞에 아무것도 없는데 뭘 들고있다 -> 바닥에 버리기
        {
            isHolding = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && activeObject.GetComponent<Object>().type == Object.ObjectType.Board && canActive && !activeObject.transform.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Handle>().isCooked)
        {
            if (activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().Pause) //실행되다 만거라면
            {
                anim.SetTrigger("startCut");
                activeObject.transform.GetChild(0).GetComponent<CuttingBoard>().PauseSlider(false);
            }
            else if (activeObject.transform.GetChild(0).GetComponent<CuttingBoard>()._CoTimer == null) //한번도 실행 안된거면 시작 가능
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
        if (other == activeObject) //중복체크
        {
            return;
        }
        if (other.CompareTag("Ingredient"))
        {
            if (activeObject == null && !other.GetComponent<Handle>().isOnDesk)
            {
                activeObject = other.gameObject; //맨땅에 재료만 있는거 줍는다면
                other.GetComponent<Object>().OnHighlight(false);
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
            other.GetComponent<Object>().canActive = true; //Object 스크립트 다 넣어주면 오류 안남
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

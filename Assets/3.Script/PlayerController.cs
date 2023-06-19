using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isMoving = false;
    [SerializeField] private Animator anim;
    public float Speed = 10.0f;
    private bool canActive = false;
    public GameObject activeObject;
    public GameObject nextActiveObject;

    public float rotateSpeed = 10.0f;       // ȸ�� �ӵ�

    float h, v;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && activeObject != null && activeObject.CompareTag("Craft"))
        {
            activeObject.GetComponent<Craft>().OpenCraft();
        }
    }

    // �̵� ���� �Լ��� © ���� Update���� FixedUpdate�� �� ȿ���� ���ٰ� �Ѵ�. �׷��� ����ߴ�.
    void FixedUpdate()
    {
        anim.SetBool("isWalking", isMoving);
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v); // new Vector3(h, 0, v)�� ���� ���̰� �Ǿ����Ƿ� dir�̶�� ������ �ְ� ���� ���ϰ� ����� �� �ְ� ��

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

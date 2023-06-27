using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class busController : MonoBehaviour
{
    public float Speed = 1.0f;
    public float rotateSpeed = 1.0f;
    float h, v;

    private SceneConnect ActiveScene;
    private bool CanGo = false;
    private void Update()
    {
        if (CanGo && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(ActiveScene.SceneName);
        }
    }

    void FixedUpdate()
    {
        //.SetBool("isWalking", isMoving);
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");


        Vector3 dir = new Vector3(h, 0, v);

        // �ٶ󺸴� �������� ȸ�� �� �ٽ� ������ �ٶ󺸴� ������ ���� ���� ����
        if (!(h == 0 && v == 0))
        {
            // �̵��� ȸ���� �Բ� ó��
            transform.position += dir * Speed * Time.deltaTime;
            // ȸ���ϴ� �κ�. Point 1.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
        }
        else
        {
            //isMoving = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CanGo = true;
        ActiveScene = other.GetComponent<SceneConnect>();
    }
    private void OnTriggerExit(Collider other)
    {
        CanGo = false;
        ActiveScene = null;
    }
}

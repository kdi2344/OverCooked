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

        // 바라보는 방향으로 회전 후 다시 정면을 바라보는 현상을 막기 위해 설정
        if (!(h == 0 && v == 0))
        {
            // 이동과 회전을 함께 처리
            transform.position += dir * Speed * Time.deltaTime;
            // 회전하는 부분. Point 1.
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

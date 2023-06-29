using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class busController : MonoBehaviour
{
    [SerializeField] GameObject Stage1UI;
    [SerializeField] GameObject Stage2UI;
    [SerializeField] GameObject Stage3UI;

    public float Speed = 1.0f;
    public float rotateSpeed = 1.0f;
    float h, v;

    private SceneConnect ActiveScene;
    private bool CanGo = false;
    private void Update()
    {
        if (ActiveScene != null && CanGo && Input.GetKeyDown(KeyCode.Space))
        {
            if (ActiveScene.SceneName.Equals("SampleScene"))
            {
                if (!StageManager.instance.stage1Space)
                {
                    StageManager.instance.stage1Space = true;
                }
                else
                {
                    LoadingSceneManager.LoadScene(ActiveScene.SceneName);
                }
            }
            else if (ActiveScene.SceneName.Equals("StageSalad"))
            {
                if (!StageManager.instance.stage2Space)
                {
                    StageManager.instance.stage2Space = true;
                }
                else
                {
                    LoadingSceneManager.LoadScene(ActiveScene.SceneName);
                }
            }
            else if (ActiveScene.SceneName.Equals("StagePotato"))
            {
                if (!StageManager.instance.stage3Space )
                {
                    StageManager.instance.stage3Space = true;
                }
                else
                {
                    LoadingSceneManager.LoadScene(ActiveScene.SceneName);
                }
            }
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
        if (ActiveScene.SceneName.Equals("StageSalad") && !Stage2UI.activeSelf)
        {
            Stage2UI.SetActive(true);
        }
        else if (ActiveScene.SceneName.Equals("SampleScene") && !Stage1UI.activeSelf)
        {
            Stage1UI.SetActive(true);
        }
        else if (ActiveScene.SceneName.Equals("StagePotato") && !Stage3UI.activeSelf)
        {
            Stage3UI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        CanGo = false;
        if (ActiveScene.SceneName.Equals("StageSalad") && Stage3UI.activeSelf)
        {
            Stage3UI.SetActive(false);
        }
        else if (ActiveScene.SceneName.Equals("SampleScene") && Stage1UI.activeSelf)
        {
            Stage1UI.SetActive(false);
        }
        else if (ActiveScene.SceneName.Equals("StagePotato") && Stage2UI.activeSelf)
        {
            Stage2UI.SetActive(false);
        }
        ActiveScene = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public bool goLeft = false;
    private Vector3 pos;
    public float Speed = 1000f;
    public Slider timer;
    private void Awake()
    {
        timer = FindObjectOfType<Slider>();
    }
    private void Update()
    {
        if (goLeft)
        {
            pos = transform.position;
            pos.x -= Speed * Time.deltaTime;
            transform.position = pos;
        }
    }
    private void OnEnable()
    {
        transform.localPosition = GameManager.instance.poolPos;
        goLeft = true;
        StartCoroutine(TimerStart());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //이동을 정지
        goLeft = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        goLeft = false;
    }
    IEnumerator TimerStart()
    {
        while (timer.value > 0)
        {
            yield return new WaitForSeconds(0.1f);
            timer.value -= 1f;
            if (timer.value < timer.maxValue * 0.66f && timer.value > timer.maxValue * 0.33f)
            {
                transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(243/255f, 239/255f, 0, 255/255f);
                //노란색
            }
            else if (timer.value < timer.maxValue * 0.33f)
            {
                transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(215/255f, 11/255f, 0, 1f);
                //빨간색
            }
            else
            {
                transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(0, 192/255f, 5/255f, 255/255f);
                //초록색
            }
        }
        GameManager.instance.MenuFail(gameObject);
    }
}

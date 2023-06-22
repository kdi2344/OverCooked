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

    float duration; // This will be your time in seconds.
    float smoothness = 0.1f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.
    Color Start = new Color(0, 192 / 255f, 5 / 255f, 255 / 255f);
    Color Middle = new Color(243 / 255f, 239 / 255f, 0, 255 / 255f);
    Color End = new Color(215 / 255f, 11 / 255f, 0, 1f);
    Color currentColor; // This is the state of the color in the current interpolation.
    private void Awake()
    {
        timer = transform.GetChild(2).GetChild(0).GetComponent<Slider>();
        duration = timer.maxValue / 2 * 30;
        currentColor = Start;
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
        StartCoroutine(LerpColor1());
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
    

    IEnumerator LerpColor1()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            currentColor = Color.Lerp(Start, Middle, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
            transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = currentColor;
        }
        StartCoroutine(LerpColor2());
    }
    IEnumerator LerpColor2()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            currentColor = Color.Lerp(Middle, End, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
            transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = currentColor;
        }
    }

    IEnumerator TimerStart()
    {
        while (timer.value > 0)
        {
            yield return new WaitForSeconds(0.1f);
            timer.value -= 1f;
        }
        GameManager.instance.MenuFail(gameObject);
    }

}

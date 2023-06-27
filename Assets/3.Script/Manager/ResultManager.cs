using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private StarLimit[] limits;
    [SerializeField] private GameObject[] newStars;
    [SerializeField] private Text[] txtStars;
    [SerializeField] private GameObject success;
    [SerializeField] private GameObject tip;
    [SerializeField] private GameObject fail;
    [SerializeField] private GameObject total;
    private bool canSkip = false;

    private int successMoney;

    private void Awake()
    {
        canSkip = false;
        Time.timeScale = 1;
        SetScoreLimit();
        Invoke("ShowSuccess", 1f);
        Invoke("SetStar", 2f);
    }
    private void Update()
    {
        if (canSkip && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Map");
        }
    }
    private void SetStar()
    {
        if (StageManager.instance.playStage == StageManager.State.stage1)
        {
            if (StageManager.instance.totalMoney > limits[0].oneStarLimit)
            {
                if (StageManager.instance.map1Star<1) StageManager.instance.map1Star = 1;
                StartCoroutine(BiggerStar(newStars[0]));
                if (StageManager.instance.totalMoney > limits[0].twoStarLimit)
                {
                    if (StageManager.instance.map1Star < 2)  StageManager.instance.map1Star = 2;
                    StartCoroutine(BiggerStar(newStars[1]));
                    if (StageManager.instance.totalMoney > limits[0].threeStarLimit)
                    {
                        if (StageManager.instance.map1Star < 3)  StageManager.instance.map1Star = 3;
                        StartCoroutine(BiggerStar(newStars[2]));
                    }
                }
            }
        }
        else if (StageManager.instance.playStage == StageManager.State.stage2)
        {
            if (StageManager.instance.totalMoney > limits[1].oneStarLimit)
            {
                if (StageManager.instance.map2Star < 1) StageManager.instance.map2Star = 1;
                StartCoroutine(BiggerStar(newStars[0]));
                if (StageManager.instance.totalMoney > limits[1].twoStarLimit)
                {
                    if (StageManager.instance.map2Star < 2) StageManager.instance.map2Star = 2;
                    StartCoroutine(BiggerStar(newStars[1]));
                    if (StageManager.instance.totalMoney > limits[1].threeStarLimit)
                    {
                        if (StageManager.instance.map2Star < 3) StageManager.instance.map2Star = 3;
                        StartCoroutine(BiggerStar(newStars[2]));
                    }
                }
            }
        }
        else if (StageManager.instance.playStage == StageManager.State.stage3)
        {
            if (StageManager.instance.totalMoney > limits[2].oneStarLimit)
            {
                if (StageManager.instance.map3Star < 1) StageManager.instance.map3Star = 1;
                StartCoroutine(BiggerStar(newStars[0]));
                if (StageManager.instance.totalMoney > limits[2].twoStarLimit)
                {
                    if (StageManager.instance.map3Star < 2) StageManager.instance.map3Star = 2;
                    StartCoroutine(BiggerStar(newStars[1]));
                    if (StageManager.instance.totalMoney > limits[2].threeStarLimit)
                    {
                        if (StageManager.instance.map3Star < 3) StageManager.instance.map3Star = 3;
                        StartCoroutine(BiggerStar(newStars[2]));
                    }
                }
            }
        }
        canSkip = true;
    }
    IEnumerator BiggerStar(GameObject star)
    {
        star.SetActive(true);
        float time = 0;
        while(time < 1f)
        {
            star.transform.localScale = Vector3.one * (1 + time);
            time += 0.01f;
            yield return new WaitForSeconds(0.005f);
        }
        yield return null;
    }
    private void SetScoreLimit()
    {
        if (StageManager.instance.playStage == StageManager.State.stage1)
        {
            txtStars[0].text = limits[0].oneStarLimit.ToString();
            txtStars[1].text = limits[0].twoStarLimit.ToString();
            txtStars[2].text = limits[0].threeStarLimit.ToString();
            successMoney = limits[0].successMoney;
        }
        else if (StageManager.instance.playStage == StageManager.State.stage2)
        {
            txtStars[0].text = limits[1].oneStarLimit.ToString();
            txtStars[1].text = limits[1].twoStarLimit.ToString();
            txtStars[2].text = limits[1].threeStarLimit.ToString();
            successMoney = limits[1].successMoney;
        }
        else if (StageManager.instance.playStage == StageManager.State.stage3)
        {
            txtStars[0].text = limits[2].oneStarLimit.ToString();
            txtStars[1].text = limits[2].twoStarLimit.ToString();
            txtStars[2].text = limits[2].threeStarLimit.ToString();
            successMoney = limits[2].successMoney;
        }
    }

    private void ShowSuccess()
    {
        success.transform.GetChild(0).GetComponent<Text>().text = "배달된 주문 x " + StageManager.instance.success;
        success.transform.GetChild(0).gameObject.SetActive(true);
        Invoke("ShowSuccessMoney", 1f);
    }
    private void ShowSuccessMoney()
    {
        success.transform.GetChild(1).GetComponent<Text>().text = (StageManager.instance.success * successMoney).ToString();
        success.transform.GetChild(1).gameObject.SetActive(true);
        Invoke("ShowTip", 1f);
    }
    private void ShowTip()
    {
        tip.transform.GetChild(0).gameObject.SetActive(true);
        Invoke("ShowTipMoney", 1f);
    }
    private void ShowTipMoney()
    {
        tip.transform.GetChild(1).GetComponent<Text>().text = (StageManager.instance.tipMoney).ToString();
        tip.transform.GetChild(1).gameObject.SetActive(true);
        Invoke("ShowFail", 1f);
    }
    private void ShowFail()
    {
        fail.transform.GetChild(0).GetComponent<Text>().text = "실패한 주문 x " + StageManager.instance.fail.ToString();
        fail.transform.GetChild(0).gameObject.SetActive(true);
        Invoke("ShowFailMoney", 1f);
    }
    private void ShowFailMoney()
    {
        fail.transform.GetChild(1).GetComponent<Text>().text = "-" + ((int)(StageManager.instance.fail * successMoney * 0.5)).ToString();
        fail.transform.GetChild(1).gameObject.SetActive(true);
        Invoke("ShowTotal", 1f);
    }
    private void ShowTotal()
    {
        total.transform.GetChild(0).gameObject.SetActive(true);
        Invoke("ShowTotalMoney", 1f);
    }
    private void ShowTotalMoney()
    {
        total.transform.GetChild(1).GetComponent<Text>().text = StageManager.instance.totalMoney.ToString();
        total.transform.GetChild(1).gameObject.SetActive(true);
    }
}

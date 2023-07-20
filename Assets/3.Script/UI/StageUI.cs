using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StageUI : MonoBehaviour
{
    enum stage { stage1, stage2, stage3 }
    [SerializeField] stage Current;

    [SerializeField] Sprite fullStar;
    [SerializeField] Transform Target;
    [SerializeField] StarLimit limit;
    Vector3 pos = new Vector3(0, 5, 0);

    private PhotonView pv;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SoundManager.instance.isSingle)
            {
                Check();
            }
            else
            {
                pv.RPC("Check", RpcTarget.All);
            }
            //Check();
        }
        if (Target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(Target.position + pos);
        }
    }
    private void OnEnable()
    {
        if (SoundManager.instance.isSingle)
        {
            Check();
        }
        else
        {
            pv.RPC("Check", RpcTarget.All);
        }
        //Check();
    }

    [PunRPC]
    private void Check()
    {
        if (Current == stage.stage1)
        {
            if (!StageManager.instance.stage1Space)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = limit.oneStarLimit.ToString();
                transform.GetChild(1).GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>().text = limit.twoStarLimit.ToString();
                transform.GetChild(1).GetChild(0).GetChild(2).GetChild(2).GetComponent<Text>().text = limit.threeStarLimit.ToString(); 
                if (StageManager.instance.isClearMap1)
                {
                    for (int i =0; i < StageManager.instance.map1Star; i++)
                    {
                        transform.GetChild(1).GetChild(0).GetChild(1).GetChild(i).GetComponent<Image>().sprite = fullStar;
                    }
                }
            }
        }
        else if (Current == stage.stage2)
        {
            if (!StageManager.instance.stage2Space)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = limit.oneStarLimit.ToString();
                transform.GetChild(1).GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>().text = limit.twoStarLimit.ToString();
                transform.GetChild(1).GetChild(0).GetChild(2).GetChild(2).GetComponent<Text>().text = limit.threeStarLimit.ToString();
                if (StageManager.instance.isClearMap1)
                {
                    for (int i = 0; i < StageManager.instance.map2Star; i++)
                    {
                        transform.GetChild(1).GetChild(0).GetChild(1).GetChild(i).GetComponent<Image>().sprite = fullStar;
                    }
                }
            }
        }
        else if (Current == stage.stage3)
        {
            if (!StageManager.instance.stage3Space)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = limit.oneStarLimit.ToString();
                transform.GetChild(1).GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>().text = limit.twoStarLimit.ToString();
                transform.GetChild(1).GetChild(0).GetChild(2).GetChild(2).GetComponent<Text>().text = limit.threeStarLimit.ToString();
                if (StageManager.instance.isClearMap1)
                {
                    for (int i = 0; i < StageManager.instance.map3Star; i++)
                    {
                        transform.GetChild(1).GetChild(0).GetChild(1).GetChild(i).GetComponent<Image>().sprite = fullStar;
                    }
                }
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}

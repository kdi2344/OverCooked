using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance = null;
    public enum State { start, stage1, stage2, stage3 };
    public State playStage = State.stage1;
    public GameObject setting;

    private bool isStop = false;

    public bool isClearMap1 = false;
    public bool isClearMap2 = false;
    public bool isClearMap3 = false;
    public bool isAllClear = false;

    public int map1Star = 0;
    public int map2Star = 0;
    public int map3Star = 0;

    public bool stage1Space = false;
    public bool stage2Space = false;
    public bool stage3Space = false;

    public int tipMoney = 0;
    public int success = 0;
    public int fail = 0;
    public int totalMoney = 0;
    public int failMoney = 0;
    public int successMoney = 0;

    private void Awake()
    {
        if (SoundManager.instance.Setting == null)
        {
            SoundManager.instance.Setting = setting;
        }
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isStop)
            {
                Time.timeScale = 0;
                SoundManager.instance.Setting = setting;
                SoundManager.instance.Sound = setting.transform.GetChild(3).gameObject;
                SoundManager.instance.Control = setting.transform.GetChild(4).gameObject;
                SoundManager.instance.Resolution = setting.transform.GetChild(5).gameObject;

                setting.SetActive(true);
                isStop = true;
            }
            
            else
            {
                Time.timeScale = 1;
                setting.SetActive(false);
                isStop = true;
            }
        }
    }
    public void SetMap(GameObject[] flags, Material[] flags_m)
    {
        if (stage1Space)
        {
            GameObject[] something = GameObject.FindGameObjectsWithTag("stage1");
            for (int i =0; i < something.Length; i++)
            {
                something[i].transform.GetChild(0).GetComponent<Animator>().enabled = false;
            }
            Material[] materials = flags[0].GetComponent<MeshRenderer>().materials;
            if (map1Star == 0)
            {
                materials[0].mainTexture = flags_m[0].mainTexture;
            }
            else if (map1Star == 1)
            {
                materials[0].mainTexture = flags_m[1].mainTexture;
            }
            else if (map1Star == 2)
            {
                materials[0].mainTexture = flags_m[2].mainTexture;
            }
            else
            {
                materials[0].mainTexture = flags_m[3].mainTexture;
            }
            flags[0].GetComponent<MeshRenderer>().materials = materials;
        }
        if (stage2Space)
        {
            GameObject[] something = GameObject.FindGameObjectsWithTag("stage2");
            for (int i = 0; i < something.Length; i++)
            {
                something[i].transform.GetChild(0).GetComponent<Animator>().enabled = false;
            }
            Material[] materials = flags[1].GetComponent<MeshRenderer>().materials;
            if (map2Star == 0)
            {
                materials[0].mainTexture = flags_m[0].mainTexture;
            }
            else if (map2Star == 1)
            {
                materials[0].mainTexture = flags_m[1].mainTexture;
            }
            else if (map2Star == 2)
            {
                materials[0].mainTexture = flags_m[2].mainTexture;
            }
            else
            {
                materials[0].mainTexture = flags_m[3].mainTexture;
            }
            flags[1].GetComponent<MeshRenderer>().materials = materials;
        }
        if (stage3Space)
        {
            GameObject[] something = GameObject.FindGameObjectsWithTag("stage3");
            for (int i = 0; i < something.Length; i++)
            {
                something[i].transform.GetChild(0).GetComponent<Animator>().enabled = false;
            }
            Material[] materials = flags[1].GetComponent<MeshRenderer>().materials;
            if (map2Star == 0)
            {
                materials[0].mainTexture = flags_m[0].mainTexture;
            }
            else if (map2Star == 1)
            {
                materials[0].mainTexture = flags_m[1].mainTexture;
            }
            else if (map2Star == 2)
            {
                materials[0].mainTexture = flags_m[2].mainTexture;
            }
            else
            {
                materials[0].mainTexture = flags_m[3].mainTexture;
            }
            flags[1].GetComponent<MeshRenderer>().materials = materials;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance = null;
    public enum State { start, stage1, stage2, stage3 };
    public State playStage = State.stage1;

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

    public void SetMap(GameObject[] flags, Material[] flags_m)
    {
        if (isClearMap1)
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
        if (isClearMap2)
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
        if (isClearMap3)
        {

        }
        if (isClearMap3 && !isAllClear)
        {
            isAllClear = true;
        }
    }
}

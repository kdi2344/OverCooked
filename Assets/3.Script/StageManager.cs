using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance = null;

    public bool isClearMap1 = false;
    public bool isClearMap2 = false;
    public bool isClearMap3 = false;
    public bool isAllClear = false;

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

    public void SetMap()
    {
        if (isClearMap1)
        {
            GameObject[] something = GameObject.FindGameObjectsWithTag("stage1");
            for (int i =0; i < something.Length; i++)
            {
                //something[i].transform.GetChild(0).GetComponent<Animator>().SetBool("isClear", true);
                something[i].transform.GetChild(0).GetComponent<Animator>().enabled = false;
            }
        }
        else if (isClearMap1 && !isClearMap2)
        {

        }
        else if (isClearMap2 && !isClearMap3)
        {

        }
        else if (isClearMap3 && !isAllClear)
        {
            isAllClear = true;
        }
    }
}

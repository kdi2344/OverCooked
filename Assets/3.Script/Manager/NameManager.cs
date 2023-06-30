using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameManager : MonoBehaviour
{
    public static NameManager instance = null;
    public string name1;
    public string name2;
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
    public void SetName1(string name)
    {
        if (name.Equals(string.Empty))
        {
            name1 = "Player1";
        }
        else
        {
            name1 = name;
        }
    }
    public void SetName2(string name)
    {
        if (name.Equals(string.Empty))
        {
            name2 = "Player2";
        }
        else
        {
            name2 = name;
        }
    }
}

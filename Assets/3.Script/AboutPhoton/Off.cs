using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Off : MonoBehaviour
{
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "IntroScene")
        {
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    private bool isSpace = false;
    [SerializeField] GameObject Shutter;
    [SerializeField] GameObject SpaceToStart;
    [SerializeField] GameObject buttons;
    private void Awake()
    {
        Camera.main.transform.position = new Vector3(-1.83f, 0.58f, -7.84f);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3 (359.932678f, 7.07423258f, 0.24220185f));
        Shutter.SetActive(true);
        SpaceToStart.SetActive(true);
        buttons.SetActive(false);
    }
    private void Update()
    {
        if (!isSpace && Input.GetKeyDown(KeyCode.Space))
        {
            isSpace = true;
            Camera.main.transform.position = new Vector3(-3.07999992f, -0.449999988f, -9.89999962f);
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0.326138616f, 12.5265751f, 0.704141259f));
            Shutter.SetActive(false);
            SpaceToStart.SetActive(false);
            buttons.SetActive(true);
        }
    }

    public void BtnStart()
    {
        LoadingSceneManager.LoadScene("Map");
    }

    public void BtnSetting()
    {

    }

    public void BtnExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    private bool isSpace = false;
    [SerializeField] GameObject name;
    [SerializeField] GameObject Shutter;
    [SerializeField] GameObject SpaceToStart;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject Setting;
    private void Awake()
    {
        Camera.main.transform.position = new Vector3(-1.83f, 0.58f, -7.84f);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3 (359.932678f, 7.07423258f, 0.24220185f));
        Shutter.SetActive(true);
        SpaceToStart.SetActive(true);
        buttons.SetActive(false);
        name.SetActive(false);
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
            name.SetActive(true);
        }
    }

    public void BtnStart()
    {
        SoundManager.instance.WantChange = true;
        LoadingSceneManager.LoadScene("Map");
    }

    public void BtnSetting()
    {
        Setting.SetActive(true);
        //소리 화면만 켜주기 설정
        SoundManager.instance.ShowSoundTab();
    }

    public void BtnExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void BtnSettingSave()
    {

    }
    public void BtnSettingExit()
    {
        Setting.SetActive(false);
    }
}

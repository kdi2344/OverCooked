using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public bool WantChange = false;
    private Scene currentScene;
    public string StageName;
    public float volumeBGM = 1;
    public float volumeEffect = 1;

    [Header("배경음악들")]
    public AudioSource asBGM; 
    public AudioClip introBGM;   
    public AudioClip mapBGM; 
    public AudioClip stage1BGM; 
    public AudioClip stage2BGM; 
    public AudioClip stage3BGM; 
    public AudioClip resultBGM; 

    [Header("효과음들")]
    public AudioSource asEffect;
    public AudioClip btn;
    public AudioClip ready;
    public AudioClip go;
    public AudioClip mapExpose;
    public AudioClip mapRoad;
    public AudioClip itemTake;
    public AudioClip itemPut;
    public AudioClip itemThrow;
    public AudioClip dash;
    public AudioClip rightDish;
    public AudioClip dontMenu;
    public AudioClip menuTimesUp;
    public AudioClip timesUp;
    public AudioClip bin;
    public AudioClip add;
    public AudioClip respawn;
    public AudioClip fall;
    public AudioClip cut;
    public AudioClip victory;
    public AudioClip star1;
    public AudioClip star2;
    public AudioClip star3;
    public AudioClip van1;
    public AudioClip van2;
    public AudioClip van3;
    public AudioClip van4;
    public AudioClip vanEngine;
    public AudioClip start;
    public AudioClip beep;

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

        asBGM.loop = true;
        asBGM.playOnAwake = true;
        PlayBGM("IntroScene");
        asEffect.loop = false;
        asEffect.playOnAwake = false;
    }
    void Start()
    {
        //if (SceneManager.GetActiveScene().name.Equals("IntroScene"))
        //{
        //    PlayBGM("IntroScene");
        //}
        //else if (SceneManager.GetActiveScene().name.Equals("LoadingScene"))
        //{
        //    while (asBGM.volume > 0)
        //    {
        //        asBGM.volume -= Time.deltaTime;
        //    }

        //    PlayBGM(FindObjectOfType<LoadingSceneManager>().nextName);
        //}
    }

    public void upBGM()
    {
        volumeBGM += 0.1f;
        if (volumeBGM >= 1)
        {
            volumeBGM = 1;
        }
        asBGM.volume = volumeBGM;
    }
    public void downBGM()
    {
        volumeBGM -= 0.1f;
        if (volumeBGM <= 0)
        {
            volumeBGM = 0;
        }
        asBGM.volume = volumeBGM;
    }

    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
        currentScene = scene;
        if (asBGM != null && WantChange && scene.name == "LoadingScene")
        {
            StartCoroutine(FadeOutMusic());
        }
        else if (scene.name == "ResultScene")
        {
            PlayBGM("ResultScene");
        }
        
    }
    private IEnumerator FadeOutMusic()
    {
        while (asBGM.volume > 0)
        {
            asBGM.volume -= Time.deltaTime*2;
            yield return new WaitForSeconds(Time.deltaTime*0.5f);
        }
        if (FindObjectOfType<LoadingSceneManager>().nextName.Equals("SampleScene") || FindObjectOfType<LoadingSceneManager>().nextName.Equals("StageSalad") || FindObjectOfType<LoadingSceneManager>().nextName.Equals("StagePotato"))
        {
            StageName = FindObjectOfType<LoadingSceneManager>().nextName;
        }
        else
        {
            PlayBGM(FindObjectOfType<LoadingSceneManager>().nextName);
            StartCoroutine(FadeInMusic());
        }
    }
    private IEnumerator FadeInMusic()
    {
        while (asBGM.volume < volumeBGM)
        {
            asBGM.volume += Time.deltaTime*2;
            yield return new WaitForSeconds(Time.deltaTime*0.5f);
        }
    }
    public void StagePlay(string name)
    {
        asBGM.volume = volumeBGM;
        PlayBGM(name);
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void PlayBGM(string bgm)
    {
        switch (bgm)
        {
            case "IntroScene":
                asBGM.clip = introBGM;
                break;
            case "Map":
                asBGM.clip = mapBGM;
                break;
            case "SampleScene":
                asBGM.clip = stage1BGM;
                break;
            case "StageSalad":
                asBGM.clip = stage2BGM;
                break;
            case "StagePotato":
                asBGM.clip = stage3BGM;
                break;
            case "ResultScene":
                asBGM.clip = resultBGM;
                break;
        }
        //asBGM.volume = volume;
        asBGM.pitch = 1;
        asBGM.Play();
    }

    public void PlayEffect(string effect)
    {
        switch (effect)
        {
            case "btn":
                asEffect.clip = btn;
                break;
            case "ready":
                asEffect.clip = ready;
                break;
            case "go":
                asEffect.clip = go;
                break;
            case "mapExpose":
                asEffect.clip = mapExpose;
                break;
            case "mapRoad":
                asEffect.clip = mapRoad;
                break;
            case "put":
                asEffect.clip = itemPut;
                break;
            case "take":
                asEffect.clip = itemTake;
                break;
            case "throw":
                asEffect.clip = itemThrow;
                break;
            case "dash":
                asEffect.clip = dash;
                break;
            case "right":
                asEffect.clip = rightDish;
                break;
            case "no":
                asEffect.clip = dontMenu;
                break;
            case "wrong":
                asEffect.clip = menuTimesUp;
                break;
            case "timesUp":
                asEffect.clip = timesUp;
                break;
            case "bin":
                asEffect.clip = bin;
                break;
            case "add":
                asEffect.clip = add;
                break;
            case "respawn":
                asEffect.clip = respawn;
                break;
            case "fall":
                asEffect.clip = fall;
                break;
            case "cut":
                asEffect.clip = cut;
                break;
            case "victory":
                asEffect.clip = victory;
                break;
            case "star1":
                asEffect.clip = star1;
                break;
            case "star2":
                asEffect.clip = star2;
                break;
            case "star3":
                asEffect.clip = star3;
                break;
            case "van1":
                asEffect.clip = van1;
                break;
            case "van2":
                asEffect.clip = van2;
                break;
            case "van3":
                asEffect.clip = van3;
                break;
            case "van4":
                asEffect.clip = van4;
                break;
            case "engine":
                asEffect.clip = vanEngine;
                break;
            case "start":
                asEffect.clip = start;
                break;
            case "beep":
                asEffect.clip = beep;
                break;
        }
        asEffect.volume = volumeEffect;
        asEffect.PlayOneShot(asEffect.clip);
    }
}

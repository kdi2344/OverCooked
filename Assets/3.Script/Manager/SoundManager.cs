using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum KeyAction1 { CUT, THROW, RUN, ACTIVE, KEYCOUNT }
public static class KeySetting1 { public static Dictionary<KeyAction1, KeyCode> keys = new Dictionary<KeyAction1, KeyCode>(); }
public static class KeySetting2 { public static Dictionary<KeyAction1, KeyCode> keys = new Dictionary<KeyAction1, KeyCode>(); }

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    private bool isStop = false;
    //�ػ� ����
    List<Resolution> resolutions = new List<Resolution>();
    [SerializeField] private Dropdown resolutionDropdown;
    int resolutionNum;
    FullScreenMode screenMode;
    public Toggle fullscreenBtn;

    //���� â
    [SerializeField] public GameObject Setting;
    [SerializeField] public GameObject Sound;
    [SerializeField] public GameObject Control;
    [SerializeField] public GameObject Resolution;

    //�� �ٲ𶧸� �����ؼ� �ڿ������� bgm �ٲٴ� �뵵
    public bool WantChange = false;
    private Scene currentScene;
    public string StageName;

    //����Ű �ٲٴ� ����
    [SerializeField] Text[] txt1;
    [SerializeField] Text[] txt2;
    KeyCode[] defaultKeys1 = new KeyCode[] { KeyCode.C, KeyCode.Q, KeyCode.LeftShift, KeyCode.Space };
    KeyCode[] defaultKeys2 = new KeyCode[] { KeyCode.KeypadEnter, KeyCode.Semicolon, KeyCode.Quote, KeyCode.RightShift };

    [Header("���� ����")]
    public float volumeBGM = 1f;
    public float volumeEffect = 1f;
    [SerializeField] private GameObject[] BGMSquares; //���� �׸�׸�
    [SerializeField] private GameObject[] effectSquares;

    [Header("������ǵ�")]
    public AudioSource asBGM; 
    public AudioClip introBGM;   
    public AudioClip mapBGM; 
    public AudioClip stage1BGM; 
    public AudioClip stage2BGM; 
    public AudioClip stage3BGM; 
    public AudioClip resultBGM; 

    [Header("ȿ������")]
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!currentScene.name.Equals("LoadingScene") && (!currentScene.name.Equals("IntroScene")))
            {
                if (!isStop)
                {
                    isStop = true;
                    Time.timeScale = 0;
                    Setting.SetActive(true);
                    ShowSoundTab();
                }
                else
                {
                    isStop = false;
                    Time.timeScale = 1;
                    Setting.SetActive(false);
                }
            }
        }
    }
    void Start()
    {
        for (int i = 0; i < (int)KeyAction1.KEYCOUNT; i++)
        {
            KeySetting1.keys.Add((KeyAction1) i, defaultKeys1[i]);
            KeySetting2.keys.Add((KeyAction1) i, defaultKeys2[i]);
        }
        SetBtnText();
        ResolutionOptions();
    }
    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }
    public void ResolutionOptions()
    {
        for (int i =0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        resolutions.AddRange(Screen.resolutions);
        resolutionDropdown.options.Clear();
        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + " X " + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();
        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }
    public void OffSetting()
    {
        Setting.SetActive(false);
        Time.timeScale = 1;
    }
    public void FullScreen(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    public void OKResolutionbtn()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
    public void ShowSoundTab()
    {
        Sound.SetActive(true);
        Control.SetActive(false);
        Resolution.SetActive(false);
    }
    public void ShowControlTab()
    {
        Sound.SetActive(false);
        Control.SetActive(true);
        Resolution.SetActive(false);
    }
    public void ShowResolutionTab()
    {
        Sound.SetActive(false);
        Control.SetActive(false);
        Resolution.SetActive(true);
    }

    private void SetBtnText()
    { //8 9 10 11   12 13 14 15
        for (int i = 0; i < txt1.Length; i++)
        {
            txt1[i] = Setting.transform.GetChild(4).GetChild(i + 8).GetChild(0).GetComponent<Text>();
        }
        for (int i = 0; i < txt2.Length; i++)
        {
            txt2[i] = Setting.transform.GetChild(4).GetChild(i + 12).GetChild(0).GetComponent<Text>();
        }
        for (int i = 0; i < txt1.Length; i++)
        {
            txt1[i].text = KeySetting1.keys[(KeyAction1)i].ToString();
        }
        for (int i = 0; i < txt2.Length; i++)
        {
            txt2[i].text = KeySetting2.keys[(KeyAction1)i].ToString();
        }
    }
    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if (keyEvent.isKey)
        {
            if (key != -1)
            {
                KeySetting1.keys[(KeyAction1)key] = keyEvent.keyCode;
                key = -1;
            }
            else
            {
                KeySetting2.keys[(KeyAction1)key2] = keyEvent.keyCode;
                key2 = -1;
            }
            SetBtnText();
        }
    }
    int key = -1;
    int key2 = -1;
    public void ChangeKey(int num)
    {
        key = num;
    }
    public void ChangeKey2(int num)
    {
        key2 = num;
    }
    private void SetBGMSquares()
    {
            int j = 0;
        for (float i =0; i < 1; i+=0.1f)
        {
            if (i < volumeBGM) //���ִ°�
            {
                BGMSquares[j].transform.GetChild(0).gameObject.SetActive(false);
                BGMSquares[j].transform.GetChild(1).gameObject.SetActive(true);
            }
            else //���ִ°�
            {
                BGMSquares[j].transform.GetChild(0).gameObject.SetActive(true);
                BGMSquares[j].transform.GetChild(1).gameObject.SetActive(false);
            }
            j++;
        }
    }
    private void SetEffectSquares()
    {
            int j = 0;
        for (float i = 0; i < 1; i+=0.1f)
        {
            if (i < volumeEffect) //���ִ°�
            {
                effectSquares[j].transform.GetChild(0).gameObject.SetActive(false);
                effectSquares[j].transform.GetChild(1).gameObject.SetActive(true);
            }
            else //���ִ°�
            {
                effectSquares[j].transform.GetChild(0).gameObject.SetActive(true);
                effectSquares[j].transform.GetChild(1).gameObject.SetActive(false);
            }
            j++;
        }
    }
    public void upBGM()
    {
        volumeBGM += 0.1f;
        if (volumeBGM >= 1)
        {
            volumeBGM = 1f;
        }
        asBGM.volume = volumeBGM;
        SetBGMSquares();
    }
    public void downBGM()
    {
        volumeBGM -= 0.1f;
        if (volumeBGM <= 0)
        {
            volumeBGM = 0;
        }
        asBGM.volume = volumeBGM;
        SetBGMSquares();
    }

    public void effectUp()
    {
        volumeEffect += 0.1f;
        if (volumeEffect >= 1)
        {
            volumeEffect = 1f;
        }
        asEffect.volume = volumeEffect;
        SetEffectSquares();
    }
    public void effectDown()
    {
        volumeEffect -= 0.1f;
        if (volumeEffect <= 0)
        {
            volumeEffect = 0;
        }
        asEffect.volume = volumeEffect;
        SetEffectSquares();
    }

    void OnEnable()
    {
        // �� �Ŵ����� sceneLoaded�� ü���� �Ǵ�.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
        if (Setting == null)
        {
            Setting = FindObjectOfType<setting>().gameObject;
        }
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

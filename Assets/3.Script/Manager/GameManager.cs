using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public PhotonView pv;

    private Coroutine alphaCo = null;
    public enum State { stage1, stage2, stage3 };
    [SerializeField] State state = State.stage1;
    public Coroutine activeCo = null;

    public bool isStop = true; //�Ͻ����� ����
    private bool move = false;

    //�����ϱ� �� ready go�� ���� ����
    [SerializeField] private float StartTime = 0;
    [SerializeField] private GameObject Ready;
    [SerializeField] private GameObject Go;
    [SerializeField] private GameObject Timesup;
    private bool PlayOnce = false;
    private bool PlayTwice = false;
    private bool StartSetting = false;
    private bool once = false;
    private bool isDone = false;
    private float lastSec = 0f;
    private float countSec = 0f;

    //�ð� UI
    [SerializeField] public float GameTime = 160f;
    [SerializeField] private Slider TimeSlider;
    [SerializeField] private Text TextTime;
    [SerializeField] private GameObject SandTimer;

    //�� UI
    public int OriginalMoney;
    public int Player1Money;
    public int Player2Money;
    public GameObject OppositeUI;

    [SerializeField] private GameObject CoinOb;
    [SerializeField] private Slider TipSlider;
    [SerializeField] private int Coin;
    [SerializeField] private int Tip;
    [SerializeField] private Text TextCoin;
    [SerializeField] private Text TextTip;
    public int tipCombo;
    [SerializeField] private GameObject flame;
    [SerializeField] GameObject TextPrefabs;

    [SerializeField] public GameObject wrong;
    //[SerializeField] private GameObject right;

    //���� ������ ����
    [SerializeField] private GameObject platePrefabs;
    [SerializeField] private float respawnTime = 3f;
    [SerializeField] private GameObject ReturnCounter;

    [SerializeField] private Menu[] Menus; //�̹� ���������� ������ �޴���
    [SerializeField] private int maxMenuLimit; //�̹� ������������ �ִ�� ���� �� �ִ� �޴� ������
    [SerializeField] private GameObject[] Single_Double_PoolUIs; //������Ʈ Ǯ������ �� ���� �޴� UI��
    [SerializeField] private GameObject[] Triple_PoolUIs; //������Ʈ Ǯ������ �� 3��¥�� �޴� UI��
    public List<Menu> CurrentOrder;
    public List<GameObject> CurrentOrderUI;
    public Vector3 poolPos;
    [SerializeField] private GameObject Canvas;

    public int i = -1;
    private int j = -1;

    float duration = 75; 
    float smoothness = 0.1f; 
    Color Start = new Color(0, 192 / 255f, 5 / 255f, 255 / 255f); //�ʷ�
    Color Middle = new Color(243 / 255f, 239 / 255f, 0, 255 / 255f); //���
    Color End = new Color(215 / 255f, 11 / 255f, 0, 1f); //����
    Color currentColor;

    IEnumerator LerpColor1()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            currentColor = Color.Lerp(Start, Middle, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
            TimeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = currentColor;
        }
        StartCoroutine(LerpColor2());
    }
    IEnumerator LerpColor2()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            currentColor = Color.Lerp(Middle, End, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
            TimeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = currentColor;
        }

    }
    
    public IEnumerator TurnAlpha(GameObject panel)
    {
        while (panel.GetComponent<Image>().color.a < 0.4f)
        {
            float alpha = panel.GetComponent<Image>().color.a;
            alpha += 0.05f;
            panel.GetComponent<Image>().color = new Color(panel.GetComponent<Image>().color.r, panel.GetComponent<Image>().color.g, panel.GetComponent<Image>().color.b, alpha);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(TurnAlphaZero(panel));
    }
    public IEnumerator TurnAlphaZero(GameObject panel)
    {
        while (panel.GetComponent<Image>().color.a > 0)
        {
            float alpha = panel.GetComponent<Image>().color.a;
            alpha -= 0.05f;
            panel.GetComponent<Image>().color = new Color(panel.GetComponent<Image>().color.r, panel.GetComponent<Image>().color.g, panel.GetComponent<Image>().color.b, alpha);
            yield return new WaitForSeconds(0.01f);
        }
        alphaCo = null;
    }


    void Awake()
    {
        if (!SoundManager.instance.isSingle)
        {
            SoundManager.instance.asBGM.volume = 0;
            SoundManager.instance.alreadyPlayed = false;
            PhotonNetwork.AutomaticallySyncScene = true;
            pv = GetComponent<PhotonView>();
            OppositeUI.SetActive(true);
            OppositeUI.transform.GetChild(2).GetComponent<Text>().text = PhotonNetwork.PlayerListOthers[0].NickName;
            PhotonNetwork.PlayerList[0].SetCustomProperties(new Hashtable() { { "OppositeMoney", -1 } });
            PhotonNetwork.PlayerList[1].SetCustomProperties(new Hashtable() { { "OppositeMoney", -1 } });
        }
        else
        {
            OppositeUI.SetActive(false);
        }

        duration = GameTime / 2;
        currentColor = TimeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color;
        if (null == instance)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //if (!SoundManager.instance.isSingle)
        //{
        //    Destroy(FindObjectOfType<PlayerController>().gameObject);
        //    Destroy(FindObjectOfType<Player2Controller>().gameObject);
        //}

        if (StageManager.instance != null)
        {
            StageManager.instance.success = 0;
            StageManager.instance.tipMoney = 0;
            StageManager.instance.fail = 0;
            StageManager.instance.totalMoney = 0;
            StageManager.instance.successMoney = 0;
            StageManager.instance.failMoney = 0;
            if (state == State.stage1)
            {
                StageManager.instance.playStage = StageManager.State.stage1;
            }
            else if (state == State.stage2)
            {
                StageManager.instance.playStage = StageManager.State.stage2;
            }
            else if (state == State.stage3)
            {
                StageManager.instance.playStage = StageManager.State.stage3;
            }
        }

        isStop = true;
        TimeSlider.maxValue = GameTime;
        TimeSlider.value = TimeSlider.maxValue;
        Coin = 0;
        SetCoinText();
        StartCoroutine(LerpColor1());
    }
    private void Update()
    {
        if (isStop && !StartSetting)
        {
            ToClock();
            StartTime += Time.unscaledDeltaTime;
            Time.timeScale = 0;
            if (StartTime > 1 && !PlayOnce)
            {
                SoundManager.instance.PlayEffect("ready");
                Ready.SetActive(true);
                PlayOnce = true;
            }
            else if (StartTime > 1 && PlayOnce && Ready.transform.localScale.x < 1)
            {
                Vector3 scale = Ready.transform.localScale;
                scale.x += Time.unscaledDeltaTime;
                scale.y += Time.unscaledDeltaTime;
                scale.z += Time.unscaledDeltaTime;
                Ready.transform.localScale = scale;
            }
            else if (StartTime > 4 && PlayOnce && Ready.transform.localScale.x >= 1 && !PlayTwice)
            {
                Ready.SetActive(false);
                PlayTwice = true;
                Go.SetActive(true);
                SoundManager.instance.PlayEffect("go");
            }
            else if (PlayTwice && Go.transform.localScale.x < 1)
            {
                Vector3 scale = Go.transform.localScale;
                scale.x += Time.unscaledDeltaTime;
                scale.y += Time.unscaledDeltaTime;
                scale.z += Time.unscaledDeltaTime;
                Go.transform.localScale = scale;
            }
            else if (StartTime > 6 && PlayTwice && Go.transform.localScale.x > 1)
            {
                SoundManager.instance.StagePlay(SoundManager.instance.StageName);
                if (!SoundManager.instance.isSingle) SoundManager.instance.StagePlay("SampleScene");
                Go.SetActive(false);
                isStop = false;
                StartSetting = true;
            }
        }
        else if (StartSetting && !once)
        {
            isStop = false;
            Time.timeScale = 1;
            once = true;
            Invoke("MakeOrder", 0.5f);
            Invoke("MakeOrder", 5f);
            Invoke("MakeOrder", 30f);
            Invoke("MakeOrder", 80f);
            Invoke("MakeOrder", 150f);
        }
        else if (SoundManager.instance.isSingle && StartSetting && once)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (isStop)
                {
                    Time.timeScale = 1;
                    isStop = false;
                }
                else
                {
                    Time.timeScale = 0;
                    isStop = true;
                }
            }
        }
       
        
        GameTime -= Time.deltaTime;
        ToClock();
        if (GameTime < 30 && SoundManager.instance.asBGM.pitch == 1)
        {
            SoundManager.instance.asBGM.pitch = 1.5f;
        }
        else if (GameTime < 15 && SoundManager.instance.asBGM.pitch == 1.5f)
        {
            SoundManager.instance.asBGM.pitch = 2;
        }

        if (GameTime < 30)
        {
            countSec += Time.deltaTime;
            SandTimer.GetComponent<Animator>().SetTrigger("shake");
        }
        if (countSec >=1)
        {
            countSec = 0;
            SoundManager.instance.PlayEffect("beep");
        }
        if (GameTime <= 0 && !isDone) //�ð� ������ ���߱�
        {
            SoundManager.instance.asBGM.pitch = 1;
            SoundManager.instance.asBGM.Stop();
            SoundManager.instance.PlayEffect("timesUp");
            Time.timeScale = 0;
            if (StageManager.instance != null) StageManager.instance.totalMoney = Coin;
            Timesup.SetActive(true);
            if (StageManager.instance != null && StageManager.instance.totalMoney >= 0)
            {
                if (state == State.stage1)
                {
                    StageManager.instance.isClearMap1 = true;
                }
                else if (state == State.stage2)
                {
                    StageManager.instance.isClearMap2 = true;
                }
                else if (state == State.stage3)
                {
                    StageManager.instance.isClearMap3 = true;
                }
            }
            isDone = true;
        }
        else if (GameTime <= 0 && isDone)
        {
            if (Timesup.transform.localScale.x < 1)
            {
                Vector3 scale = Timesup.transform.localScale;
                scale.x += Time.unscaledDeltaTime;
                scale.y += Time.unscaledDeltaTime;
                scale.z += Time.unscaledDeltaTime;
                Timesup.transform.localScale = scale;
            }
            else 
            {
                if (SoundManager.instance.isSingle)
                { //�̱�
                    lastSec += Time.unscaledDeltaTime;
                    if (lastSec > 1)
                    {
                        SceneManager.LoadScene("ResultScene");
                    }
                }
                else
                { //��Ƽ
                    lastSec += Time.unscaledDeltaTime;
                    if (lastSec > 1 && !move)
                    {
                        PhotonNetwork.AutomaticallySyncScene = true;
                        //SceneManager.LoadScene("FightResultScene");
                        pv.RPC("LoadResult", RpcTarget.All);
                        move = true;
                    }
                }
            }
        }

    }

    [PunRPC]
    public void LoadResult()
    {
        PhotonNetwork.LoadLevel("FightResultScene");
        Debug.Log("����");
    }

    private void ToClock()
    {
        TimeSlider.value = GameTime;
        int min = 0;
        int sec = 0;
        min = (int)((int)GameTime / 60);
        sec = (int)((int)GameTime % 60);
        TextTime.text = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", sec);
    }
    public void PlateReturn()
    {
        StartCoroutine(PlateReturn_co());
    }
    IEnumerator PlateReturn_co()
    {
        yield return new WaitForSeconds(respawnTime);
        GameObject newPlate = Instantiate(platePrefabs, Vector3.zero, Quaternion.identity);
        if (state == State.stage2)
        {
            newPlate.GetComponent<Plates>().limit = 3;
        }
        else if (state == State.stage3)
        {
            newPlate.GetComponent<Plates>().limit = 3;
        }
        newPlate.transform.SetParent(ReturnCounter.transform);
        ReturnCounter.transform.GetChild(1).GetComponent<Return>().returnPlates.Add(newPlate);
        Vector3 spawnPos = ReturnCounter.transform.GetChild(1).GetComponent<Return>().SetPosition();
        newPlate.transform.localPosition = spawnPos;
        newPlate.GetComponent<Plates>().Canvas = Canvas;
    }

    public void MakeOrder()
    {
        if (CurrentOrder.Count >= maxMenuLimit)
        {
            return;
        }
        i = -1;
        j = -1;
        i = Random.Range(0, Menus.Length);
        if (Menus[i].Ingredient.Count == 1) //��ᰡ �Ѱ�
        {
            for (j = 0; j < Single_Double_PoolUIs.Length; j++)
            {
                if (!Single_Double_PoolUIs[j].activeSelf) //�����ִ°� ã�Ƽ�
                {
                    Single_Double_PoolUIs[j].SetActive(true); //���ְ�
                    Single_Double_PoolUIs[j].transform.GetChild(1).gameObject.SetActive(false); //�ι�° ��� �κ��� ����
                    Single_Double_PoolUIs[j].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Menus[i].IngredientIcon[0]; //ù��° ��� ������ �ٲٰ�
                    Single_Double_PoolUIs[j].transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = Menus[i].MenuIcon; //�޴� ������ �ٲٰ�
                    Single_Double_PoolUIs[j].transform.GetChild(2).GetChild(0).GetComponent<Slider>().maxValue = Menus[i].LimitTime; //�����̴� �ð���� �Ҵ�
                    Single_Double_PoolUIs[j].transform.GetChild(2).GetChild(0).GetComponent<Slider>().value = Menus[i].LimitTime; //Ǯ�� ����
                    CurrentOrder.Add(Menus[i]);
                    CurrentOrderUI.Add(Single_Double_PoolUIs[j]);
                    return;
                }
                else //�� ���������� ����
                {
                    continue;
                }
            }
        }
        else if (Menus[i].Ingredient.Count == 2) //��ᰡ �ΰ�
        {
            for (j = 0; j < Single_Double_PoolUIs.Length; j++)
            {
                if (!Single_Double_PoolUIs[j].activeSelf) //�����ִ°� ã�Ƽ�
                {
                    Single_Double_PoolUIs[j].SetActive(true); //���ְ�
                    Single_Double_PoolUIs[j].transform.GetChild(1).gameObject.SetActive(true); //�ι�° ��� �κ� Ű��
                    Single_Double_PoolUIs[j].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Menus[i].IngredientIcon[0]; //ù��° ��� ������ �ٲٰ�
                    Single_Double_PoolUIs[j].transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Menus[i].IngredientIcon[1]; //�ι�° ��� ������ �ٲٰ�
                    Single_Double_PoolUIs[j].transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = Menus[i].MenuIcon; //�޴� ������ �ٲٰ�
                    Single_Double_PoolUIs[j].transform.GetChild(2).GetChild(0).GetComponent<Slider>().maxValue = Menus[i].LimitTime; //�����̴� �ð���� �Ҵ�
                    Single_Double_PoolUIs[j].transform.GetChild(2).GetChild(0).GetComponent<Slider>().value = Menus[i].LimitTime; //Ǯ�� ����
                    CurrentOrder.Add(Menus[i]);
                    CurrentOrderUI.Add(Single_Double_PoolUIs[j]);
                    return;
                }
                else //�� ���������� ����
                {
                    continue;
                }
            }
        }
        else //��ᰡ ����
        {
            for (j = 0; j < Triple_PoolUIs.Length; j++)
            {
                if (!Triple_PoolUIs[j].activeSelf) //�����ִ°� ã�Ƽ�
                {
                    Triple_PoolUIs[j].SetActive(true); //���ְ�
                    Triple_PoolUIs[j].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Menus[i].IngredientIcon[0]; //ù��° ��� ������ �ٲٰ�
                    Triple_PoolUIs[j].transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Menus[i].IngredientIcon[1]; //�ι�° ��� ������ �ٲٰ�
                    Triple_PoolUIs[j].transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = Menus[i].IngredientIcon[2]; //����° ��� ������ �ٲٰ�
                    Triple_PoolUIs[j].transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = Menus[i].MenuIcon; //�޴� ������ �ٲٰ�
                    Triple_PoolUIs[j].transform.GetChild(2).GetChild(0).GetComponent<Slider>().maxValue = Menus[i].LimitTime; //�����̴� �ð���� �Ҵ�
                    Triple_PoolUIs[j].transform.GetChild(2).GetChild(0).GetComponent<Slider>().value = Menus[i].LimitTime; //Ǯ�� ����
                    CurrentOrder.Add(Menus[i]);
                    CurrentOrderUI.Add(Triple_PoolUIs[j]);
                    return;
                }
                else //�� ���������� ����
                {
                    continue;
                }
            }
        }
    }
    private void SuccessEffect() 
    {
        //CurrentOrderUI[i].GetComponent<>
        //textColor = Color.Lerp(Color.white, Start, progress);
    }

    public bool CheckMenu(List<Handle.HandleType> containIngredients) //plate�� ��� list�� ������ �޾Ƽ� ��
    {
        OriginalMoney = Coin;
        if (containIngredients == null) //�� ���ø� ���� ������ Ż��
        {
            //������ ��
            //StartCoroutine(TurnAlpha(wrong));
            return false;
        }
        else
        {
            for (int i = 0; i < CurrentOrder.Count; i++) //���� ���� order���� ���Ѵ�
            {
                if (containIngredients.Count != CurrentOrder[i].Ingredient.Count) //���� ��� �������� �ٸ��� ��
                {
                    //StartCoroutine(TurnAlpha(wrong));
                    continue;
                }
                else //���� ��� ������ ���ٸ�
                {
                    if (containIngredients.Count == 1) //�� ��ᰡ �ϳ���
                    {
                        if (containIngredients[0] == CurrentOrder[i].Ingredient[0])
                        {
                            if (i == 0) //������� �޴��� �´ٸ� �޺�
                            {
                                tipCombo += 1;
                                if (tipCombo >= 4)
                                {
                                    if (!flame.activeSelf)
                                    {
                                        flame.SetActive(true);
                                    }
                                    tipCombo = 4; //�ִ� 4�޺�����
                                }
                            }
                            else
                            {
                                flame.SetActive(false);
                                tipCombo = 0;
                            }
                            CurrentOrderUI[i].transform.position = poolPos;
                            CurrentOrderUI[i].SetActive(false);
                            if (StageManager.instance != null) StageManager.instance.successMoney += CurrentOrder[i].Price;
                            Coin += CurrentOrder[i].Price;
                            CoinOb.transform.parent.GetChild(1).GetComponent<Animator>().SetTrigger("spin");
                            AddTip(i);
                            SetPosition(i);
                            CurrentOrder.RemoveAt(i);
                            CurrentOrderUI.RemoveAt(i);
                            if (StageManager.instance != null) StageManager.instance.success += 1;
                            return true;
                        }
                    }
                    else if (containIngredients.Count == 2) //�ΰ��϶� ��
                    {
                        if ((containIngredients[0] == CurrentOrder[i].Ingredient[0] && containIngredients[1] == CurrentOrder[i].Ingredient[1]) || (containIngredients[1] == CurrentOrder[i].Ingredient[0] && containIngredients[0] == CurrentOrder[i].Ingredient[1]))
                        {
                            if (i == 0) //������� �޴��� �´ٸ� �޺�
                            {
                                tipCombo += 1;
                                if (tipCombo >= 4)
                                {
                                    if (!flame.activeSelf)
                                    {
                                        flame.SetActive(true);
                                    }
                                    tipCombo = 4; //�ִ� 4�޺�����
                                }
                            }
                            else
                            {
                                flame.SetActive(false);
                                tipCombo = 0;
                            }
                            CurrentOrderUI[i].transform.position = poolPos;
                            CurrentOrderUI[i].SetActive(false);
                            if (StageManager.instance != null) StageManager.instance.successMoney += CurrentOrder[i].Price;
                            Coin += CurrentOrder[i].Price;
                            CoinOb.transform.parent.GetChild(1).GetComponent<Animator>().SetTrigger("spin");
                            AddTip(i);
                            SetPosition(i);
                            CurrentOrder.RemoveAt(i);
                            CurrentOrderUI.RemoveAt(i);
                            if (StageManager.instance != null) StageManager.instance.success += 1;
                            return true;
                        }
                    }
                    else if (containIngredients.Count == 3) //�� ���϶� �ҰŸ� �� �ϴ��� ������
                    {//containIngredients 3���� CurrentOrderUI[i]�� ingredient ��
                        int count = 0;
                        for (int j =0; j < containIngredients.Count; j++)
                        {
                            for (int k =0; k < CurrentOrder[i].Ingredient.Count; k++)
                            {
                                if (containIngredients[j].Equals(CurrentOrder[i].Ingredient[k]))
                                {
                                    count++;
                                    break;
                                }
                            }
                        }
                        if (count == 3)
                        {
                            if (i == 0) //������� �޴��� �´ٸ� �޺�
                            {
                                tipCombo += 1;
                                if (tipCombo >= 4)
                                {
                                    if (!flame.activeSelf)
                                    {
                                        flame.SetActive(true);
                                    }
                                    tipCombo = 4; //�ִ� 4�޺�����
                                }
                            }
                            else
                            {
                                flame.SetActive(false);
                                tipCombo = 0;
                            }
                            CurrentOrderUI[i].transform.position = poolPos;
                            CurrentOrderUI[i].SetActive(false);
                            if (StageManager.instance != null) StageManager.instance.successMoney += CurrentOrder[i].Price;
                            Coin += CurrentOrder[i].Price;
                            CoinOb.transform.parent.GetChild(1).GetComponent<Animator>().SetTrigger("spin");
                            AddTip(i);
                            SetPosition(i);
                            CurrentOrder.RemoveAt(i);
                            CurrentOrderUI.RemoveAt(i);
                            if (StageManager.instance != null) StageManager.instance.success += 1;
                            return true;
                        }
                        else
                        {
                            //StartCoroutine(TurnAlpha(wrong));
                            return false;
                        }
                    }
                }
            }
        }
        //StartCoroutine(TurnAlpha(wrong));
        return false;
    }

    private void AddTip(int i)
    {
        if (CurrentOrderUI[i].GetComponent<OrderUI>().timer.value > CurrentOrderUI[i].GetComponent<OrderUI>().timer.maxValue * 0.6f)
        {
            Tip = 8;
        }
        else if (CurrentOrderUI[i].GetComponent<OrderUI>().timer.value > CurrentOrderUI[i].GetComponent<OrderUI>().timer.maxValue * 0.3f)
        {
            Tip = 5;
        }
        else
        {
            Tip = 3;
        }
        SetCoinText();
        StartCoroutine(StartBigger()); //---> Ŀ���ٰ� �۾����� �ڷ�ƾ 
    }

    private void SetCoinText()
    {
        if (tipCombo < 2) //�� �޺� ������Ʈ
        {
            TextTip.text = "";
        }
        else
        {
            TextTip.text = "�� x " + tipCombo.ToString(); //�� x 3 ������ �۾� �ٲٱ�
            Tip *= tipCombo;
        }
        TipSlider.value = tipCombo;
        if (StageManager.instance != null) StageManager.instance.tipMoney += Tip;
        Coin += Tip;
        TextCoin.text = Coin.ToString(); //�� �󸶵ƴٰ� ������Ʈ

        if (!SoundManager.instance.isSingle) pv.RPC("SetCoinPhoton", RpcTarget.Others, Coin);

        if (Tip != 0)
        {
            GameObject tipText = Instantiate(TextPrefabs, Camera.main.WorldToScreenPoint(FindObjectOfType<Station>().transform.position), Quaternion.identity, Canvas.transform);
            tipText.GetComponent<Text>().text = "+" + Tip + " ��!";
        }
    }

    [PunRPC]
    public void SetCoinPhoton(int Coin)
    {
        if (OriginalMoney != Coin && (int)PhotonNetwork.LocalPlayer.CustomProperties["Color"] == 1) 
        {
            Player1Money = Coin;
            OppositeUI.transform.GetChild(0).GetComponent<Text>().text = Player1Money.ToString();
            PhotonNetwork.PlayerList[0].SetCustomProperties(new Hashtable() { { "OppositeMoney", Coin } });
        }
        else if (OriginalMoney != Coin && (int)PhotonNetwork.LocalPlayer.CustomProperties["Color"] == 0)
        {
            Player2Money = Coin;
            OppositeUI.transform.GetChild(0).GetComponent<Text>().text = Player2Money.ToString();
            PhotonNetwork.PlayerList[1].SetCustomProperties(new Hashtable() { { "OppositeMoney", Coin } });
        }
    }

    IEnumerator StartBigger()
    {
        float progress = 0;
        Color textColor = TextCoin.GetComponent<Text>().color;
        while (CoinOb.transform.localScale.x < 2)
        {
            textColor = Color.Lerp(Color.white, Start, progress);
            progress += Time.deltaTime * 3;
            TextCoin.GetComponent<Text>().color = textColor;
            Vector3 CurrentScale = TextCoin.gameObject.transform.localScale;
            CurrentScale.x += Time.deltaTime * 3;
            CurrentScale.y += Time.deltaTime * 3;
            CurrentScale.z += Time.deltaTime * 3;
            TextCoin.gameObject.transform.localScale = CurrentScale;
            yield return null;
        }
        StartCoroutine(StartSmaller());
    }
    IEnumerator StartSmaller()
    {
        float progress = 0;
        Color textColor = TextCoin.GetComponent<Text>().color;
        while (CoinOb.transform.localScale.x > 1)
        {
            textColor = Color.Lerp(Start, Color.white, progress);
            progress += Time.deltaTime * 3;
            TextCoin.GetComponent<Text>().color = textColor;
            Vector3 CurrentScale = TextCoin.gameObject.transform.localScale;
            CurrentScale.x -= Time.deltaTime * 3;
            CurrentScale.y -= Time.deltaTime * 3;
            CurrentScale.z -= Time.deltaTime * 3;
            TextCoin.gameObject.transform.localScale = CurrentScale;
            yield return null;
        }
    }


    public void MenuFail(GameObject whichUI) //�޴��� �־��� �ð� ���� ���� ������ �� �۵�
    {
        if (StageManager.instance != null)
        {
            StageManager.instance.fail += 1;
        }
        for (int i =0; i < CurrentOrderUI.Count; i++)
        {
            if (CurrentOrderUI[i] == whichUI)
            {
                tipCombo = 0;
                Tip = 0;
                TextTip.text = "";
                TipSlider.value = tipCombo;
                if (StageManager.instance != null) StageManager.instance.failMoney += (int)(CurrentOrder[i].Price * 0.5f);
                Coin -= (int)(CurrentOrder[i].Price * 0.5f);
                TextCoin.text = Coin.ToString(); //�� �󸶵ƴٰ� ������Ʈ
                if (alphaCo == null)
                {
                    alphaCo = StartCoroutine(TurnAlpha(wrong));
                }
                whichUI.transform.position = poolPos;
                whichUI.SetActive(false);
                SetPosition(i);
                CurrentOrderUI.RemoveAt(i);
                CurrentOrder.RemoveAt(i);
                MakeOrder();
            }
        }
        if (flame.activeSelf)
        {
            flame.SetActive(false);
        }
    }

    public void SetPosition(int i)
    {
        float width = CurrentOrderUI[i].GetComponent<BoxCollider2D>().size.x;

        for (int j = 0; j < CurrentOrderUI.Count; j++)
        {
            if (i < j && !CurrentOrderUI[j].GetComponent<OrderUI>().goLeft)
            {
                Vector3 CurrentPosition = CurrentOrderUI[j].transform.position;
                CurrentPosition.x -= width * 0.92f;
                CurrentOrderUI[j].transform.position = CurrentPosition;
            }
        }
    }
}

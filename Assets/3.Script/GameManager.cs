using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Coroutine activeCo = null;

    public bool isStop = true; //일시정지 여부

    //시작하기 전 ready go를 위한 공간
    [SerializeField] private float StartTime = 0;
    [SerializeField] private GameObject Ready;
    [SerializeField] private GameObject Go;
    private bool PlayOnce = false;
    private bool PlayTwice = false;
    private bool StartSetting = false;
    private bool once = false;

    //시간 UI
    [SerializeField] private float GameTime = 160f;
    [SerializeField] private Slider TimeSlider;
    [SerializeField] private Text TextTime;

    //돈 UI
    [SerializeField] private GameObject CoinOb;
    [SerializeField] private Slider TipSlider;
    private int Coin;
    private int Tip;
    [SerializeField] private Text TextCoin;
    [SerializeField] private Text TextTip;
    public int tipCombo;
    bool canCombo;
    [SerializeField] GameObject TextPrefabs;

    //접시 리스폰 관련
    [SerializeField] private GameObject platePrefabs;
    [SerializeField] private float respawnTime = 3f;
    [SerializeField] private GameObject ReturnCounter;

    [SerializeField] private Menu[] Menus; //이번 스테이지에 등장할 메뉴들
    [SerializeField] private int maxMenuLimit; //이번 스테이지에서 최대로 쌓일 수 있는 메뉴 개수들
    [SerializeField] private GameObject[] Single_Double_PoolUIs; //오브젝트 풀링으로 쓸 단일 메뉴 UI들
    public List<Menu> CurrentOrder;
    public List<GameObject> CurrentOrderUI;
    public Vector3 poolPos;
    [SerializeField] private GameObject Canvas;

    public int i = -1;
    private int j = -1;

    float duration = 75; // This will be your time in seconds.
    float smoothness = 0.1f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.
    Color Start = new Color(0, 192 / 255f, 5 / 255f, 255 / 255f); //초록
    Color Middle = new Color(243 / 255f, 239 / 255f, 0, 255 / 255f);
    Color End = new Color(215 / 255f, 11 / 255f, 0, 1f);
    Color currentColor; // This is the state of the color in the current interpolation.

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

    void Awake()
    {
        currentColor = TimeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color;
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
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
        else if (StartSetting && once)
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
        if (GameTime <= 0) //시간 지나면 멈추기
        {
            Time.timeScale = 0;
        }
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
        if (Menus[i].Ingredient.Count == 1)
        {
            for (j = 0; j < Single_Double_PoolUIs.Length; j++)
            {
                if (!Single_Double_PoolUIs[j].activeSelf) //꺼져있는거 찾아서
                {
                    Single_Double_PoolUIs[j].SetActive(true); //켜주고
                    Single_Double_PoolUIs[j].transform.GetChild(1).gameObject.SetActive(false); //두번째 재료 부분은 끄고
                    Single_Double_PoolUIs[j].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Menus[i].IngredientIcon[0]; //첫번째 재료 아이콘 바꾸고
                    Single_Double_PoolUIs[j].transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = Menus[i].MenuIcon; //메뉴 아이콘 바꾸고
                    Single_Double_PoolUIs[j].transform.GetChild(2).GetChild(0).GetComponent<Slider>().maxValue = Menus[i].LimitTime; //슬라이더 시간대로 할당
                    Single_Double_PoolUIs[j].transform.GetChild(2).GetChild(0).GetComponent<Slider>().value = Menus[i].LimitTime; //풀로 시작
                    CurrentOrder.Add(Menus[i]);
                    CurrentOrderUI.Add(Single_Double_PoolUIs[j]);
                    return;
                }
                else //다 켜져있으면 실패
                {
                    continue;
                }
            }
        }
        else if (Menus[i].Ingredient.Count == 2)
        {
            for (j = 0; j < Single_Double_PoolUIs.Length; j++)
            {
                if (!Single_Double_PoolUIs[j].activeSelf) //꺼져있는거 찾아서
                {
                    Single_Double_PoolUIs[j].SetActive(true); //켜주고
                    Single_Double_PoolUIs[j].transform.GetChild(1).gameObject.SetActive(true); //두번째 재료 부분 키고
                    Single_Double_PoolUIs[j].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Menus[i].IngredientIcon[0]; //첫번째 재료 아이콘 바꾸고
                    Single_Double_PoolUIs[j].transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Menus[i].IngredientIcon[1]; //두번째 재료 아이콘 바꾸고
                    Single_Double_PoolUIs[j].transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = Menus[i].MenuIcon; //메뉴 아이콘 바꾸고
                    Single_Double_PoolUIs[j].transform.GetChild(2).GetChild(0).GetComponent<Slider>().maxValue = Menus[i].LimitTime; //슬라이더 시간대로 할당
                    Single_Double_PoolUIs[j].transform.GetChild(2).GetChild(0).GetComponent<Slider>().value = Menus[i].LimitTime; //풀로 시작
                    CurrentOrder.Add(Menus[i]);
                    CurrentOrderUI.Add(Single_Double_PoolUIs[j]);
                    return;
                }
                else //다 켜져있으면 실패
                {
                    continue;
                }
            }
        }
    }

    public bool CheckMenu(List<Handle.HandleType> containIngredients) //plate의 재료 list들 통으로 받아서 비교
    {
        if (containIngredients == null) //빈 접시만 내면 무조건 탈롹
        {
            //빨간색 띵
            return false;
        }
        else
        {
            for (int i = 0; i < CurrentOrder.Count; i++) //현재 쌓인 order에서 비교한다
            {
                if (containIngredients.Count != CurrentOrder[i].Ingredient.Count) //둘이 재료 개수부터 다르면 땡
                {
                    continue;
                }
                else //둘이 재료 개수가 같다면
                {
                    if (containIngredients.Count == 1) //들어간 재료가 하나면
                    {
                        if (containIngredients[0] == CurrentOrder[i].Ingredient[0])
                        {
                            if (i == 0) //순서대로 메뉴를 냈다면 콤보
                            {
                                tipCombo += 1;
                                if (tipCombo > 4)
                                {
                                    tipCombo = 4; //최대 4콤보까지
                                }
                            }
                            else
                            {
                                tipCombo = 0;
                            }
                            CurrentOrderUI[i].transform.position = poolPos;
                            CurrentOrderUI[i].SetActive(false);
                            Coin += CurrentOrder[i].Price;
                            CoinOb.transform.parent.GetChild(1).GetComponent<Animator>().SetTrigger("spin");
                            AddTip(i);
                            SetPosition(i);
                            CurrentOrder.RemoveAt(i);
                            CurrentOrderUI.RemoveAt(i);
                            return true;
                        }
                    }
                    else if (containIngredients.Count == 2) //두개일때 비교
                    {
                        if ((containIngredients[0] == CurrentOrder[i].Ingredient[0] && containIngredients[1] == CurrentOrder[i].Ingredient[1]) || (containIngredients[1] == CurrentOrder[i].Ingredient[0] && containIngredients[0] == CurrentOrder[i].Ingredient[1]))
                        {
                            CurrentOrderUI[i].transform.position = poolPos;
                            CurrentOrderUI[i].SetActive(false);
                            Coin += CurrentOrder[i].Price;
                            CoinOb.transform.parent.GetChild(1).GetComponent<Animator>().SetTrigger("spin");
                            AddTip(i);
                            SetPosition(i);
                            CurrentOrderUI.RemoveAt(i);
                            CurrentOrderUI.RemoveAt(i);
                            return true;
                        }
                    }
                    else if (containIngredients.Count == 3) //세 개일때 할거면 뭐 하던지 말던지
                    {

                    }
                }
            }
        }
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
        StartCoroutine(StartBigger()); //---> 커졌다가 작아지는 코루틴 
    }

    private void SetCoinText()
    {
        if (tipCombo < 2) //팁 콤보 업데이트
        {
            TextTip.text = "";
        }
        else
        {
            TextTip.text = "팁 x " + tipCombo.ToString(); //팁 x 3 같은거 글씨 바꾸기
            Tip *= tipCombo;
        }
        TipSlider.value = tipCombo;
        Coin += Tip;
        TextCoin.text = Coin.ToString(); //돈 얼마됐다고 업데이트
        if (Tip != 0)
        {
            GameObject tipText = Instantiate(TextPrefabs, Camera.main.WorldToScreenPoint(FindObjectOfType<Station>().transform.position), Quaternion.identity, Canvas.transform);
            tipText.GetComponent<Text>().text = "+" + Tip + " 팁!";
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


    public void MenuFail(GameObject whichUI) //메뉴를 주어진 시간 내로 전달 못했을 때 작동
    {
        for (int i =0; i < CurrentOrderUI.Count; i++)
        {
            if (CurrentOrderUI[i] == whichUI)
            {
                //점수나 뭐 깎기 + 빨간 효과
                whichUI.transform.position = poolPos;
                whichUI.SetActive(false);
                SetPosition(i);
                CurrentOrderUI.RemoveAt(i);
                CurrentOrder.RemoveAt(i);
                MakeOrder();
            }
        }
    }

    public void SetPosition(int i)
    {
        float width = CurrentOrderUI[i].GetComponent<BoxCollider2D>().size.x;

        for (int j = 0; j < CurrentOrderUI.Count; j++)
        {
            if (i < j && !CurrentOrderUI[j].GetComponent<OrderUI>().goLeft)
            {
                Debug.Log(CurrentOrderUI[j].name +"가 "+ width + "만큼 이동");
                Vector3 CurrentPosition = CurrentOrderUI[j].transform.position;
                CurrentPosition.x -= width;
                CurrentOrderUI[j].transform.position = CurrentPosition;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private Coroutine activeCo = null;

    public bool isStop = true; //�Ͻ����� ����

    //�����ϱ� �� ready go�� ���� ����
    [SerializeField] private float StartTime = 0;
    [SerializeField] private GameObject Ready;
    [SerializeField] private GameObject Go;
    private bool PlayOnce = false;
    private bool PlayTwice = false;
    private bool StartSetting = false;
    private bool once = false;

    //�ð� UI
    [SerializeField] private float GameTime = 160f;
    [SerializeField] private Slider TimeSlider;
    [SerializeField] private Text TextTime;
    
    //�� UI
    [SerializeField] private Slider TipSlider;
    private int Coin;
    [SerializeField] private Text TextCoin;
    [SerializeField] private Text TextTip;
    int tipCombo;
    bool canCombo;

    //���� ������ ����
    [SerializeField] private GameObject platePrefabs;
    [SerializeField] private float respawnTime = 3f;
    [SerializeField] private GameObject ReturnCounter;

    [SerializeField] private Menu[] Menus; //�̹� ���������� ������ �޴���
    [SerializeField] private int maxMenuLimit; //�̹� ������������ �ִ�� ���� �� �ִ� �޴� ������
    [SerializeField] private GameObject[] Single_Double_PoolUIs; //������Ʈ Ǯ������ �� ���� �޴� UI��
    public List<Menu> CurrentOrder;
    public List<GameObject> CurrentOrderUI;
    public Vector3 poolPos;
    [SerializeField] private GameObject Canvas;

    private int i = -1;
    private int j = -1;

    float duration = 75; // This will be your time in seconds.
    float smoothness = 0.1f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.
    Color Start = new Color(0, 192 / 255f, 5 / 255f, 255 / 255f);
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
            //Invoke("MakeOrder", 30f);
            //Invoke("MakeOrder", 80f);
            //Invoke("MakeOrder", 150f);
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
        else if (Menus[i].Ingredient.Count == 2)
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
    }

    public bool CheckMenu(List<Handle.HandleType> containIngredients) //plate�� ��� list�� ������ �޾Ƽ� ��
    {
        if (containIngredients == null) //�� ���ø� ���� ������ Ż��
        {
            return false;
        }
        else
        {
            for (int i = 0; i < CurrentOrder.Count; i++) //���� ���� order���� ���Ѵ�
            {
                if (containIngredients.Count != CurrentOrder[i].Ingredient.Count) //���� ��� �������� �ٸ��� ��
                {
                    continue;
                }
                else //���� ��� ������ ���ٸ�
                {
                    if (containIngredients.Count == 1) //�� ��ᰡ �ϳ���
                    {
                        if (containIngredients[0] == CurrentOrder[i].Ingredient[0])
                        {
                            CurrentOrderUI[i].transform.position = poolPos;
                            CurrentOrderUI[i].SetActive(false);
                            AddCoin(i);
                            CurrentOrder.RemoveAt(i);
                            CurrentOrderUI.RemoveAt(i);
                            StartCoroutine(TimingControl(i));
                            return true;
                        }
                    }
                    else if (containIngredients.Count == 2) //�ΰ��϶� ��
                    {
                        if ((containIngredients[0] == CurrentOrder[i].Ingredient[0] && containIngredients[1] == CurrentOrder[i].Ingredient[1]) || (containIngredients[1] == CurrentOrder[i].Ingredient[0] && containIngredients[0] == CurrentOrder[i].Ingredient[1]))
                        {
                            CurrentOrderUI[i].transform.position = poolPos;
                            CurrentOrderUI[i].SetActive(false);
                            
                            CurrentOrderUI.RemoveAt(i);
                            CurrentOrderUI.RemoveAt(i);
                            return true;
                        }
                    }
                    else if (containIngredients.Count == 3) //�� ���϶� �ҰŸ� �� �ϴ��� ������
                    {

                    }
                }
            }
        }
        return false;
    }

    private void AddCoin(int i)
    {
        //if tip �����Ҷ��̸� ���ñ�
        if (CurrentOrderUI[i].GetComponent<OrderUI>().timer.value > CurrentOrderUI[i].GetComponent<OrderUI>().timer.maxValue * 0.6f)
        {
            Coin += 28;
        }
        else if (CurrentOrderUI[i].GetComponent<OrderUI>().timer.value > CurrentOrderUI[i].GetComponent<OrderUI>().timer.maxValue * 0.3f)
        {
            Coin += 20;
        }
        else
        {
            Coin += 15;
        }
        SetCoinText();
        //StartCoroutine();---> Ŀ���ٰ� �۾����� �ڷ�ƾ ����� + ���� �Ͼ� -> �ʷ� -> �Ͼ�
    }

    private void SetCoinText()
    {
        TextCoin.text = Coin.ToString();
    }

    IEnumerator TimingControl(int i)
    {
        Debug.Log(i+"��°�� ������, �� ������ �ֵ��� �� ���ĵǾ����");
        for (j = 0; j < CurrentOrderUI.Count; j++)
        {
            if (j >= i && CurrentOrderUI[j].activeSelf)
            {
                yield return new WaitForSeconds(0.2f);
                Debug.Log(j + "����");
                CurrentOrderUI[j].GetComponent<OrderUI>().goLeft = true;
                //j�ֵ鸸 �������� �����̱�
            }
        }
        activeCo = null;
    }


    public void MenuFail(GameObject whichUI) //�޴��� �־��� �ð� ���� ���� ������ �� �۵�
    {
        for (int i =0; i < CurrentOrderUI.Count; i++)
        {
            if (CurrentOrderUI[i] == whichUI)
            {
                //������ �� ��� + ���� ȿ��
                whichUI.transform.position = poolPos;
                whichUI.SetActive(false);
                CurrentOrderUI.RemoveAt(i);
                CurrentOrder.RemoveAt(i);
                MakeOrder();
                if (activeCo == null)
                {
                    activeCo = StartCoroutine(TimingControl(i));
                }
            }
        }
    }
}

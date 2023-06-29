using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MapManager : MonoBehaviour
{
    private bool isStage1open = false;
    private bool isStage2open = false;
    private bool isStage3open = false;
    private bool isStage4open = false;

    public GameObject bus;
    [SerializeField] private Vector3[] positions;
    [SerializeField] private Vector3[] targetPositions;
    [SerializeField] private CinemachineVirtualCamera Cam;
    [SerializeField] private GameObject focus;
    public bool canActive = false;

    [SerializeField] private GameObject[] flags;
    [SerializeField] private Material[] flags_m;

    [Header("첫번째 flip")]
    [SerializeField] private GameObject[] first;
    [SerializeField] private GameObject[] second;
    [SerializeField] private GameObject[] third;
    [SerializeField] private GameObject[] fourth;
    [SerializeField] private GameObject[] fifth;
    [SerializeField] private GameObject[] sixth;

    [Header("두번째 flip")]
    [SerializeField] private GameObject[] first2;
    [SerializeField] private GameObject[] second2;
    [SerializeField] private GameObject[] third2;
    [SerializeField] private GameObject[] fourth2;
    [SerializeField] private GameObject[] fifth2;

    [Header("세번째 flip")]
    [SerializeField] private GameObject[] first3;
    [SerializeField] private GameObject[] second3;
    [SerializeField] private GameObject[] third3;
    [SerializeField] private GameObject[] fourth3;
    [SerializeField] private GameObject[] fifth3;
    [SerializeField] private GameObject[] sixth3;

    private void Awake()
    {
        bus.SetActive(false);
        StageManager.instance.SetMap(flags, flags_m);
        Time.timeScale = 1;
        if (StageManager.instance.playStage == StageManager.State.start && !isStage1open)
        {
            bus.transform.position = positions[0];
            Cam.Follow = focus.transform;
            Cam.LookAt = focus.transform;
            StartCoroutine(FlipFirst());
        }
        else if (StageManager.instance.playStage == StageManager.State.stage1)
        {
            bus.transform.position = positions[1];
            
            if (!isStage2open && StageManager.instance.isClearMap1)
            {
                focus.transform.position = targetPositions[0];
                Cam.Follow = focus.transform;
                Cam.LookAt = focus.transform;
                StartCoroutine(FlipSecondMap());
            }
            else
            {
                Cam.Follow = focus.transform;
                Cam.LookAt = focus.transform;
                bus.SetActive(true);
            }
        }
        else if (StageManager.instance.playStage == StageManager.State.stage2)
        {
            bus.transform.position = positions[2];
            if (!isStage3open && StageManager.instance.isClearMap2)
            {
                focus.transform.position = targetPositions[1];
                Cam.Follow = focus.transform;
                Cam.LookAt = focus.transform;
                StartCoroutine(FlipThirdMap());
            }
        }
        else if (StageManager.instance.playStage == StageManager.State.stage3)
        {

        }
    }

    IEnumerator FlipFirst()
    {
        for (int i = 0; i < first.Length; i++)
        {
            first[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < second.Length; i++)
        {
            second[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < third.Length; i++)
        {
            third[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < fourth.Length; i++)
        {
            fourth[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < fifth.Length; i++)
        {
            fifth[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        isStage1open = true;
        Invoke("BusOn", 2f);
    }

    IEnumerator FlipSecondMap()
    {
        for (int i = 0; i < first2.Length; i++)
        {
            first2[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < second2.Length; i++)
        {
            second2[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < third2.Length; i++)
        {
            third2[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < fourth2.Length; i++)
        {
            fourth2[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < fifth2.Length; i++)
        {
            fifth2[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        isStage2open = true;
        Invoke("BusOn", 2f);
    }

    IEnumerator FlipThirdMap()
    {
        for (int i = 0; i < first3.Length; i++)
        {
            first3[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < second3.Length; i++)
        {
            second3[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < third3.Length; i++)
        {
            third3[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < fourth3.Length; i++)
        {
            fourth3[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < fifth3.Length; i++)
        {
            fifth3[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < sixth3.Length; i++)
        {
            sixth3[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        isStage3open = true;
        Invoke("BusOn", 2f);
    }
    private void BusOn()
    {
        Cam.Follow = bus.transform;
        Cam.LookAt = bus.transform;
        bus.SetActive(true);
    }
}

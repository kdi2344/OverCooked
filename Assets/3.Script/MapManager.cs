using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MapManager : MonoBehaviour
{
    public GameObject bus;
    [SerializeField] private Vector3[] positions;
    [SerializeField] private Vector3[] targetPositions;
    [SerializeField] private CinemachineVirtualCamera Cam;
    [SerializeField] private GameObject focus;
    public bool canActive = false;

    [SerializeField] private GameObject[] first;
    [SerializeField] private GameObject[] second;
    [SerializeField] private GameObject[] third;
    [SerializeField] private GameObject[] fourth;
    [SerializeField] private GameObject[] fifth;
    [SerializeField] private GameObject[] sixth;

    [Header("µÎ¹øÂ° flip")]
    [SerializeField] private GameObject[] first2;
    [SerializeField] private GameObject[] second2;
    [SerializeField] private GameObject[] third2;
    [SerializeField] private GameObject[] fourth2;
    [SerializeField] private GameObject[] fifth2;

    private void Awake()
    {
        bus.SetActive(false);
        StageManager.instance.SetMap();
        Time.timeScale = 1;
        if (!StageManager.instance.isClearMap1)    
        {
            Cam.Follow = focus.transform;
            Cam.LookAt = focus.transform;
            StartCoroutine(FlipFirst());
        }
        else if (!StageManager.instance.isClearMap2)
        {
            focus.transform.position = targetPositions[0];
            bus.transform.position = positions[0];
            Cam.Follow = focus.transform;
            Cam.LookAt = focus.transform;
            StartCoroutine(FlipSecondMap());
        }
        else if (!StageManager.instance.isClearMap3)
        {

        }
        else if (!StageManager.instance.isAllClear)
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
        Cam.Follow = bus.transform;
        Cam.LookAt = bus.transform;
        Invoke("BusOn", 1f);
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
        
        Invoke("BusOn", 1f);
    }
    private void BusOn()
    {
        Cam.Follow = bus.transform;
        Cam.LookAt = bus.transform;
        bus.SetActive(true);
    }
}

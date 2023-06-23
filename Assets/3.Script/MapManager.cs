using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject bus;
    public bool canActive = false;

    [SerializeField] private GameObject[] first;
    [SerializeField] private GameObject[] second;
    [SerializeField] private GameObject[] third;
    [SerializeField] private GameObject[] fourth;
    [SerializeField] private GameObject[] fifth;
    [SerializeField] private GameObject[] sixth;

    private void Awake()
    {
        StartCoroutine(FlipFirst());
        bus.SetActive(false);
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
        for (int i = 0; i < sixth.Length; i++)
        {
            sixth[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("flip");
        }
        yield return new WaitForSeconds(0.3f);
        Invoke("BusOn", 1f);
    }
    private void BusOn()
    {
        bus.SetActive(true);
    }
}

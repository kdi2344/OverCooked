using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class CuttingBoard : MonoBehaviour
{
    private Object parentObject;
    [SerializeField] private Slider cookingBar;
    [SerializeField] Vector3 pos;
    public Coroutine _CoTimer;
    public bool Pause = false;
    public float CuttingTime;
    private void Start()
    {
        parentObject = transform.parent.GetComponent<Object>();
    }
    private void Update()
    {
        cookingBar.transform.position = Camera.main.WorldToScreenPoint(parentObject.transform.position + pos);
        cookingBar.value = CuttingTime;
        if (parentObject.onSomething)
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false); //Ä® ²ô±â
        }
        else
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true); //Ä® ÄÑ±â
        }
    }
    public void StartCutting(UnityAction EndCallBack = null)
    {
        if (parentObject.onSomething)
        {
            ClearTime();
            _CoTimer = StartCoroutine(CoStartCutting(EndCallBack));
            FindObjectOfType<PlayerController>().anim.SetTrigger("startCut");
            cookingBar.gameObject.SetActive(true);
        }
    }
    public void PauseSlider(bool pause)
    {
        Pause = pause;
    }

    private IEnumerator CoStartCutting(UnityAction EndCallBack = null)
    {
        while (CuttingTime <= 1)
        {
            while (Pause)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            CuttingTime += 0.25f;
        }
        EndCallBack?.Invoke();
        OffSlider();
    }

    private void ClearTime()
    {
        if (_CoTimer != null)
        {
            StopCoroutine(_CoTimer);
            _CoTimer = null;
        }
        Pause = false;
    }

    public void OffSlider()
    {
        cookingBar.gameObject.SetActive(false);
        cookingBar.value = 0f;
        FindObjectOfType<PlayerController>().anim.SetBool("canCut", false);
        FindObjectOfType<Handle>().isCooked = true;
    }
}

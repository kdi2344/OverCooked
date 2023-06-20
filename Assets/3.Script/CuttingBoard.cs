using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class CuttingBoard : MonoBehaviour
{
    private Object parentObject;
    public Slider cookingBar;
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
            cookingBar.gameObject.SetActive(true);
            ClearTime();
            _CoTimer = StartCoroutine(CoStartCutting(EndCallBack));
            FindObjectOfType<PlayerController>().anim.SetTrigger("startCut");
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
        _CoTimer = null;
        Pause = false;
        CuttingTime = 0;
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
        cookingBar.value = 0f;
        cookingBar.gameObject.SetActive(false);
        FindObjectOfType<PlayerController>().anim.SetBool("canCut", false);
        Handle Ingredient = transform.parent.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Handle>();
        Ingredient.isCooked = true;
        Ingredient.changeMesh();
    }
}

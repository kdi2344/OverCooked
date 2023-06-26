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

    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject IngredientUI;
    [SerializeField] private Sprite[] Icons;
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
            FindObjectOfType<PlayerController>().anim.SetBool("canCut", true);
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
        Ingredient.changeMesh(Ingredient.hand);
        InstantiateUI();
    }
    public void InstantiateUI()
    {
        Handle Ingredient = transform.parent.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Handle>();
        if (Ingredient.hand == Handle.HandleType.Fish)
        {
            GameObject madeUI = Instantiate(IngredientUI, Vector3.zero, Quaternion.identity, Canvas.transform);
            madeUI.transform.GetChild(0).gameObject.SetActive(true);
            Image image = madeUI.transform.GetChild(0).GetComponent<Image>();
            image.sprite = Icons[0];
            madeUI.GetComponent<IngredientUI>().Target = Ingredient.transform;
        }
        else if (Ingredient.hand == Handle.HandleType.Shrimp)
        {
            GameObject madeUI = Instantiate(IngredientUI, Vector3.zero, Quaternion.identity, Canvas.transform);
            madeUI.transform.GetChild(0).gameObject.SetActive(true);
            Image image = madeUI.transform.GetChild(0).GetComponent<Image>();
            image.sprite = Icons[1];
            madeUI.GetComponent<IngredientUI>().Target = Ingredient.transform;
        }
        else if (Ingredient.hand == Handle.HandleType.Tomato)
        {
            GameObject madeUI = Instantiate(IngredientUI, Vector3.zero, Quaternion.identity, Canvas.transform);
            madeUI.transform.GetChild(0).gameObject.SetActive(true);
            Image image = madeUI.transform.GetChild(0).GetComponent<Image>();
            image.sprite = Icons[2];
            madeUI.GetComponent<IngredientUI>().Target = Ingredient.transform;
        }
        else if (Ingredient.hand == Handle.HandleType.Lettuce)
        {
            GameObject madeUI = Instantiate(IngredientUI, Vector3.zero, Quaternion.identity, Canvas.transform);
            madeUI.transform.GetChild(0).gameObject.SetActive(true);
            Image image = madeUI.transform.GetChild(0).GetComponent<Image>();
            image.sprite = Icons[3];
            madeUI.GetComponent<IngredientUI>().Target = Ingredient.transform;
        }
        else if (Ingredient.hand == Handle.HandleType.Cucumber)
        {
            GameObject madeUI = Instantiate(IngredientUI, Vector3.zero, Quaternion.identity, Canvas.transform);
            madeUI.transform.GetChild(0).gameObject.SetActive(true);
            Image image = madeUI.transform.GetChild(0).GetComponent<Image>();
            image.sprite = Icons[4];
            madeUI.GetComponent<IngredientUI>().Target = Ingredient.transform;
        }
        else if (Ingredient.hand == Handle.HandleType.Potato)
        {
            GameObject madeUI = Instantiate(IngredientUI, Vector3.zero, Quaternion.identity, Canvas.transform);
            madeUI.transform.GetChild(0).gameObject.SetActive(true);
            Image image = madeUI.transform.GetChild(0).GetComponent<Image>();
            image.sprite = Icons[5];
            madeUI.GetComponent<IngredientUI>().Target = Ingredient.transform;
        }
        else if (Ingredient.hand == Handle.HandleType.Chicken)
        {
            GameObject madeUI = Instantiate(IngredientUI, Vector3.zero, Quaternion.identity, Canvas.transform);
            madeUI.transform.GetChild(0).gameObject.SetActive(true);
            Image image = madeUI.transform.GetChild(0).GetComponent<Image>();
            image.sprite = Icons[6];
            madeUI.GetComponent<IngredientUI>().Target = Ingredient.transform;
        }
    }
}

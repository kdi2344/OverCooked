using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plates : MonoBehaviour
{
    public List<Handle.HandleType> containIngredients = new List<Handle.HandleType>();
    public int limit = 1;
    public GameObject Canvas;
    [SerializeField] private GameObject IngredientUI;
    [SerializeField] private Sprite[] Icons;
    GameObject madeUI;

    private void Update()
    {
        //if (containIngredients)
    }

    public bool AddIngredient(Handle.HandleType handleType)
    {
        if (!CheckOverlap(handleType) && containIngredients.Count < limit)
        {
            containIngredients.Add(handleType);
            if (handleType == Handle.HandleType.Fish)
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (handleType == Handle.HandleType.Shrimp)
            {
                transform.GetChild(2).gameObject.SetActive(true);
            }
            return true;
        }
        return false;
    }

    public void ClearIngredient()
    {
        if (containIngredients.Count == 1)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
        }
        containIngredients.RemoveRange(0, containIngredients.Count); //다 지우고
        Destroy(madeUI); //UI도 지우기
    }
    private bool CheckOverlap(Handle.HandleType type)
    {
        for (int i =0; i < containIngredients.Count; i++)
        {
            if (containIngredients[i].Equals(type))
            {
                return true;
            }
        }
        return false;
    }

    public void InstantiateUI()
    {
        if (containIngredients.Count == 1)
        {
            madeUI = Instantiate(IngredientUI, Vector3.zero, Quaternion.identity, Canvas.transform);
            Image image = madeUI.GetComponent<Image>();
            if (containIngredients[0].Equals(Handle.HandleType.Fish))
            {
                image.sprite = Icons[0];
            }
            else if (containIngredients[0].Equals(Handle.HandleType.Fish))
            {
                image.sprite = Icons[1];
            }
            madeUI.GetComponent<IngredientUI>().Target = transform;
        }
    }
}

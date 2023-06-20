using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    [SerializeField] private Animator CraftAnim;
    public enum FoodType { Fish, Shrimp }
    public FoodType food;
    public GameObject foodPrefabs;
    public void OpenCraft()
    {
        CraftAnim.SetTrigger("Open");
        if (food == FoodType.Fish)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<PlayerController>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<PlayerController>().transform);
        }
        else if (food == FoodType.Shrimp)
        {
            GameObject newShrimp = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<PlayerController>().isHolding = true;
            newShrimp.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<PlayerController>().transform);
        }
    }
}

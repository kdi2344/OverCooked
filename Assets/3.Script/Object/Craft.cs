using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{
    [SerializeField] private Animator CraftAnim;
    public enum FoodType { Fish, Shrimp, Lettuce, Cucumber, Tomato, Chicken, Potato }
    public FoodType food;
    public GameObject foodPrefabs;

    public void OpenCraftPlayer1()
    {
        CraftAnim.SetTrigger("Open");
        if (food == FoodType.Fish)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<PlayerController>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<PlayerController>().transform, Handle.HandleType.Fish);
        }
        else if (food == FoodType.Shrimp)
        {
            GameObject newShrimp = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<PlayerController>().isHolding = true;
            newShrimp.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<PlayerController>().transform, Handle.HandleType.Shrimp);
        }
        else if (food == FoodType.Lettuce)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<PlayerController>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<PlayerController>().transform, Handle.HandleType.Lettuce);
        }
        else if (food == FoodType.Cucumber)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<PlayerController>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<PlayerController>().transform, Handle.HandleType.Cucumber);
        }
        else if (food == FoodType.Tomato)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<PlayerController>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<PlayerController>().transform, Handle.HandleType.Tomato);
        }
        else if (food == FoodType.Chicken)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<PlayerController>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<PlayerController>().transform, Handle.HandleType.Chicken);
        }
        else if (food == FoodType.Potato)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<PlayerController>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<PlayerController>().transform, Handle.HandleType.Potato    );
        }
    }
    public void OpenCraftPlayer2()
    {
        CraftAnim.SetTrigger("Open");
        if (food == FoodType.Fish)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<Player2Controller>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<Player2Controller>().transform, Handle.HandleType.Fish);
        }
        else if (food == FoodType.Shrimp)
        {
            GameObject newShrimp = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<Player2Controller>().isHolding = true;
            newShrimp.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<Player2Controller>().transform, Handle.HandleType.Shrimp);
        }
        else if (food == FoodType.Lettuce)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<Player2Controller>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<Player2Controller>().transform, Handle.HandleType.Lettuce);
        }
        else if (food == FoodType.Cucumber)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<Player2Controller>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<Player2Controller>().transform, Handle.HandleType.Cucumber);
        }
        else if (food == FoodType.Tomato)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<Player2Controller>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<Player2Controller>().transform, Handle.HandleType.Tomato);
        }
        else if (food == FoodType.Chicken)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<Player2Controller>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<Player2Controller>().transform, Handle.HandleType.Chicken);
        }
        else if (food == FoodType.Potato)
        {
            GameObject newFish = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
            FindObjectOfType<Player2Controller>().isHolding = true;
            newFish.transform.GetChild(0).transform.GetChild(0).GetComponent<Handle>().IngredientHandle(FindObjectOfType<Player2Controller>().transform, Handle.HandleType.Potato);
        }
    }
}

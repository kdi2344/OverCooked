using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    [SerializeField] private Animator CraftAnim;
    public enum FoodType { Tomato, Shrimp }
    public FoodType food;
    public GameObject foodPrefabs;
    public void OpenCraft()
    {
        CraftAnim.SetTrigger("Open");

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Menu Data", menuName = "Scriptable Object/Menu data", order = int.MaxValue)]

public class Menu : ScriptableObject
{
    public int Level;
    public string MenuName;
    public List<Handle.HandleType> Ingredient; 
    public float LimitTime;
    public Sprite MenuIcon;
    public List <Sprite> IngredientIcon;
    public int Price;
}

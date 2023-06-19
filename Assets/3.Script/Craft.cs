using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    [SerializeField] private Animator CraftAnim;
    public void OpenCraft()
    {
        CraftAnim.SetTrigger("Open");
    }
}

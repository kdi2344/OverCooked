using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCart : MonoBehaviour
{
    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    private void Start() 
    {
        StartCoroutine(Move_co());
    }

    IEnumerator Move_co()
    {
        while(GameManager.instance.GameTime > 0)
        {
            yield return new WaitForSeconds(20f);
            anim.SetTrigger("move");
        }
    }
}

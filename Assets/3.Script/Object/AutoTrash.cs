using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTrash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient") && !FindObjectOfType<PlayerController>().isHolding)
        {
            //없어지는 scale 조정느낌
            Destroy(other.transform.parent.parent.gameObject);
        }
    }
}

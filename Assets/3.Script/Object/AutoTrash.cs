using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTrash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient") && !FindObjectOfType<PlayerController>().isHolding)
        {
            //�������� scale ��������
            Destroy(other.transform.parent.parent.gameObject);
        }
    }
}

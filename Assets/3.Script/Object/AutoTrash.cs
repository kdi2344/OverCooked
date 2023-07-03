using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTrash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient") && (!other.transform.parent.IsChildOf(FindObjectOfType<PlayerController>().transform) && !other.transform.parent.IsChildOf(FindObjectOfType<Player2Controller>().transform)))
        {
            //없어지는 scale 조정느낌
            //StartCoroutine(ScaleSmaller(other));
            SoundManager.instance.PlayEffect("bin");
            Destroy(other.transform.parent.parent.gameObject);
        }
    }
    IEnumerator ScaleSmaller(Collider other)
    {
        if (other == null)
        {
            yield return null;
        }
        else if (other.transform.parent.gameObject.GetComponent<Rigidbody>() != null)
        {
            other.transform.parent.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.transform.parent.GetComponent<MeshCollider>().isTrigger = true;
            Vector3 pos = transform.position;
            if (other.GetComponent<Handle>().hand == Handle.HandleType.Fish)
            {
                other.transform.parent.localPosition = other.GetComponent<Handle>().fishLocalPos;
            }
            else if (other.GetComponent<Handle>().hand == Handle.HandleType.Shrimp)
            {
                other.transform.parent.localPosition = other.GetComponent<Handle>().shrimpLocalPos;
            }
            other.transform.parent.parent.position = pos;
            while (other.transform.parent.parent.localScale.x > 0)
            {
                Vector3 scale = other.transform.parent.parent.localScale;
                scale.x -= 0.05f;
                scale.y -= 0.05f;
                scale.z -= 0.05f;
                other.transform.parent.parent.localScale = scale;
                yield return new WaitForSeconds(0.01f);
            }
            Destroy(other.transform.parent.parent.gameObject);
        }
    }
}

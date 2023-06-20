using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    public bool isOnDesk = true;
    public bool isCooked = false;

    public void IngredientHandle(Transform something)
    {
        transform.parent.transform.parent.transform.SetParent(something);
        transform.parent.transform.parent.transform.localPosition = new Vector3(-0.409999996f, 0, 1.84000003f);
    }
    public void IngredientHandleOff(Transform parent, Vector3 target)
    {
        transform.parent.transform.parent.transform.SetParent(parent);
        transform.parent.transform.parent.transform.localPosition = target;
    }
    public void PlayerHandle(Transform something)
    {
        transform.SetParent(something);
        transform.localPosition = new Vector3(-0.4f, 0.24f, 2.23f);
    }
    public void PlayerHandleOff(Transform parent, Vector3 target)
    {
        transform.SetParent(parent);
        transform.localPosition = target;
    }
}

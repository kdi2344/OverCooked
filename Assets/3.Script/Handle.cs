using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
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

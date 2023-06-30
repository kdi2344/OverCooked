using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sky : MonoBehaviour
{
    float y = 0.1f;
    float yOffset;

    void Update()
    {
        yOffset -= (Time.deltaTime * y) / 10f;
        gameObject.GetComponent<Renderer>().materials[0].mainTextureOffset = new Vector2(0, yOffset);
        gameObject.GetComponent<MeshRenderer>().materials[0].mainTextureOffset = new Vector2(0, yOffset);
        gameObject.GetComponent<MeshRenderer>().materials[0].SetTextureOffset("_MainTex", new Vector2(0, yOffset));
    }
}

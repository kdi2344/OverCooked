using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    float x = 5.5f;
    float xOffset;

    float y = 5.5f;
    float yOffset;

    void Update()
    {
        xOffset -= (Time.deltaTime * x) / 10f;
        gameObject.GetComponent<Renderer>().materials[3].mainTextureOffset = new Vector2(xOffset, 0);
        gameObject.GetComponent<MeshRenderer>().materials[3].mainTextureOffset = new Vector2(xOffset, 0);
        gameObject.GetComponent<MeshRenderer>().materials[3].SetTextureOffset("_MainTex", new Vector2(xOffset, 0));

        yOffset -= (Time.deltaTime * y) / 10f;
        gameObject.GetComponent<Renderer>().materials[3].mainTextureOffset = new Vector2(0, yOffset);
        gameObject.GetComponent<MeshRenderer>().materials[3].mainTextureOffset = new Vector2(0, yOffset);
        gameObject.GetComponent<MeshRenderer>().materials[3].SetTextureOffset("_MainTex", new Vector2(0, yOffset));

    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrolling : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    Vector3 originPos = new Vector3(0, 1090, 0);
    private void Awake()
    {
        
    }
    private void Update()
    {
        float pos = transform.localPosition.y;
        pos -= speed * Time.deltaTime;
        transform.localPosition = new Vector3(transform.localPosition.x, pos, transform.localPosition.z);
        if (transform.localPosition.y < -1090)
        {
            transform.localPosition = originPos;
        }
    }
}

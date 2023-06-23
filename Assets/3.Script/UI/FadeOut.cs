using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Vector3 target;
    private Text GetText;
    private bool isCo = false;

    private void Awake()
    {
        TryGetComponent(out GetText);
    }
    private void Start()
    {
        target = Camera.main.WorldToScreenPoint(FindObjectOfType<Station>().transform.position) + new Vector3(0, 100, 0);
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target, 1.5f * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 10 && !isCo)
        {
            StartCoroutine(ColorChange_co());
        }
    }

    IEnumerator ColorChange_co()
    {
        isCo = true;

        Color c = GetText.color;
        c.a -= 0.05f;
        GetText.color = c;

        if (c.a < 0.01f)
        {
            Destroy(gameObject);
        }

        yield return new WaitForSeconds(0.005f);

        isCo = false;
    }
}

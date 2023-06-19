using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public bool canActive = false;
    [SerializeField] private Material[] materials;
    [HideInInspector] public enum ObjectType { CounterTop, Craft, Return, Station, Sink };
    public ObjectType type;

    public void OnHighlight()
    {
        if (type == ObjectType.CounterTop)
        {
                canActive = true;
                Renderer rd = transform.parent.GetComponent<MeshRenderer>();
                rd.material = materials[1];
        }
        else if (type == ObjectType.Craft)
        {
                canActive = true;
                Renderer rd = transform.parent.transform.parent.transform.parent.GetChild(0).GetComponent<MeshRenderer>();
                rd.material = materials[1];
                rd = transform.parent.transform.parent.transform.parent.GetChild(1).GetComponent<MeshRenderer>();
                rd.material = materials[1];
        }
    }

    public void OffHighlight()
    {
        if (type == ObjectType.CounterTop)
        {
                canActive = false;
                Renderer rd = transform.parent.GetComponent<MeshRenderer>();
                rd.material = materials[0];
        }
        else if (type == ObjectType.Craft)
        {
            Renderer rd = transform.parent.transform.parent.transform.parent.GetChild(0).GetComponent<MeshRenderer>();
            rd.material = materials[0];
            rd = transform.parent.transform.parent.transform.parent.GetChild(1).GetComponent<MeshRenderer>();
            rd.material = materials[0];
        }
    }
}

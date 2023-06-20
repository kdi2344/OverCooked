using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public bool onSomething = false;
    public bool canActive = false;
    [SerializeField] private Material[] materials;
    [HideInInspector] public enum ObjectType { CounterTop, Craft, Return, Station, Sink, Ingredient, Board };
    public ObjectType type;

    public void OnHighlight(bool onSomething)
    {
        if (type == ObjectType.CounterTop || type == ObjectType.Board)
        {
            canActive = true;
            Renderer rd = transform.parent.GetComponent<MeshRenderer>();
            rd.material = materials[1];
            if (onSomething)
            {
                //rd = transform.parent.transform.parent.GetChild(2).transform.GetChild(0).GetComponent<MeshRenderer>();
                //������ �ִ��� ���찡 �ִ��� �ٸ��� material ����
                //rd.material = materials[1];
            }
        }
        else if (type == ObjectType.Craft)
        {
            canActive = true;
            Renderer rd = transform.parent.GetComponent<MeshRenderer>();
            rd.material = materials[1];
            rd = transform.parent.transform.parent.GetChild(1).GetComponent<MeshRenderer>();
            rd.material = materials[1];
            if (onSomething)
            {
                //rd = transform.parent.transform.parent.GetChild(2).GetComponent<MeshRenderer>();
                //rd.material = materials[1];
            }
        }
        else if (type == ObjectType.Ingredient)
        {
            canActive = true;
            Renderer rd = transform.parent.GetComponent<MeshRenderer>();
            rd.material = materials[1];
        }
    }

    public void OffHighlight()
    {
        if (type == ObjectType.CounterTop || type == ObjectType.Board)
        {
            canActive = false;
            Renderer rd = transform.parent.GetComponent<MeshRenderer>();
            rd.material = materials[0];
        }
        else if (type == ObjectType.Craft)
        {
            canActive = false;
            Renderer rd = transform.parent.GetComponent<MeshRenderer>();
            rd.material = materials[0];
            rd = transform.parent.transform.parent.GetChild(1).GetComponent<MeshRenderer>();
            rd.material = materials[0];
        }
        else if (type == ObjectType.Ingredient)
        {
            canActive = false;
            Renderer rd = transform.parent.GetComponent<MeshRenderer>();
            rd.material = materials[0];
        }
    }

    //public Vector3 PutSomething()
    //{
    //    if (!onSomething)
    //    {
    //        onSomething = true;
    //        if (type == ObjectType.CounterTop)
    //        {
    //            Vector3 position = transform.parent.GetChild(1).position; //�����ص� Pos ��ġ�� ����
    //            return position;
    //        }
    //        else if (type == ObjectType.Craft)
    //        {
    //            Vector3 position = Vector3.zero;
    //            return position;
    //        }
    //        else //if (type == ObjectType.Ingredient)
    //        {
    //            Vector3 position = Vector3.zero;
    //            return position;
    //        }
    //    }
    //    else
    //    {
    //        return Vector3.zero;
    //    }
    //}
}

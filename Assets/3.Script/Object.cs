using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public bool onSomething = false;
    public bool canActive = false;
    [SerializeField] private Material[] materials;
    [HideInInspector] public enum ObjectType { CounterTop, Craft, Return, Station, Sink, Ingredient, Board, Bin, Plate};
    public ObjectType type;

    public void OnHighlight(bool isCooked)
    {
        if (type == ObjectType.CounterTop || type == ObjectType.Board || type == ObjectType.Bin || type == ObjectType.Plate)
        {
            canActive = true;
            Renderer rd = transform.parent.GetComponent<MeshRenderer>();
            rd.material = materials[1];
        }
        else if (type == ObjectType.Craft)
        {
            canActive = true;
            Renderer rd = transform.parent.GetComponent<MeshRenderer>();
            rd.material = materials[1];
            rd = transform.parent.transform.parent.GetChild(1).GetComponent<MeshRenderer>();
            rd.material = materials[1];
        }
        else if (type == ObjectType.Ingredient)
        {
            canActive = true;
            Renderer rd = transform.parent.GetComponent<MeshRenderer>();
            if (!isCooked)
            {
                rd.material = materials[1];
            }
            else
            {
                rd.material = materials[3];
            }
        }
        else if (type == ObjectType.Station)
        {
            canActive = true;
            Material[] originM = transform.GetChild(1).GetComponent<MeshRenderer>().materials;
            originM[0] = materials[2];
            originM[3] = materials[3];
            transform.GetChild(1).GetComponent<MeshRenderer>().materials = originM;
        }
        else if (type == ObjectType.Return)
        {
            canActive = true;
            Renderer rd = transform.parent.GetChild(0).GetComponent<MeshRenderer>();
            rd.material = materials[1];
        }
    }

    public void OffHighlight(bool isCooked)
    {
        if (type == ObjectType.CounterTop || type == ObjectType.Board || type == ObjectType.Bin || type == ObjectType.Plate)
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
            if (!isCooked)
            {
                rd.material = materials[0];
            }
            else
            {
                rd.material = materials[2];
            }
        }
        else if (type == ObjectType.Station)
        {
            canActive = false;
            Material[] originM = transform.GetChild(1).GetComponent<MeshRenderer>().materials;
            originM[0] = materials[0];
            originM[3] = materials[1];
            transform.GetChild(1).GetComponent<MeshRenderer>().materials = originM;
        }
        else if (type == ObjectType.Return)
        {
            canActive = false;
            Renderer rd = transform.parent.GetChild(0).GetComponent<MeshRenderer>();
            rd.material = materials[0];
        }
    }
}

    //public Vector3 PutSomething()
    //{
    //    if (!onSomething)
    //    {
    //        onSomething = true;
    //        if (type == ObjectType.CounterTop)
    //        {
    //            Vector3 position = transform.parent.GetChild(1).position; //지정해둔 Pos 위치에 놓기
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

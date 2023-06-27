using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    public enum HandleType { Fish, Shrimp, Plate, Lettuce, Tomato, Cucumber, Chicken, Potato }
    public HandleType hand;
    public bool isOnDesk = true;
    public bool isCooked = false;

    [SerializeField] private Mesh cookedIngredient;
    [SerializeField] private Material cookedFish;

    private Vector3 fishLocalPos = new Vector3(0, 0.138f, 0.08f);
    private Vector3 shrimpLocalPos = new Vector3(-0.365000874f, -0.0890001357f, -0.423000485f);

    private Quaternion shrimpLocalRotation = new Quaternion(0.287927657f, 0.0875888839f, -0.277543157f, 0.912357152f);


    public void IngredientHandle(Transform something, HandleType handle)
    {
        //들려있을때는 collider끄기
        transform.parent.GetComponent<MeshCollider>().isTrigger = true;
        if (handle == HandleType.Fish)
        {
            transform.parent.localPosition = fishLocalPos;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Shrimp)
        {
            transform.parent.localPosition = shrimpLocalPos;
            transform.parent.localRotation = Quaternion.Euler(new Vector3(35.029995f, -1.04264745e-06f, 326.160004f));
        }
        else if (handle == HandleType.Lettuce)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Tomato)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Cucumber)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Chicken)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Potato)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.localRotation = Quaternion.identity;
        }
        transform.parent.parent.SetParent(something);
        transform.parent.parent.localRotation = Quaternion.identity;
        transform.parent.parent.localPosition = new Vector3(-0.409999996f, 0, 1.84000003f); //손 위치로 위치
        //transform.parent.parent.localRotation = Quaternion.identity;
        //transform.parent.localRotation = Quaternion.identity;
    }
    public void IngredientHandleOff(Transform parent, Vector3 target, HandleType handle)
    {
        transform.parent.GetComponent<MeshCollider>().isTrigger = false;
        if (handle == HandleType.Fish)
        {
            transform.parent.localPosition = fishLocalPos;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Shrimp)
        {
            transform.parent.localPosition = shrimpLocalPos;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Lettuce)
        {
            transform.parent.localPosition = new Vector3(0, 0, 0);
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Tomato)
        {
            transform.parent.localPosition = new Vector3(0, 0, 0);
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Cucumber)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Chicken)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Potato)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.GetChild(0).localRotation = Quaternion.identity;
        }
        //transform.parent.parent.transform.rotation = Quaternion.identity;
        transform.parent.parent.SetParent(parent);
        transform.parent.parent.localPosition = target;
        transform.parent.parent.localRotation = Quaternion.identity;
    }
    public void IngredientAuto(Transform parent, Vector3 target, HandleType handle)
    {
        transform.parent.GetComponent<MeshCollider>().isTrigger = false;
        if (handle == HandleType.Fish)
        {
            transform.parent.localPosition = fishLocalPos;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Shrimp)
        {
            transform.parent.localPosition = shrimpLocalPos;
            transform.parent.localRotation = Quaternion.Euler(new Vector3(35.029995f, -1.04264745e-06f, 326.160004f));
        }
        else if (handle == HandleType.Lettuce)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Tomato)
        {
            transform.parent.localPosition = new Vector3(0, 0, 0);
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Cucumber)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Chicken)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.localRotation = Quaternion.identity;
        }
        else if (handle == HandleType.Potato)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.GetChild(0).localRotation = Quaternion.identity;
        }
        transform.parent.parent.SetParent(parent);
        transform.parent.parent.localRotation = Quaternion.identity;
        transform.parent.parent.localPosition = target;
    }
    public void PlayerHandle(Transform something)
    {
        transform.SetParent(something);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(-0.4f, 0.24f, 2.23f);
    }
    public void PlayerHandleOff(Transform parent, Vector3 target)
    {
        transform.SetParent(parent);
        transform.rotation = Quaternion.identity;
        transform.localPosition = target;
    }

    public void changeMesh(HandleType handType)
    {
        transform.parent.GetComponent<MeshFilter>().mesh = cookedIngredient;
        transform.parent.GetComponent<MeshCollider>().sharedMesh = cookedIngredient;
        if (handType == HandleType.Fish)
        {
            MeshRenderer mesh = transform.parent.GetComponent<MeshRenderer>();
            mesh.material = cookedFish;

        }
        else if (handType == HandleType.Tomato)
        {
            transform.parent.GetComponent<MeshCollider>().sharedMesh = cookedIngredient;
        }
    }
}

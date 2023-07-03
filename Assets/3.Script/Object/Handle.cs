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

    public Vector3 fishLocalPos = new Vector3(0, 0.138f, 0.08f);
    public Vector3 shrimpLocalPos = new Vector3(-0.365000874f, -0.0890001357f, -0.423000485f);
    public Vector3 lettuceLocalPos = new Vector3(0, 0.418000013f, 0);

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
            transform.parent.localPosition = lettuceLocalPos;
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
            if (!isCooked)
            {
                transform.parent.localPosition = shrimpLocalPos;
                transform.parent.localRotation = Quaternion.Euler(new Vector3(35.029995f, -1.04264745e-06f, 326.160004f));
            }
            else
            {
                transform.parent.localPosition = Vector3.zero;
                transform.parent.localRotation = Quaternion.identity;
            }
        }
        else if (handle == HandleType.Lettuce)
        {
            transform.parent.localPosition = lettuceLocalPos;
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
        if (handle == HandleType.Shrimp)
        {
            if (!isCooked)
            {
                transform.parent.parent.localRotation = Quaternion.Euler(new Vector3(0, 244.302612f, 0));
            }
            else
            {
                transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.0008f, transform.parent.parent.localPosition.z);
            }
        }
        else if (handle == HandleType.Fish && isCooked)
        {
            transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.00146f, transform.parent.parent.localPosition.z);
        }
        else if (handle == HandleType.Fish || handle == HandleType.Cucumber || handle == HandleType.Tomato)
        {

        }
        else if (handle == HandleType.Lettuce)
        {
            if (isCooked)
            {
                //transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.5f, transform.parent.parent.localPosition.z);
            }
            else
            {
                //transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.095f, transform.parent.parent.localPosition.z);
            }
        }
        else if (handle == HandleType.Tomato)
        {
            if (!isCooked)
            {
                transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y -0.08f, transform.parent.parent.localPosition.z);
            }
            else
            {
                transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.21f, transform.parent.parent.localPosition.z);
            }
        }
        else
        {
            transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.0025f, transform.parent.parent.localPosition.z);
        }
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
            if (!isCooked)
            {
                transform.parent.localPosition = shrimpLocalPos;
                transform.parent.localRotation = Quaternion.Euler(new Vector3(35.029995f, -1.04264745e-06f, 326.160004f));
            }
            else
            {
                transform.parent.localPosition = Vector3.zero;
                transform.parent.localRotation = Quaternion.identity;
            }
        }
        else if (handle == HandleType.Lettuce)
        {
            transform.parent.localPosition = lettuceLocalPos;
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
        if (handle == HandleType.Shrimp && !isCooked)
        {
            if (!isCooked)
            {
                transform.parent.parent.localRotation = Quaternion.Euler(new Vector3(0, 244.302612f, 0));
            }
            else
            {
                transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y- 0.0023f, transform.parent.parent.localPosition.z);
            }
        }
        else if (handle == HandleType.Fish && isCooked)
        {
            if (isCooked)
            {
                transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.00224f, transform.parent.parent.localPosition.z);
            }
        }
        else if (handle == HandleType.Lettuce)
        {
            if (isCooked)
            {
                //transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.5f, transform.parent.parent.localPosition.z);
            }
            else
            {
                transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.095f, transform.parent.parent.localPosition.z);
            }
        }
        else if (handle == HandleType.Tomato)
        {
            if (!isCooked)
            {
                transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.08f, transform.parent.parent.localPosition.z);
            }
            else
            {
                transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.21f, transform.parent.parent.localPosition.z);
            }
        }
            //}
            //else 
            //{
            //    transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.0025f, transform.parent.parent.localPosition.z);
            //}
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
            transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.0017f, transform.parent.parent.localPosition.z);
        }
        else if (handType == HandleType.Tomato)
        {
            transform.parent.GetComponent<MeshCollider>().sharedMesh = cookedIngredient;
        }
        else if (handType == HandleType.Shrimp)
        {
            transform.parent.localPosition = Vector3.zero;
            transform.parent.localRotation = Quaternion.identity;
            transform.parent.parent.localRotation = Quaternion.identity;
            transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.0013f, transform.parent.parent.localPosition.z);
        }
        else if (handType == HandleType.Tomato)
        {
            transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.017f, transform.parent.parent.localPosition.z);
        }
        else if (handType == HandleType.Lettuce)
        {
            MeshRenderer mesh = transform.parent.GetComponent<MeshRenderer>();
            mesh.material = cookedFish;
            transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.0028f, transform.parent.parent.localPosition.z);
        }
        else
        {
            transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, transform.parent.parent.localPosition.y - 0.0025f, transform.parent.parent.localPosition.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("deadZone"))
        {
            if (hand != HandleType.Plate)
            {
                GameObject parent = transform.parent.parent.gameObject;
                parent.GetComponent<DestroyIngredient>().DestroySelf();
            }
            else //접시가 떨어지면 접시만 리스폰
            {
                Destroy(gameObject);
                GameManager.instance.PlateReturn();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        if (other.CompareTag("Plate")) return;
        if (!(FindObjectOfType<PlayerController>().transform.childCount > 1 && FindObjectOfType<PlayerController>().transform.GetChild(1).GetChild(0) != null && !FindObjectOfType<PlayerController>().transform.GetChild(1).GetChild(0).GetChild(0).Equals(other)) && !(FindObjectOfType<Player2Controller>().transform.childCount > 1 && FindObjectOfType<Player2Controller>().transform.GetChild(1).GetChild(0) != null && !FindObjectOfType<Player2Controller>().transform.GetChild(1).GetChild(0).GetChild(0).Equals(other)))
        {
            if (other.GetComponent<MeshCollider>() == null)
            {
                return;
            }
            if (!transform.parent.parent.GetChild(0).GetComponent<Object>().onSomething)
            {
                Transform Collider = transform.parent.parent.GetChild(0);
                GameObject handleThing = other.gameObject;
                if (Collider.GetComponent<Object>().type == Object.ObjectType.CounterTop)
                {
                    if (handleThing.CompareTag("Ingredient"))
                    {
                        Collider.GetComponent<Object>().onSomething = true;
                        handleThing.transform.GetChild(0).GetComponent<Handle>().isOnDesk = true;
                        handleThing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        handleThing.transform.GetChild(0).GetComponent<Handle>().IngredientAuto(transform.parent.parent, transform.parent.parent.GetChild(1).localPosition, handleThing.transform.GetChild(0).GetComponent<Handle>().hand);
                    }
                    else //접시
                    {
                        Collider.GetComponent<Object>().onSomething = true;
                        handleThing.transform.GetChild(0).GetComponent<Handle>().isOnDesk = true;
                        handleThing.transform.GetChild(0).GetComponent<Handle>().PlayerHandleOff(transform.parent.parent, transform.parent.parent.GetChild(1).localPosition);
                    }
                }
                else if (Collider.GetComponent<Object>().type == Object.ObjectType.Board)
                {
                    if (handleThing.CompareTag("Ingredient"))
                    {
                        Collider.GetComponent<Object>().onSomething = true;
                        handleThing.transform.GetChild(0).GetComponent<Handle>().isOnDesk = true;
                        handleThing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        handleThing.transform.GetChild(0).GetComponent<Handle>().IngredientAuto(transform.parent.parent, transform.parent.parent.GetChild(1).localPosition, handleThing.transform.GetChild(0).GetComponent<Handle>().hand);
                    }
                }
                else if (Collider.GetComponent<Object>().type == Object.ObjectType.Craft)
                {
                    if (handleThing.CompareTag("Ingredient"))
                    {
                        Collider.GetComponent<Object>().onSomething = true;
                        handleThing.transform.GetChild(0).GetComponent<Handle>().isOnDesk = true;
                        handleThing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        handleThing.transform.GetChild(0).GetComponent<Handle>().IngredientAuto(transform.parent.parent.parent.parent, transform.parent.localPosition, handleThing.transform.GetChild(0).GetComponent<Handle>().hand);
                    }
                    else //접시
                    {
                        if (Collider.GetComponent<Object>().type != Object.ObjectType.Board)
                        {
                            Collider.GetComponent<Object>().onSomething = true;
                            handleThing.transform.GetChild(0).GetComponent<Handle>().isOnDesk = true;
                            handleThing.transform.GetChild(0).GetComponent<Handle>().PlayerHandleOff(transform.parent.parent.parent.parent, transform.parent.localPosition);
                        }

                    }
                }
            }
        }
    }
}

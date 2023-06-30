using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inputfield : MonoBehaviour
{
    [SerializeField] private InputField player1;
    [SerializeField] private InputField player2;

    private void Update()
    {
        NameManager.instance.SetName1(player1.text);
        NameManager.instance.SetName2(player2.text);
    }
}

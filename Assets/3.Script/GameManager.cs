using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] private GameObject platePrefabs;
    [SerializeField] private float respawnTime = 3f;
    [SerializeField] private GameObject ReturnCounter;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlateReturn()
    {
        StartCoroutine(PlateReturn_co());
    }
    IEnumerator PlateReturn_co()
    {
        yield return new WaitForSeconds(respawnTime);
        GameObject newPlate = Instantiate(platePrefabs, Vector3.zero, Quaternion.identity);
        newPlate.transform.SetParent(ReturnCounter.transform);
        ReturnCounter.transform.GetChild(1).GetComponent<Return>().returnPlates.Add(newPlate);
        Vector3 spawnPos = ReturnCounter.transform.GetChild(1).GetComponent<Return>().SetPosition();
        newPlate.transform.localPosition = spawnPos;
    }
}

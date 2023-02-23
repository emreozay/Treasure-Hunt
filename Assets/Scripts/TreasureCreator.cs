using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCreator : MonoBehaviour
{
    [SerializeField]
    private Transform holeParent;
    [SerializeField]
    private GameObject holePrefab;

    public static TreasureCreator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void CreateNewTreasure()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-15f, 15f), Random.Range(-20f, 20f), 0);
        GameObject newHole = Instantiate(holePrefab, randomSpawnPosition, Quaternion.identity, holeParent);
    }
}

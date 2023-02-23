using System.Collections;
using UnityEngine;

public class CollectableCreator : MonoBehaviour
{
    [SerializeField]
    private Transform holeParent;
    [SerializeField]
    private GameObject holePrefab;

    [SerializeField]
    private GameObject boostObject;
    [SerializeField]
    private GameObject freezeObject;

    public static CollectableCreator Instance { get; private set; }

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

    private void Start()
    {
        StartCoroutine(SpawnBoosts());
    }

    public void CreateNewTreasure()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-15f, 15f), Random.Range(-20f, 20f), 0);
        GameObject newHole = Instantiate(holePrefab, randomSpawnPosition, Quaternion.identity, holeParent);
    }

    private IEnumerator SpawnBoosts()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            Vector3 randomSpawnPosition = new Vector3(Random.Range(-15f, 15f), Random.Range(-20f, 20f), 0);
            boostObject.transform.position = randomSpawnPosition;
            boostObject.SetActive(true);

            randomSpawnPosition = new Vector3(Random.Range(-15f, 15f), Random.Range(-20f, 20f), 0);
            freezeObject.transform.position = randomSpawnPosition;
            freezeObject.SetActive(true);
        }
    }
}

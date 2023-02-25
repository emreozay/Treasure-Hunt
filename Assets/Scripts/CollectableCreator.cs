using System.Collections;
using UnityEngine;

public class CollectableCreator : MonoBehaviour
{
    [SerializeField]
    private Transform holeParent;
    [SerializeField]
    private GameObject holePrefab;

    private GameObject megaBoots;
    private GameObject frostArrow;

    private Collider2D megaBootsCollider;
    private Collider2D frostArrowCollider;

    private SpriteRenderer megaBootsSpriteRenderer;
    private SpriteRenderer frostArrowSpriteRenderer;

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
        yield return new WaitForSeconds(5f);
        GetBoostObjectsComponents();

        if (megaBoots == null || frostArrow == null)
            yield break;

        while (true)
        {
            yield return new WaitForSeconds(5f);

            Vector3 randomSpawnPosition = LevelManager.Instance.GetRandomPosition();
            megaBoots.transform.position = randomSpawnPosition;

            randomSpawnPosition = LevelManager.Instance.GetRandomPosition();
            frostArrow.transform.position = randomSpawnPosition;

            ActivateBoostObjects();
        }
    }

    private void ActivateBoostObjects()
    {
        megaBootsCollider.enabled = true;
        frostArrowCollider.enabled = true;

        megaBootsSpriteRenderer.enabled = true;
        frostArrowSpriteRenderer.enabled = true;
    }

    private void GetBoostObjectsComponents()
    {
        megaBoots = GameObject.FindGameObjectWithTag("Boost");
        frostArrow = GameObject.FindGameObjectWithTag("Freeze");

        megaBootsCollider = megaBoots.GetComponent<Collider2D>();
        frostArrowCollider = frostArrow.GetComponent<Collider2D>();

        megaBootsSpriteRenderer = megaBoots.GetComponent<SpriteRenderer>();
        frostArrowSpriteRenderer = frostArrow.GetComponent<SpriteRenderer>();
    }
}

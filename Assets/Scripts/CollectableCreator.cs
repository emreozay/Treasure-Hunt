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

    private LayerMask holeMask;

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

        holeMask = LayerMask.GetMask("Hole");
    }

    public void CreateNewTreasure()
    {
        Vector3 randomSpawnPosition = LevelManager.Instance.GetRandomPosition();

        Collider2D[] col = Physics2D.OverlapCircleAll(randomSpawnPosition, 7f, holeMask);
        
        while (col.Length > 0)
        {
            randomSpawnPosition = LevelManager.Instance.GetRandomPosition();

            col = Physics2D.OverlapCircleAll(randomSpawnPosition, 7f, holeMask);
        }

        Instantiate(holePrefab, randomSpawnPosition, Quaternion.identity, holeParent);
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

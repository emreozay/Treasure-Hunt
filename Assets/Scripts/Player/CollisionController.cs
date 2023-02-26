using System.Collections;
using TMPro;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [SerializeField]
    private GameObject pouch;

    [SerializeField]
    private Transform enemyParent;

    private Animator animator;
    private Movement movement;

    private PlayerUIController playerUI;

    private int score;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        playerUI = GetComponent<PlayerUIController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hole"))
        {
            StartCoroutine(Dig(collision.transform));
        }
        if (collision.CompareTag("Boost"))
        {
            DeactivateBoostObject(collision.gameObject);
            StartCoroutine(SpeedBoost());
        }
        if (collision.CompareTag("Freeze"))
        {
            DeactivateBoostObject(collision.gameObject);
            StartCoroutine(Freeze());
        }
    }

    private IEnumerator Dig(Transform newHole)
    {
        newHole.tag = "DugHole";

        movement.StopMoving();
        newHole.position = transform.position;

        animator.SetTrigger("isDigging");
        newHole.GetComponent<Animator>().SetTrigger("isDigging");

        yield return new WaitForSeconds(2f);

        pouch.SetActive(true);

        yield return new WaitForSeconds(1f);
        pouch.SetActive(false);

        score++;
        playerUI.SetScoreText(score);

        movement.ContinueMoving();

        CollectableCreator.Instance.CreateNewTreasure();
    }

    private IEnumerator SpeedBoost()
    {
        movement.MaxMovementSpeed *= 1.1f;
        movement.MovementSpeed = movement.MaxMovementSpeed;

        yield return new WaitForSeconds(5f);

        movement.MaxMovementSpeed /= 1.1f;
        movement.MovementSpeed = movement.MaxMovementSpeed;
    }

    private IEnumerator Freeze()
    {
        EnemyMovement[] enemyMovements = enemyParent.GetComponentsInChildren<EnemyMovement>();

        for (int i = 0; i < enemyMovements.Length; i++)
        {
            enemyMovements[i].FreezeCharacter();
        }

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < enemyMovements.Length; i++)
        {
            if (enemyMovements[i] != null || enemyMovements[i].gameObject.activeSelf == true)
                enemyMovements[i].UnfreezeCharacter();
        }
    }

    private void DeactivateBoostObject(GameObject boostObject)
    {
        boostObject.GetComponent<Collider2D>().enabled = false;
        boostObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [SerializeField]
    private GameObject pouch;

    [SerializeField]
    private Transform scoreTextParent; // Change this later!!!

    private Animator animator;
    private Movement movement;

    private int score;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hole"))
        {
            StartCoroutine(Dig(collision.transform));
        }
        if (collision.CompareTag("Boost"))
        {
            Destroy(collision.gameObject);

            movement.MaxMovementSpeed *= 1.2f;
            movement.MovementSpeed = movement.MaxMovementSpeed;
        }
        if (collision.CompareTag("Freeze"))
        {
            //StartCoroutine(Dig(collision.transform));
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
        UIManager.Instance.SetScoreText(scoreTextParent, score);

        movement.ContinueMoving();

        TreasureCreator.Instance.CreateNewTreasure();
    }
}
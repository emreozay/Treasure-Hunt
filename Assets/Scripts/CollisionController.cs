using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [SerializeField]
    private GameObject pouch;

    private Animator animator;
    private Movement movement;

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

        movement.ContinueMoving();
    }
}

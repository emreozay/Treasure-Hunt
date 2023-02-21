using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [SerializeField]
    private GameObject pouch;

    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
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

        playerController.StopMoving();
        newHole.position = transform.position;

        animator.SetTrigger("isDigging");
        newHole.GetComponent<Animator>().SetTrigger("isDigging");

        yield return new WaitForSeconds(2f);

        pouch.SetActive(true);

        yield return new WaitForSeconds(1f);
        pouch.SetActive(false);

        playerController.ContinueMoving();
    }
}

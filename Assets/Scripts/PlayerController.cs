using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private FloatingJoystick floatingJoystick;

    private Animator animator;
    private Rigidbody2D playerRigidbody;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private GameObject hole;
    [SerializeField]
    private GameObject pouch;

    private bool isMoving = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    private void FixedUpdate()
    {
        MoveWithJoystick();
    }

    private void MoveWithJoystick()
    {
        if (!isMoving)
            return;

        playerRigidbody.velocity = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical).normalized * moveSpeed;

        if (floatingJoystick.Horizontal != 0 || floatingJoystick.Vertical != 0)
        {
            animator.SetBool("isRunning", true);
            animator.SetFloat("Horizontal", floatingJoystick.Horizontal);
            animator.SetFloat("Vertical", floatingJoystick.Vertical);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerRigidbody.velocity = Vector2.zero;
        isMoving = false;

        collision.gameObject.SetActive(false);

        StartCoroutine(Dig());
    }

    private IEnumerator Dig()
    {
        animator.SetBool("isRunning", false);
        animator.SetTrigger("isDigging");
        Instantiate(hole, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        pouch.transform.position = transform.position;
        pouch.SetActive(true);

        yield return new WaitForSeconds(1f);
        pouch.SetActive(false);
        isMoving = true;
    }
}
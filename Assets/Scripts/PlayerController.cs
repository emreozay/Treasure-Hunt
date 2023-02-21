using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private FloatingJoystick floatingJoystick;

    [SerializeField]
    private float movementSpeed;

    public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }

    private Animator animator;
    private Rigidbody2D playerRigidbody;

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

        playerRigidbody.velocity = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical).normalized * movementSpeed;

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

    public void StopMoving()
    {
        playerRigidbody.velocity = Vector2.zero;
        isMoving = false;
        animator.SetBool("isRunning", false);
    }

    public void ContinueMoving()
    {
        isMoving = true;
        animator.SetBool("isRunning", true);
    }
}
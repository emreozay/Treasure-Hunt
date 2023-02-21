using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Movement
{
    [SerializeField]
    private FloatingJoystick floatingJoystick;

    [SerializeField]
    private float movementSpeed;

    private Rigidbody2D playerRigidbody;

    public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    private void FixedUpdate()
    {
        Move();
        Animate();
    }

    protected override void Move()
    {
        if (!isMoving)
            return;

        playerRigidbody.velocity = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical).normalized * movementSpeed;
    }

    protected override void Animate()
    {
        if (!isMoving)
            return;

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

    public override void ContinueMoving()
    {
        isMoving = true;
        animator.SetBool("isRunning", true);
    }

    public override void StopMoving()
    {
        playerRigidbody.velocity = Vector2.zero;
        isMoving = false;
        animator.SetBool("isRunning", false);
    }
}
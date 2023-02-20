using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private FloatingJoystick floatingJoystick;

    private Animator animator;
    private Rigidbody2D playerRigidbody;

    [SerializeField]
    private float moveSpeed;

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
}

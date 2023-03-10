using UnityEngine;

public class PlayerController : Movement
{
    [SerializeField]
    private FloatingJoystick floatingJoystick;

    private Rigidbody2D playerRigidbody;

    public override float MovementSpeed { get; set; }

    protected override void Awake()
    {
        base.Awake();

        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        MovementSpeed = MaxMovementSpeed;
    }

    private void FixedUpdate()
    {
        Move();
        Animate();
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

    protected override void Move()
    {
        if (!isMoving)
            return;

        playerRigidbody.velocity = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical).normalized * MovementSpeed;
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
}
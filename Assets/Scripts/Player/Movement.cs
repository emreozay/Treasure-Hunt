using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public abstract float MovementSpeed { get; set; }
    public float MaxMovementSpeed = 7f;

    protected Animator animator;
    
    protected bool isMoving = false;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        UIManager.CountdownFinishAction += ContinueMoving;
    }

    protected abstract void Move();

    protected abstract void Animate();

    public abstract void ContinueMoving();

    public abstract void StopMoving();

    private void OnDestroy()
    {
        UIManager.CountdownFinishAction -= ContinueMoving;
    }
}
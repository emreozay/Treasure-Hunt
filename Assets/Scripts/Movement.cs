using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public abstract float MovementSpeed { get; set; }

    protected Animator animator;

    protected bool isMoving = true;

    protected abstract void Move();

    protected abstract void Animate();

    public abstract void ContinueMoving();

    public abstract void StopMoving();
}
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    protected Animator animator;

    protected bool isMoving = true;

    protected abstract void Move();

    protected abstract void Animate();

    public abstract void ContinueMoving();

    public abstract void StopMoving();
}
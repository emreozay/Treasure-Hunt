using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public abstract float MovementSpeed { get; set; }
    public float MaxMovementSpeed = 6f;

    protected Animator animator;
    private Vector2 firstPosition;

    private SpriteRenderer spriteRenderer;
    private Sprite firstSprite;

    protected bool isMoving = true;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //LevelManager.Instance.NextLevelAction += SetFirstPosition;
        //LevelManager.Instance.NextLevelAction += SetFirstSprite;
    }

    protected virtual void Start()
    {
        firstPosition = transform.position;
        firstSprite = spriteRenderer.sprite;
    }

    protected abstract void Move();

    protected abstract void Animate();

    public abstract void ContinueMoving();

    public abstract void StopMoving();

    private void SetFirstPosition()
    {
        Time.timeScale = 0;
        transform.position = firstPosition;
    }

    private void SetFirstSprite()
    {
        spriteRenderer.sprite = firstSprite;
    }

    private void OnDestroy()
    {
        //LevelManager.Instance.NextLevelAction -= SetFirstPosition;
        //LevelManager.Instance.NextLevelAction -= SetFirstSprite;
    }
}
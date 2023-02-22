using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : Movement
{
    [SerializeField]
    private Vector3 target;

    [SerializeField]
    private bool newTarget;

    private NavMeshAgent agent;

    [SerializeField]
    private Transform holeParent;

    [SerializeField]
    private Color newColor;
    private Vector3 newDestination;
    private Vector3 currentDestination;
    private Vector3 randomDestination;
    public override float MovementSpeed { get => agent.speed; set => SetNewDestination(value); }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        agent.speed = MovementSpeed;

        SetRandomDestination();
    }

    void Update()
    {
        Move();
        Animate();
    }

    protected override void Move()
    {
        if ((agent.remainingDistance < 0.1f && isMoving) || agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            currentDestination = agent.destination;

            SetRandomDestination();
        }
    }

    protected override void Animate()
    {
        if (!isMoving)
            return;

        Vector2 agentVelocity = agent.velocity.normalized;

        if (agentVelocity != Vector2.zero)
        {
            animator.SetBool("isRunning", true);

            animator.SetFloat("Horizontal", agentVelocity.x);
            animator.SetFloat("Vertical", agentVelocity.y);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    public override void ContinueMoving()
    {
        isMoving = true;
        agent.isStopped = false;
        SetRandomDestination();
        animator.SetBool("isRunning", true);
    }

    public override void StopMoving()
    {
        agent.velocity = Vector2.zero;
        agent.isStopped = true;
        agent.ResetPath();

        isMoving = false;
        animator.SetBool("isRunning", false);
    }

    private void SetRandomDestination()
    {
        Vector3 newRandomDestination = new Vector3(Random.Range(-15f, 15f), Random.Range(-20f, 20f), 0);
        randomDestination = newRandomDestination;
        agent.SetDestination(newRandomDestination);
    }

    private void SetNewDestination(float newSpeed)
    {
        float oldSpeed = agent.speed;
        agent.speed = newSpeed;

        if (!isMoving)
            return;

        if (oldSpeed < newSpeed)
        {
            Vector3 positionDifference = agent.destination - transform.position;
            newDestination = transform.position - (positionDifference.normalized * 3f);
            //newDestination = new Vector3(Mathf.Clamp(newDestination.x, -15f, 15f), Mathf.Clamp(newDestination.y, -20f, 20f), 0);

            newDestination.x += Mathf.Cos(Random.Range(180f, 360f) / (180f / Mathf.PI)) * 3f;
            newDestination.y += Mathf.Sin(Random.Range(180f, 360f) / (180f / Mathf.PI)) * 3f;
            newDestination = new Vector3(Mathf.Clamp(newDestination.x, -15f, 15f), Mathf.Clamp(newDestination.y, -20f, 20f), 0);

            agent.SetDestination(newDestination);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = newColor;
        Gizmos.DrawSphere(newDestination, 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(randomDestination, 0.5f);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(currentDestination, 0.5f);
    }
}
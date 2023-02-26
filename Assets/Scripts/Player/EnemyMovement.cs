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

    private Vector3 newDestination;

    public override float MovementSpeed { get => agent.speed; set => SetCorrectDestination(value); }

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected override void Start()
    {
        base.Start();
        agent.speed = MaxMovementSpeed;

        SetAgentDestination();
    }

    void Update()
    {
        Move();
        Animate();
    }

    public override void ContinueMoving()
    {
        isMoving = true;
        agent.isStopped = false;
        
        SetAgentDestination();
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

    protected override void Move()
    {
        if ((agent.remainingDistance < 0.1f && isMoving) || agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            SetAgentDestination();
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

    private void SetAgentDestination()
    {
        EasyAIDestination();
    }

    private void EasyAIDestination()
    {
        Vector3 newRandomDestination = LevelManager.Instance.GetRandomPosition();
        
        agent.SetDestination(newRandomDestination);
    }

    private void MediumAIDestination()
    {
        Transform randomHole = holeParent.GetChild(Random.Range(0, holeParent.childCount));
        while (randomHole.CompareTag("DugHole"))
        {
            randomHole = holeParent.GetChild(Random.Range(0, holeParent.childCount));
        }

        Vector2 newDestination = randomHole.position;
        newDestination += new Vector2(Random.Range(-6f, 6f), Random.Range(-6f, 6f));

        agent.SetDestination(newDestination);
    }

    private void HardAIDestination()
    {
        Transform randomHole = holeParent.GetChild(Random.Range(0, holeParent.childCount));
        while (randomHole.CompareTag("DugHole"))
        {
            randomHole = holeParent.GetChild(Random.Range(0, holeParent.childCount));
        }

        Vector2 newDestination = randomHole.position;
        newDestination += new Vector2(Random.Range(-4.5f, 4.5f), Random.Range(-4.5f, 4.5f));

        agent.SetDestination(newDestination);
    }

    private void SetCorrectDestination(float newSpeed)
    {
        float oldSpeed = agent.speed;
        agent.speed = newSpeed;

        if (!isMoving)
            return;

        if (oldSpeed < newSpeed)
        {
            Vector3 positionDifference = agent.destination - transform.position;
            newDestination = transform.position - (positionDifference.normalized * 3f);

            newDestination.x += Mathf.Cos(Random.Range(180f, 360f) / (180f / Mathf.PI)) * 3f;
            newDestination.y += Mathf.Sin(Random.Range(180f, 360f) / (180f / Mathf.PI)) * 3f;

            Vector2 boundary = LevelManager.Instance.GetMapSize();
            newDestination = new Vector3(Mathf.Clamp(newDestination.x, -boundary.x, boundary.x), Mathf.Clamp(newDestination.y, -boundary.y, boundary.y), 0);

            agent.SetDestination(newDestination);
        }
    }

    public void FreezeCharacter()
    {
        animator.SetBool("isRunning", false);
        isMoving = false;
        agent.velocity = Vector2.zero;
        agent.isStopped = true;
    }

    public void UnfreezeCharacter()
    {
        isMoving = true;
        agent.isStopped = false;
    }
}
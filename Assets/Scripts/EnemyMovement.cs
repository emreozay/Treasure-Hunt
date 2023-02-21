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

    private void Awake()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        Move();
    }

    void Update()
    {/*
        if(newTarget)
            Move();
        */
        Animate();
    }

    protected override void Move()
    {
        //agent.SetDestination(Vector2.left*5f);

        foreach (Transform hole in holeParent)
        {
            if (hole.CompareTag("Hole"))
            {
                agent.SetDestination(hole.position);
                return;
            }
        }

        //agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
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
        Move();
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
}
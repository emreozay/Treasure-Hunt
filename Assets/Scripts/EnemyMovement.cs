using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 target;

    private Animator animator;

    private NavMeshAgent agent;

    private bool isMoving = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        CheckAnimations();
        SetAgentPosition();
    }

    private void CheckAnimations()
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

    private void SetAgentPosition()
    {
        //agent.SetDestination(Vector2.left*5f);
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }
}

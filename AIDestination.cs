using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDestination : MonoBehaviour
{
    public Transform target;                                    // target to aim for

    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    public AICharacterMovement character { get; private set; }
    public bool canReachTarget = true;
    public bool rotationTowardsTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<AICharacterMovement>();

        agent.updateRotation = false;
        agent.updatePosition = true;

        target = new GameObject().transform;
        target.name = "AI Target";
        SetTarget(transform.position);
    }
    private void OnDisable()
    {
        Destroy(target.gameObject);
    }

    public bool CanReachTarget(Transform _target)
    {
        UnityEngine.AI.NavMeshPath navMeshPath = new UnityEngine.AI.NavMeshPath();
        if (agent.CalculatePath(target.position, navMeshPath) && navMeshPath.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            canReachTarget = true;
            return true;
        }
        else
        {
            canReachTarget = false;

            return false;
        }
    }
    private void FixedUpdate()
    {
        if (target != null)
            agent.SetDestination(target.position);

        //move to target
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            // agent.isStopped =false;

            character.Move(agent.desiredVelocity);
        }
        else
        {
            character.Move(Vector3.zero);

            // agent.isStopped = true;
        }


    }


    public void SetTarget(Vector3 newTargetPos)
    {
        target.position = newTargetPos;
    }
    public void NavAgentDie()
    {
        agent.SetDestination(transform.position);

        character.Move(Vector3.zero);
        agent.isStopped = true;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(target.position, agent.stoppingDistance);
    }
}

using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AgentBehaviour))]
public class AgentMoveBehavior : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    private AgentBehaviour agentBehaviour;
    private ITarget CurrentTarget;
    [SerializeField] private float minMoveDistance = 0.25f;
    public int distanceForCheckingGround = 25;
    private bool isAgentOnNavMesh = false;
    private Vector3 lastPosition;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        agentBehaviour = GetComponent<AgentBehaviour>();
    }


    private void OnEnable()
    {
        agentBehaviour.Events.OnTargetChanged += EventsOnTargetChanged;
        agentBehaviour.Events.OnTargetOutOfRange += EventsOnTargetOutOfRange;
    }

    private void OnDisable()
    {
        agentBehaviour.Events.OnTargetChanged -= EventsOnTargetChanged;
        agentBehaviour.Events.OnTargetOutOfRange -= EventsOnTargetOutOfRange;
    }

    private void EventsOnTargetOutOfRange(ITarget target)
    {
        
    }

    private void EventsOnTargetChanged(ITarget target, bool inRange)
    {
        CurrentTarget = target;
        navMeshAgent.isStopped = false;
        lastPosition = CurrentTarget.Position;
        navMeshAgent.SetDestination(target.Position);
    }


    public void stopMoving()
    {
        if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = true;
        }
    }

    private void Update()
    {
        if (CurrentTarget == null || !navMeshAgent.isOnNavMesh)
        {
            return;
        }

        // Move to target if it has moved enough since last update
        if (minMoveDistance <= Vector3.Distance(CurrentTarget.Position, lastPosition))
        {
            lastPosition = CurrentTarget.Position;
            navMeshAgent.SetDestination(CurrentTarget.Position);
        }
    }
}

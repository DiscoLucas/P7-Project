using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AgentBehaviour))]
public class AgentMoveBehavior : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private AgentBehaviour agentBehaviour;
    private ITarget CurrentTarget;
    [SerializeField] private float minMoveDistance = 0.25f;

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
        if (inRange) {
            CurrentTarget = target;
            lastPosition = CurrentTarget.Position;
            navMeshAgent.SetDestination(target.Position);
        }
        
    }

    private void Update()
    {
        if (CurrentTarget == null)
        {
            return;
        }

      
        if (minMoveDistance <= Vector3.Distance(CurrentTarget.Position, lastPosition))
        {
            lastPosition = CurrentTarget.Position;
            navMeshAgent.SetDestination(CurrentTarget.Position);
        }
    }
}




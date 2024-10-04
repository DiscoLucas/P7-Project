using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class MonsterBehavior : MonoBehaviour
{
    AgentBehaviour agent;
    ITarget currentTarget;
    bool shouldMove;

    public NavMeshAgent navMeshAgent;

    
    void Awake()
    {
        agent = GetComponent<AgentBehaviour>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void OnTargetChanged(ITarget target, bool inRange)
    {
        currentTarget = target;
        shouldMove = !inRange;

        navMeshAgent.SetDestination(currentTarget.Position);
    }

    void OnTargetInRange(ITarget target)
    {
        shouldMove = false; 
    }
    void OnTargetOutOfRange(ITarget target)
    {
        shouldMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

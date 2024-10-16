using Assets.Scripts.GOAP.Actions;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class ScreamBehavior : MonoBehaviour
{
    private AgentBehaviour agentBehaviour;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        agentBehaviour = GetComponent<AgentBehaviour>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        agentBehaviour.Events.OnActionStart += OnActionStart;
        agentBehaviour.Events.OnActionStop += OnActionStop;
    }

    private void OnDisable()
    {
        agentBehaviour.Events.OnActionStart -= OnActionStart;
        agentBehaviour.Events.OnActionStop -= OnActionStop;
    }

    private void OnActionStart(IActionBase action)
    {
        if (action is ScreamAction)
        {
            navMeshAgent.isStopped = true; // Stop movement
        }
    }

    private void OnActionStop(IActionBase action)
    {
        if (action is ScreamAction)
        {
            navMeshAgent.isStopped = false; // Resume movement after scream ends
        }
    }
}

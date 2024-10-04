using UnityEngine;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;

public class AgentMoveBehavior : MonoBehaviour
{
    AgentBehaviour agent;
    ITarget currentTarget;
    bool shouldMove;

    private void Awake()
    {
        this.agent = this.GetComponent<AgentBehaviour>();
    }

    private void OnEnable()
    {
        agent.Events.OnTargetInRange += OnTargetInRange;
        agent.Events.OnTargetChanged += OnTargetChanged;
        agent.Events.OnTargetOutOfRange += OnTargetOutOfRange;
    }

    private void OnDisable()
    {
        agent.Events.OnTargetInRange -= OnTargetInRange;
        agent.Events.OnTargetChanged -= OnTargetChanged;
        agent.Events.OnTargetOutOfRange -= OnTargetOutOfRange;
    }

    private void OnTargetInRange(ITarget target)
    {
        this.shouldMove = false;
    }

    void OnTargetChanged(ITarget target, bool inRange)
    {
        currentTarget = target;
        shouldMove = !inRange;
    }

    void OnTargetOutOfRange(ITarget target)
    {
        shouldMove = true;
    }

    public void Update()
    {
        if (!this.shouldMove) return;
        if (this.currentTarget == null) return;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentTarget.Position.x, transform.position.y, currentTarget.Position.z), Time.deltaTime);
    }
}

using UnityEngine;
using CrashKonijn.Goap.Behaviours;

public class AgentBrain : MonoBehaviour
{
    AgentBehaviour agent;

    private void Awake()
    {
        agent = GetComponent<AgentBehaviour>();
    }

    private void Start()
    {
        //agent.SetGoal<WanderGoal>(false);
        agent.SetGoal<SpookPlayerGoal>(true);
    }
}

using CrashKonijn.Goap.Behaviours;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class MonsterBrain : MonoBehaviour
    {
        private AgentBehaviour agentBehaviour;

        private void Awake()
        {
            agentBehaviour= GetComponent<AgentBehaviour>();
        }

        void Start()
        {
            agentBehaviour.SetGoal<WanderGoalM>(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
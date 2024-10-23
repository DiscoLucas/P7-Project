using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine.AI;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;

namespace Assets.Scripts.GOAP.Actions
{
    public class ChaseAction : ActionBase<ChaseData>, IInjectableObj
    {
        private MonsterConfig config;
        private NavMeshAgent navMeshAgent; // Use NavMeshAgent to move the monster
        private Transform targetTransform;  // Store the target's transform

        public void Inject(DependencyInjector injector)
        {
            config = injector.config1 as MonsterConfig;
        }

        public override void Created()
        {
            // Initialize the action
        }

        public override void Start(IMonoAgent agent, ChaseData data)
        {
            navMeshAgent = agent.GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to the agent
            targetTransform = (data.Target as TransformTarget)?.Transform; // Set the target Transform (player)

            if (targetTransform != null)
            {
                navMeshAgent.isStopped = false; // Make sure NavMeshAgent is not stopped
                navMeshAgent.SetDestination(targetTransform.position); // Start moving toward the player
                Debug.Log("Chasing Player: Moving towards " + targetTransform.position);
            }
        }

        public override ActionRunState Perform(IMonoAgent agent, ChaseData data, ActionContext context)
        {
            // Recalculate path if necessary (if target moves)
            if (targetTransform != null)
            {
                navMeshAgent.SetDestination(targetTransform.position);

                // Check if within melee range
                float distanceToTarget = Vector3.Distance(agent.transform.position, targetTransform.position);
                if (distanceToTarget <= config.meleeRange)
                {
                    Debug.Log("ChaseAction: Player within melee range, switching to MeleeAction.");
                    return ActionRunState.Stop; // Stop the chase (MeleeAction should start)
                }

                // If still far away, continue chasing
                Debug.Log("ChaseAction: Chasing... Distance to player: " + distanceToTarget);
                return ActionRunState.Continue;
            }

            return ActionRunState.Stop; // No target, stop the chase
        }

        public override void End(IMonoAgent agent, ChaseData data)
        {
            // Stop the NavMeshAgent when the chase action ends
            navMeshAgent.isStopped = true;
            Debug.Log("ChaseAction: Stopping chase.");
        }
    }

    // Custom data class for chase action, implementing IActionData
    public class ChaseData : IActionData
    {
        public float timer { get; set; }
        public ITarget Target { get; set; }
    }
}

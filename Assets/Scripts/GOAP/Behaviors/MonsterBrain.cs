using Assets.Scripts.GOAP.Goals;
using Assets.Scripts.GOAP.Sensors;
using CrashKonijn.Goap.Behaviours;
using UnityEngine;

namespace Assets.Scripts.GOAP.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class MonsterBrain : MonoBehaviour
    {
        private AgentBehaviour agentBehaviour;
        public PlayerSensor p_sensor;
        public ProtectionAreaSensor protectionSensor;
        public PlayerAwarenessSensor awarenessSensor;
        public MonsterConfig config;
        public DependencyInjector dependencyInjector;

        private void Awake()
        {
            agentBehaviour = GetComponent<AgentBehaviour>();
        }

        void Start()
        {
            // Start with the wander goal
            agentBehaviour.SetGoal<WanderGoalM>(true);
            config = dependencyInjector.config1;
            p_sensor.Collider.radius = config.AgentSensorRadius;

            if (protectionSensor != null)
            {
                protectionSensor.Inject(dependencyInjector);
            }
        }

        private void OnEnable()
        {
            p_sensor.OnPlayerEnter += ps_OnEnter;
            p_sensor.OnPlayerExit += ps_OnExit;

            if (protectionSensor != null)
            {
                protectionSensor.OnPlayerEnter += protectionSensor_OnEnter;
                protectionSensor.OnPlayerExit += protectionSensor_OnExit;
            }
        }

        private void OnDisable()
        {
            p_sensor.OnPlayerEnter -= ps_OnEnter;
            p_sensor.OnPlayerExit -= ps_OnExit;

            if (protectionSensor != null)
            {
                protectionSensor.OnPlayerEnter -= protectionSensor_OnEnter;
                protectionSensor.OnPlayerExit -= protectionSensor_OnExit;
            }
        }

        private void ps_OnEnter(Transform player)
        {
            EvaluateGoal(player);
        }

        private void ps_OnExit(Vector3 lastKnownPosition)
        {
            // When player leaves the detection radius, return to wandering
            agentBehaviour.SetGoal<WanderGoalM>(true);
        }

        private void protectionSensor_OnEnter(Transform player)
        {
            EvaluateGoal(player);
        }

        private void protectionSensor_OnExit(Vector3 lastKnownPosition)
        {
            agentBehaviour.SetGoal<WanderGoalM>(true);
        }

        private void EvaluateGoal(Transform player)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float playerAwareness = awarenessSensor.GetPlayerAwarenessLevel(); // Player awareness level from PlayerAwarenessSensor
            float monsterAggression = config.startingAggressionLevel; // Monster's aggression score from MonsterConfig

            // Hurt player if within melee range
            if (distanceToPlayer <= config.meleeRange)
            {
                agentBehaviour.SetGoal<HurtPlayerGoal>(true);
            }
            // Chase player if within chase range and aggression/awareness is high
            else if (distanceToPlayer <= config.chaseRange &&
                     (monsterAggression >= config.aggressionThreshold ||
                      playerAwareness >= config.stalkMaxPlayerAwareness))
            {
                agentBehaviour.SetGoal<ChasePlayerAwayGoalM>(true);
            }
            // Stalk player if within stalk range and conditions fit
            else if (distanceToPlayer <= config.stalkActionRange &&
                     monsterAggression < config.stalkMaxAgressionLevel &&
                     playerAwareness >= config.stalkMinPlayerAwareness &&
                     playerAwareness <= config.stalkMaxPlayerAwareness)
            {
                agentBehaviour.SetGoal<StalkGoal>(true);
            }
            // Default to wandering if none of the conditions are met
            else
            {
                agentBehaviour.SetGoal<WanderGoalM>(true);
            }
        }
    }
}

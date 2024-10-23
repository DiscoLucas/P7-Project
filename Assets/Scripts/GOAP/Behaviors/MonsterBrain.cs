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
            Debug.Log("Player detected near the monster, switching to ChasePlayerAwayGoalM.");
            agentBehaviour.SetGoal<ChasePlayerAwayGoalM>(true);
        }

        private void ps_OnExit(Vector3 lastKnownPosition)
        {
            Debug.Log("Player exited monster's range, switching back to WanderGoalM.");
            agentBehaviour.SetGoal<WanderGoalM>(true);
        }

        private void protectionSensor_OnEnter(Transform player)
        {
            Debug.Log("Player detected near the protection area, switching to ChasePlayerAwayGoalM.");
            agentBehaviour.SetGoal<ChasePlayerAwayGoalM>(true);
            
            
        }

        private void protectionSensor_OnExit(Vector3 lastKnownPosition)
        {
            Debug.Log("Player exited the protection area, switching back to WanderGoalM.");
            agentBehaviour.SetGoal<WanderGoalM>(true);
        }
    }
}


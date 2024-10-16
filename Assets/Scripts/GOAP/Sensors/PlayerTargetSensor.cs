using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace Assets.Scripts.GOAP.Sensors
{
    public class PlayerTargetSensor : LocalTargetSensorBase, IInjectableObj
    {
        private MonsterConfig config;
        public Transform player;              // Player's transform
        public Transform protectionObj;       // The protection point
        private Collider[] colliders = new Collider[1]; // For detecting player within agent's radius

        public override void Created()
        {
            // Initialize if needed
        }

        public void Inject(DependencyInjector injector)
        {
            config = injector.config1 as MonsterConfig;
            player = injector.player;
            protectionObj = injector.protectArea;
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            // Detect the player near the protection point
            float distanceToProtection = Vector3.Distance(player.position, protectionObj.position);
            Debug.Log("Distance to Protection: " + distanceToProtection);

            if (distanceToProtection <= config.protectionAreaRadius)
            {
                // Player is within protection area, switch to chase action
                Debug.Log("Player within protection area. Target acquired.");
                return new TransformTarget(player);
            }

            // Detect if the player is within the agent's general detection radius
            if (Physics.OverlapSphereNonAlloc(agent.transform.position, config.AgentSensorRadius, colliders, config.playerLayerMask) > 0)
            {
                // Player is within sensor range
                return new TransformTarget(colliders[0].transform);
            }

            // No target detected
            return null;
        }

        public override void Update()
        {
            // Update logic if necessary
        }
    }
}

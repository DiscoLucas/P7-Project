using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Assets.Scripts.GOAP.WorldKeys;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Assets.Scripts.GOAP.Sensors
{
    public class PlayerTargetSensor : LocalTargetSensorBase, IInjectableObj
    {
        private MonsterConfig config;
        public Transform player;              // Player's transform
        public Transform protectionObj;       // The protection point
        private Collider[] colliders = new Collider[1]; // For detecting player within agent's radius
        private float distanceToLastKnownPosition;
        public Transform lastKnownPositionTransform;

        public override void Created() { }

        public void Inject(DependencyInjector injector)
        {
            config = injector.config1 as MonsterConfig;
            player = injector.player;
            lastKnownPositionTransform = injector.lkppTransform;
            protectionObj = injector.protectArea;
            Debug.Log("Injected Player: " + player.transform.gameObject.name);
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            if (player == null || protectionObj == null)
            {
                Debug.LogError("Player or protection object is null!");
                return null;
            }

            float distanceToProtection = Vector3.Distance(player.position, protectionObj.position);

            if (distanceToProtection <= config.protectionAreaRadius)
            {
                Debug.Log("Player within protection area. Target acquired at " + player.transform.position);
                lastKnownPositionTransform.position = getPostionOnMesh(player.transform.position);
                return new TransformTarget(lastKnownPositionTransform); // This should return a valid target.
            }
            else if (Physics.OverlapSphereNonAlloc(agent.transform.position, config.AgentSensorRadius, colliders, config.playerLayerMask) > 0)
            {
                Debug.Log("Player near monster at: " + colliders[0].transform.position);
                lastKnownPositionTransform.position = getPostionOnMesh(colliders[0].transform.position);
                return new TransformTarget(lastKnownPositionTransform); // This should also return a valid target.
            }
            else
            {
                Debug.Log("Player not found. Distance to protection: " + distanceToProtection);
                return new TransformTarget(lastKnownPositionTransform);
            }
        }

        public Vector3 getPostionOnMesh(Vector3 pos)
        {
            Debug.LogError("The postion of the player: " + pos);
            if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 5, NavMesh.AllAreas)) // Adjust radius if needed
            {
                Debug.Log($"Sampled Position on NavMesh: {hit.position}, Original Player Position: {pos}");
                return hit.position;
            }
            else
            {
                Debug.LogWarning($"NavMesh.SamplePosition failed, using player's original position: {pos}");
                return pos; // Fallback to player's position if sampling fails
            }
        }

        public override void Update() { }
    }

}

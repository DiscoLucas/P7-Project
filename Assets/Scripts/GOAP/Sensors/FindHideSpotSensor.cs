using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Assets.Scripts.GOAP.WorldKeys;

namespace Assets.Scripts.GOAP.Sensors
{
    public class FindHideSpotSensor : LocalTargetSensorBase, IInjectableObj
    {
        protected MonsterConfig config;

        public override void Created()
        {
        }

        public void Inject(DependencyInjector injector)
        {
            config = injector.config1;
        }

        protected int tries = 5;

        protected Vector3? FindHidingSpot(Vector3 playerPosition, Transform playerCamera, float hideRange, IMonoAgent agent)
        {
            Vector3? hidespot = null;

            for (int i = 0; i < tries; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * hideRange;
                randomDirection.y = 0; // Keep it horizontal
                Vector3 potentialSpot = playerPosition + randomDirection;
                Vector3 directionToSpot = potentialSpot - playerCamera.position;

                RaycastHit hit;
                if (Physics.Raycast(playerCamera.position, directionToSpot.normalized, out hit, hideRange))
                {
                    if (Vector3.Distance(hit.point, potentialSpot) < 0.1f && hit.collider.gameObject != agent.gameObject)
                    {
                        hidespot = potentialSpot;
                        break;
                    }
                }
            }

            return hidespot;
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            PlayerSensor playerSensor = references.GetComponentInChildren<PlayerSensor>();
            Vector3 playerPosition = playerSensor.playersRealPostion.position;
            float hideRange = config.hideRange;
            var playerCamera = references.GetComponentInChildren<PlayerAwarenessSensor>().playerCamera.transform;

            Vector3? hidespot = FindHidingSpot(playerPosition, playerCamera, hideRange ,agent);

            if (!hidespot.HasValue)
            {
                hidespot = playerPosition - playerCamera.forward * hideRange; // Fallback
            }

            playerSensor.hidespot.position = hidespot.Value;
            return new TransformTarget(playerSensor.hidespot);
        }

        public override void Update()
        {
        }
    }
}

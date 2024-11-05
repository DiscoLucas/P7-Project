using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Assets.Scripts.GOAP.WorldKeys;

namespace Assets.Scripts.GOAP.Sensors
{
    public class FindPeakSpotSensor : FindHideSpotSensor
    {
        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            PlayerSensor playerSensor = references.GetComponentInChildren<PlayerSensor>();
            Vector3 playerPosition = playerSensor.playersRealPostion.position;
            float peekRange = config.hideRange;
            var playerCamera = references.GetComponentInChildren<PlayerAwarenessSensor>().playerCamera.transform;

            Vector3? peekingSpot = FindPeekingSpot(playerPosition, playerCamera, peekRange);

            if (!peekingSpot.HasValue)
            {
                peekingSpot = playerPosition + playerCamera.forward * peekRange;
            }

            playerSensor.peakSpot.position = peekingSpot.Value; 
            return new TransformTarget(playerSensor.peakSpot);
        }

        protected Vector3? FindPeekingSpot(Vector3 playerPosition, Transform playerCamera, float peekRange)
        {
            Vector3? peekingSpot = null;

            for (int i = 0; i < tries; i++)
            {
                Vector3 directionToPlayer = playerCamera.forward * -1; 
                Vector3 offsetDirection = (Random.insideUnitSphere + directionToPlayer).normalized;
                Vector3 potentialSpot = playerPosition + offsetDirection * peekRange;

                RaycastHit hit;
                if (IsPositionVisible(potentialSpot, playerCamera, peekRange, out hit))
                {
                    if (hit.distance >= peekRange) 
                    {
                        peekingSpot = potentialSpot;
                        break;
                    }
                }
            }

            return peekingSpot;
        }

        private bool IsPositionVisible(Vector3 position, Transform playerCamera, float visibilityRange, out RaycastHit hit)
        {
            Vector3 directionToPosition = position - playerCamera.position;
            if (Physics.Raycast(playerCamera.position, directionToPosition.normalized, out hit, visibilityRange))
            {
                return Vector3.Distance(hit.point, position) < 0.1f;
            }
            return false;
        }
    }
}


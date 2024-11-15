using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Assets.Scripts.GOAP.WorldKeys;
using UnityEngine.AI;
using UnityEngine.UIElements;
using System;

namespace Assets.Scripts.GOAP.Sensors
{
    public class PlayerTargetSensor : LocalTargetSensorBase, IInjectableObj
    {
        private MonsterConfig config;
        public Transform player;          
        public Transform protectionObj;  
        private Collider[] colliders = new Collider[1]; 
        private float distanceToLastKnownPosition;
        public Transform lastKnownPositionTransform;

        public override void Created() { }

        public void Inject(DependencyInjector injector)
        {
            config = injector.config1 as MonsterConfig;
            lastKnownPositionTransform = injector.lkppTransform;
            protectionObj = injector.protectArea;
        }
        bool checkIfComponentAreThere() {
            return (player == null || protectionObj == null);
        }
        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            
            if (checkIfComponentAreThere())
            {
                if (GameManager.Instance != null) {
                    try
                    {
                        player = GameManager.Instance.playerObject.transform;
                        protectionObj = GameManager.Instance.protectionAreaObject.transform;
                    }
                    catch (Exception e) {
                        Debug.LogError(e.ToString());
                    }
                    
                }
                
                if(checkIfComponentAreThere())
                    return null;
            }

            float distanceToProtection = Vector3.Distance(player.position, protectionObj.position);

            if (distanceToProtection <= config.protectionAreaRadius)
            {
                lastKnownPositionTransform.position = getPostionOnMesh(player.transform.position, agent.transform.position);
                return new TransformTarget(lastKnownPositionTransform); 
            }
            else if (Physics.OverlapSphereNonAlloc(agent.transform.position, config.AgentSensorRadius, colliders, config.playerLayerMask) > 0)
            {
                lastKnownPositionTransform.position = getPostionOnMesh(colliders[0].transform.position, agent.transform.position);
                return new TransformTarget(lastKnownPositionTransform);
            }
            else
            {
                return new TransformTarget(lastKnownPositionTransform);
            }
        }

        public Vector3 getPostionOnMesh(Vector3 pos, Vector3 agentPos)
        {
            pos = new Vector3(pos.x, agentPos.y, pos.z);
            if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 5, NavMesh.AllAreas))
            {
                return hit.position;
            }
            else
            {
                return pos; 
            }
        }

        public override void Update() { }
    }

}

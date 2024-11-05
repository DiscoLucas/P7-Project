﻿using Assets.Scripts.GOAP.Actions;
using Assets.Scripts.GOAP.Goals;
using Assets.Scripts.GOAP.Sensors;
using CrashKonijn.Goap.Behaviours;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GOAP.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class MonsterBrain : MonoBehaviour
    {
        private AgentBehaviour agentBehaviour;
        public PlayerSensor p_sensor;
        public ProtectionAreaSensor protectionSensor;
        public AgressionSensor monsterAggressionLevelSensor;
        public PlayerAwarenessSensor awarenessSensor;
        public MonsterConfig config;
        public DependencyInjector dependencyInjector;
        public BotState currentStat;

        private bool playerSpotted = false; // Flag to track if the player has been spotted

        private void Awake()
        {
            agentBehaviour = GetComponent<AgentBehaviour>();
        }

        void Start()
        {
            dependencyInjector.brain = this;
            // Start with the wander goal
            agentBehaviour.SetGoal<WanderGoalM>(true);
            config = dependencyInjector.config1;
            p_sensor.Collider.radius = config.AgentSensorRadius;
            p_sensor.playerPositionMapTracker.playerPostionsSummeries.AddListener(playerPostionSmelled);

            if (protectionSensor != null)
            {
                protectionSensor.Inject(dependencyInjector);
            }

        }

        private void playerPostionSmelled(Vector3 arg0, float arg1)
        {
            EvaluateGoal(p_sensor.playerLastPos);
        }

        private void OnEnable()
        {

            if (protectionSensor != null)
            {
                protectionSensor.OnPlayerEnter += protectionSensor_OnEnter;
                protectionSensor.OnPlayerExit += protectionSensor_OnExit;
            }

            awarenessSensor.playerSpottede.AddListener(OnPlayerSpotted);

        }

        private void OnDisable()
        {

            if (protectionSensor != null)
            {
                protectionSensor.OnPlayerEnter -= protectionSensor_OnEnter;
                protectionSensor.OnPlayerExit -= protectionSensor_OnExit;
            }


            awarenessSensor.playerSpottede.RemoveListener(OnPlayerSpotted);
            
        }

        // Called when the player is spotted or lost from sight, with a boolean value
        private void OnPlayerSpotted(bool isSpotted)
        {
            playerSpotted = isSpotted;
            if (playerSpotted)
            {

                EvaluateGoal(p_sensor.playerLastPos);
            }
        }

        private void ps_OnEnter(Transform player)
        {
            EvaluateGoal(player);
        }

        private void ps_OnExit(Vector3 lastKnownPosition)
        {
            EvaluateGoal(p_sensor.playerLastPos);
        }

        private void protectionSensor_OnEnter(Transform player)
        {
            EvaluateGoal(player);
        }

        private void protectionSensor_OnExit(Vector3 lastKnownPosition)
        {
        }


        public void EvaluateGoal() {
            EvaluateGoal(p_sensor.playerLastPos);
        }

        private void EvaluateGoal(Transform player)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float playerAwareness = awarenessSensor.GetPlayerAwarenessLevel(); // Player awareness level from PlayerAwarenessSensor
            float monsterAggression = monsterAggressionLevelSensor.aggressionLevel;
            
     
            if (playerSpotted && monsterAggression > config.agressionLevelBeginChase)
             {
                changeState(BotState.CHASE);
             }
             else if(playerSpotted)
             {
                changeState(BotState.STALK);

            }
             else
             {
                changeState(BotState.IDLE);
            }

        }

        void changeState(BotState state)
        {
            if (state != currentStat) {
                agentBehaviour.EndAction();
                currentStat = state;
                if (state == BotState.CHASE)
                {
                    agentBehaviour.SetGoal<HurtPlayerGoal>(true);
                }
                else if (state == BotState.STALK)
                {
                    agentBehaviour.SetGoal<StalkGoal>(true);
                }
                else {
                    agentBehaviour.SetGoal<WanderGoalM>(false);
                }

                Debug.Log("Current Goal Change: " + state.ToString());
                currentStat = state;
                agentBehaviour.Run();
            }

        }
    }
}

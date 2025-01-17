﻿using System.Collections;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using JSAM;
using Assets.Scripts.GOAP.Sensors;

namespace Assets.Scripts.GOAP.Actions
{
    public class ScreamAction : ActionBase<CommonDataM>, IInjectableObj
    {
        private MonsterConfig config;

        public override void Created() { }

        public override void Start(IMonoAgent agent, CommonDataM data)
        {
            agent.GetComponent<AgentMoveBehavior>().stopMoving();
            agent.GetComponentInChildren<AnimationBehaviors>().startScreamAction();
        }

        public override ActionRunState Perform(IMonoAgent agent, CommonDataM data, ActionContext context)
        {
            if (agent.GetComponent<AgentSoundBehaviors>().checkIfScreamStopped()) {
                return ActionRunState.Stop;
            }
            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, CommonDataM data) { }

        public void Inject(DependencyInjector injector)
        {
            config = injector.config1 as MonsterConfig;
        }

        public override float GetCost(IMonoAgent agent, IComponentReference references)
        {
            PlayerSensor playerSensor = references.GetCachedComponentInChildren<PlayerSensor>();
            PlayerAwarenessSensor awarenessSensor = references.GetCachedComponentInChildren<PlayerAwarenessSensor>();
            float timeCost = agent.GetComponent<AgentSoundBehaviors>().getLastScreamTimeCost();
            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerSensor.playersRealPostion.position);
            float playerAwareness = awarenessSensor.playerAwarenessLevel;
            float awarenessMidpoint = (config.stalkMaxPlayerAwareness + config.stalkMinPlayerAwareness) / 2f;
            float awarenessMultiplier = playerAwareness < awarenessMidpoint ? config.lowAwarenessCostMultiplier : config.highAwarenessCostMultiplier;
            float distanceFactor = Mathf.InverseLerp(config.screamActionCostRange.x, config.screamActionCostRange.y, distanceToPlayer);
            float lerp = distanceFactor / 2 + timeCost / 2;
            return Mathf.Lerp(config.screamActionCostRange.x, config.screamActionCostRange.y, lerp) * awarenessMultiplier;
        }
    }


    public class ScreamData : IActionData
    {
        public TransformTarget Target { get; set; }
        ITarget IActionData.Target
        {
            get => Target;
            set => Target = value as TransformTarget;
        }
    }
}

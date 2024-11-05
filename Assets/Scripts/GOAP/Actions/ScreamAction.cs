using System.Collections;
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
            agent.GetComponent<AgentMoveBehavior>().stopMoveing();
        }

        public override ActionRunState Perform(IMonoAgent agent, CommonDataM data, ActionContext context)
        {
            Debug.Log("Screaming");
            return ActionRunState.Stop;
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
            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerSensor.playersRealPostion.position);
            float playerAwareness = awarenessSensor.playerAwarenessLevel;
            float awarenessMidpoint = (config.stalkMaxPlayerAwareness + config.stalkMinPlayerAwareness) / 2f;
            float awarenessMultiplier = playerAwareness < awarenessMidpoint ? config.lowAwarenessCostMultiplier : config.highAwarenessCostMultiplier;
            float distanceFactor = Mathf.InverseLerp(config.screamActionCostRange.x, config.screamActionCostRange.y, distanceToPlayer);
            return Mathf.Lerp(config.screamActionCostRange.x, config.screamActionCostRange.y, distanceFactor) * awarenessMultiplier;
        }
    }


    public class ScreamData : IActionData
    {
        public TransformTarget Target { get; set; }
        ITarget IActionData.Target
        {
            get => Target;
            set => Target = value as TransformTarget; // Ensure the target is set correctly
        }
    }
}

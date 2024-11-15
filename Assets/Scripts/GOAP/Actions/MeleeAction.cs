using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Classes;
using Unity.VisualScripting;

namespace Assets.Scripts.GOAP.Actions
{
    public class MeleeAction : ActionBase<AttackData>, IInjectableObj
    {
        private MonsterConfig config;
        public override void Created()
        {

        }
        public override void End(IMonoAgent agent, AttackData data)
        {
           
        }

        public void Inject(DependencyInjector injector)
        {
            config = injector.config1 as MonsterConfig;
        }

        public override ActionRunState Perform(IMonoAgent agent, AttackData data, ActionContext context)
        {
            if(agent.GetComponentInChildren<AnimationBehaviors>().canEndAttack())
                return ActionRunState.Stop;

            return ActionRunState.Continue;

        }

        public override void Start(IMonoAgent agent, AttackData data)
        {
            agent.GetComponent<AgentMoveBehavior>().stopMoving();
            data.timer = config.attackDelay;
            agent.GetComponentInChildren<AnimationBehaviors>().startAttack();
        }
    }
}
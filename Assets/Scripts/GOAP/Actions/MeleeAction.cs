using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Classes;

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
           //Stop attacking
        }

        public void Inject(DependencyInjector injector)
        {
            config = injector.config1 as MonsterConfig;
        }

        public override ActionRunState Perform(IMonoAgent agent, AttackData data, ActionContext context)
        {
            Debug.LogError("Attacking the player!");
            // Add logic to deal damage
            return ActionRunState.Stop; ;

        }

        public override void Start(IMonoAgent agent, AttackData data)
        {
            data.timer = config.attackDelay;
        }
    }
}
using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;

namespace Assets.Scripts.GOAP
{
    public class DependencyInjector : GoapConfigInitializerBase, IGoapInjector
    {
        public MonsterConfig config1;
        public Transform player;
        public Transform protectArea;
        /// <summary>
        /// 
        /// </summary>
        public Transform lkppTransform;

        public override void InitConfig(GoapConfig config)
        {
            config.GoapInjector= this;
        }

        public void Inject(IActionBase action)
        {
            if (action is IInjectableObj injectable) {
                injectable.Inject(this);
            }
        }

        public void Inject(IGoalBase goal)
        {
            if (goal is IInjectableObj injectable)
            {
                injectable.Inject(this);
            }
        }

        public void Inject(IWorldSensor worldSensor)
        {
            if (worldSensor is IInjectableObj injectable)
            {
                injectable.Inject(this);
            }
        }

        public void Inject(ITargetSensor targetSensor)
        {
            if (targetSensor is IInjectableObj injectable)
            {
                injectable.Inject(this);
            }
        }
    }
}
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Sensors
{
    public class PlayerAwarenessLevelSensor : LocalWorldSensorBase, IInjectableObj
    {
        MonsterConfig config;
        public override void Created()
        {

        }

        public void Inject(DependencyInjector injector)
        {
            config = injector.config1;
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            int paLevel = references.GetCachedComponentInChildren<PlayerAwarenessSensor>().GetPlayerAwarenessLevel();
          
            return new SenseValue(paLevel);

        }

        public override void Update()
        {

        }

    }
}
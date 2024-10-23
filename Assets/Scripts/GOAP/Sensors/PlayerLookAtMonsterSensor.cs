using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Sensors
{
    public class PlayerLookAtMonsterSensor : LocalWorldSensorBase, IInjectableObj
    {

        public override void Created()
        {

        }

        public void Inject(DependencyInjector injector)
        {

        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            return new SenseValue();
        }

        public override void Update()
        {

        }
    }
}
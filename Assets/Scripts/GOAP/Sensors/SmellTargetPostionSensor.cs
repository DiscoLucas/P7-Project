using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Assets.Scripts.GOAP.WorldKeys;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Assets.Scripts.GOAP.Sensors
{
    public class SmellTargetPostionSensor : LocalTargetSensorBase, IInjectableObj
    {
        public override void Created()
        {
 
        }

        public void Inject(DependencyInjector injector)
        {

        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            return new TransformTarget(references.GetComponentInChildren<PlayerSensor>().getSentSmelledPoint());
        }

        public override void Update()
        {

        }
    }
}
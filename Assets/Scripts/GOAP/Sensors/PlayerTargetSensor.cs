using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Sensors;
using CrashKonijn.Goap.Interfaces;

namespace Assets.Scripts.GOAP.Sensors
{
    public class PlayerTargetSensor : LocalTargetSensorBase
    {
        public override void Created()
        {

        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            return null;
            /*if (Physics.OverlapSphereNonAlloc(agent.transform.position,)) {
            
            }*/
        }

        public override void Update()
        {

        }
    }
}
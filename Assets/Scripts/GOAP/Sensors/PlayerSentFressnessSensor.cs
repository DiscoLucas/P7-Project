using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Sensors
{
    internal class PlayerSentFressnessSensor : LocalWorldSensorBase
    {
        public override void Created()
        {

        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            return new SenseValue((int)references.GetComponentInChildren<PlayerSensor>().getSentFreshness());
        }

        public override void Update()
        {
        }
    }
}

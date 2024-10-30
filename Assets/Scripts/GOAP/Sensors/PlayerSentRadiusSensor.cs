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
    internal class PlayerSentRadiusSensor: LocalWorldSensorBase
    {

        public override void Created()
        {
            
        }
        public override void Update() { }
        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            return new SenseValue(references.GetComponentInChildren<AgressionSensor>().aggressionLevel);
        }
    }
}

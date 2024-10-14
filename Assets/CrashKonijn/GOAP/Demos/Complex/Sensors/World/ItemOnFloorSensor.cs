using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Sensors;
using Demos.Complex.Behaviours;
using UnityEngine;

namespace Demos.Complex.Sensors.World
{
    public class ItemOnFloorSensor : GlobalWorldSensorBase
    {
        private ItemCollection collection;

        public override void Created()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            this.collection = GameObject.FindObjectOfType<ItemCollection>();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public override SenseValue Sense()
        {
            return this.collection.Count(false, false);
        }
    }
}
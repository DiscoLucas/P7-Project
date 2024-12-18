﻿using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Sensors
{
    public class PlayerDistanceSensor : LocalWorldSensorBase, IInjectableObj
    {
        public override void Created()
        {

        }

        public void Inject(DependencyInjector injector)
        {

        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            return new SenseValue(references.GetCachedComponentInChildren<PlayerSensor>().ditanceToPlayer());

        }

        public override void Update()
        {

        }

    }
}
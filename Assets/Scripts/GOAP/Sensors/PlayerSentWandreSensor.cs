using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Sensors;
using CrashKonijn.Goap.Interfaces;
using System;
using UnityEngine.AI;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Sensors;
using Assets.Scripts.GOAP.Sensors;
using Assets.Scripts.GOAP;

public class PlayerSentWandreSensor : LocalTargetSensorBase, IInjectableObj
{
    MonsterConfig config;
    public override void Created() { }

    public override void Update() { }

    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        Vector3 smellArea = references.GetCachedComponentInChildren<PlayerSensor>().getSentSmelledPoint().position;
        float radius = references.GetCachedComponentInChildren<PlayerSensor>().getSentSmelledDistance() * config.smelledSearchAreaMultiplyer;
        Vector3 position = getAreaToSearch(smellArea, radius);

        return new PositionTarget(position);
    }

    private Vector3 getAreaToSearch(Vector3 pos, float radius)
    {
        int count = 0;

        while (count < 5)
        {
            Vector2 random = UnityEngine.Random.insideUnitCircle * radius;
            Vector3 position = pos + new Vector3(
                random.x,
                0,
                random.y
            );
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 1, NavMesh.AllAreas))
            {
                return hit.position;
            }

            count++;
        }

        return pos;
    }

    public void Inject(DependencyInjector injector)
    {
        config = injector.config1;
    }
}

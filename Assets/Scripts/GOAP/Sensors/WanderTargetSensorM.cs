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

public class WanderTargetSensorM : LocalTargetSensorBase, IInjectableObj
{
    MonsterConfig config;
    public override void Created() { }

    public override void Update() { }

    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        Vector3 position = GetRandomPosition(agent);

        return new PositionTarget(position);
    }

    private Vector3 GetRandomPosition(IMonoAgent agent)
    {
        int count = 0;

        Vector3 pos = new Vector3(agent.transform.position.x, agent.transform.position.y, agent.transform.position.z);
        float dist = config.wanderingSetinRange;

        while (count < 5)
        {
            Vector2 random = UnityEngine.Random.insideUnitCircle * dist;
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

        return agent.transform.position;
    }

    public void Inject(DependencyInjector injector)
    {
        config = injector.config1;
    }
}

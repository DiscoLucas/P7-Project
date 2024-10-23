using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Sensors;
using CrashKonijn.Goap.Interfaces;
using System;
using UnityEngine.AI;
using CrashKonijn.Goap.Classes;

public class WanderTargetSensorM : LocalTargetSensorBase
{
    public float wandreradius = 10;
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

        while (count < 5)
        {
            Vector2 random = UnityEngine.Random.insideUnitCircle * 10;
            Vector3 position = agent.transform.position + new Vector3(
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

}

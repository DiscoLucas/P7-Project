using UnityEngine;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

public class WanderTargetSensor : LocalTargetSensorBase
{
    public override void Created()
    {
        // This is called when the sensor is created.
    }

    // Called every frame. Can be used to gather data from the world before the sense method is called.
    // This is useful for gathering 'base data' that is common for all agents.
    public override void Update()
    {
        
    }

    // called when the sensor needs to sense a target for a specific agent.
    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        var random = this.GetRandomPosition(agent);

        return new PositionTarget(random);
    }

    private Vector3 GetRandomPosition(IMonoAgent agent)
    {
        var random = Random.insideUnitCircle * 5f;
        var position = agent.transform.position + new Vector3(random.x, 0, random.y);
        return position;
    }
}

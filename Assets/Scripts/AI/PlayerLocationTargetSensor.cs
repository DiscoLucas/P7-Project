using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using CrashKonijn.Goap.Classes;
using UnityEngine;

public class PlayerLocationTargetSensor : LocalTargetSensorBase
{
    public override void Created()
    {

    }

    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        // Get the player's position
        var player = GameObject.FindWithTag("Player");
        return new PositionTarget(player.transform.position);
    }

    public override void Update()
    {

    }
}

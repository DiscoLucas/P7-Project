using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Classes.Builders;
using System;
using CrashKonijn.Goap.Enums;

public class GoapConfigFactory : GoapSetFactoryBase
{
    public int targetPosStop = 1;
    public int wanderingBaseCost = 5;
    public float wanderingSetinRange = 10;
    public override IGoapSetConfig Create()
    {
        GoapSetBuilder builder = new("Monsterset");
        buildGoals(builder);
        buildActions(builder);
        buildSensors(builder);
        return builder.Build();
    }

    private void buildSensors(GoapSetBuilder builder)
    {
        builder.AddGoal<WanderGoalM>().AddCondition<WanderingWK>(CrashKonijn.Goap.Resolver.Comparison.GreaterThanOrEqual, targetPosStop);
    }

    private void buildActions(GoapSetBuilder builder)
    {
        builder.AddAction<WanderActionM>()
            .SetTarget<WanderTargetM>()
            .AddEffect<WanderingWK>(EffectType.Increase)
            .SetBaseCost(wanderingBaseCost)
            .SetInRange(wanderingSetinRange);
    }

    private void buildGoals(GoapSetBuilder builder)
    {
        builder.AddTargetSensor<WanderTargetSensorM>().SetTarget<WanderTargetM>();
    }
}

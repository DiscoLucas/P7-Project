using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Classes.Builders;
using System;
using CrashKonijn.Goap.Enums;
using Assets.Scripts.GOAP.Goals;
using Assets.Scripts.GOAP.Targets;
using CrashKonijn.Goap.Resolver;
using Assets.Scripts.GOAP.Actions;
using Assets.Scripts.GOAP;
using Unity.VisualScripting;
using Assets.Scripts.GOAP.WorldKeys;
using Assets.Scripts.GOAP.Sensors;

[RequireComponent(typeof(DependencyInjector))]
public class GoapConfigFactory : GoapSetFactoryBase
{
    private DependencyInjector injector;

    public override IGoapSetConfig Create()
    {
        injector = GetComponent<DependencyInjector>();
        GoapSetBuilder builder = new("Monsterset");
        buildGoals(builder);
        buildActions(builder);
        buildSensors(builder);
        return builder.Build();
    }

    private void buildSensors(GoapSetBuilder builder)
    {
        builder.AddGoal<WanderGoalM>().AddCondition<WanderingWK>(CrashKonijn.Goap.Resolver.Comparison.GreaterThanOrEqual, injector.config1.targetPosStop);
        builder.AddGoal<ChasePlayerAwayGoalM>().AddCondition<PlayerPosition>(CrashKonijn.Goap.Resolver.Comparison.GreaterThanOrEqual, 5);
    }

    private void buildActions(GoapSetBuilder builder)
    {
        builder.AddAction<WanderActionM>()
            .SetTarget<WanderTargetM>()
            .AddEffect<WanderingWK>(EffectType.Increase)
            .SetBaseCost(injector.config1.wanderingBaseCost)
            .SetInRange(injector.config1.wanderingSetinRange);

        builder.AddAction<MeleeAction>()
            .SetTarget<PlayerTarget>()
            .AddEffect<PlayerPosition>(EffectType.Increase)
            .SetBaseCost((int)injector.config1.meleeCost)
            .SetInRange(injector.config1.AgentSensorRadius);

        // Add new actions here
        builder.AddAction<StalkAction>()
            .SetTarget<PlayerTarget>()
            .SetBaseCost(injector.config1.stalkActionCost);

        builder.AddAction<ScreamAction>()
            .SetTarget<PlayerTarget>() 
            .SetBaseCost(injector.config1.screamActionCost);
    }

    private void buildGoals(GoapSetBuilder builder)
    {
        builder.AddTargetSensor<WanderTargetSensorM>().SetTarget<WanderTargetM>();
        builder.AddTargetSensor<PlayerTargetSensor>().SetTarget<PlayerTarget>();
    }
}


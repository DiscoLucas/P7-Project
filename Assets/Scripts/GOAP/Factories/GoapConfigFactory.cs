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
using Assets.Scripts.GOAP.Actions.Data;

[RequireComponent(typeof(DependencyInjector))]
public class GoapConfigFactory : GoapSetFactoryBase
{
    private DependencyInjector injector;

    public override IGoapSetConfig Create()
    {
        injector = GetComponent<DependencyInjector>();
        GoapSetBuilder builder = new("Monsterset");

        // Build only the basic goals and actions for now
        buildGoals(builder);
        buildActions(builder);
        buildSensors(builder);
        return builder.Build();
    }

    private void buildSensors(GoapSetBuilder builder)
    {
        // Basic sensors for wandering and chasing
        builder.AddTargetSensor<WanderTargetSensorM>()
            .SetTarget<WanderingTargetPositionTK>();

        builder.AddTargetSensor<PlayerTargetSensor>()
            .SetTarget<ChaseTargetPositionTK>();

        builder.AddWorldSensor<AggressionLevelSensor>() 
            .SetKey<MonsterAggressionLevelWK>();

        builder.AddWorldSensor<PlayerDistanceSensor>()
            .SetKey<PlayerDistanceWK>();

        builder.AddWorldSensor<PlayerAwarenessLevelSensor>()
            .SetKey<PlayerAwarenessWK>();

        builder.AddTargetSensor<SmellTargetPostionSensor>()
            .SetTarget<SmellTargetPostionTK>();

        builder.AddWorldSensor<PlayerSentFressnessSensor>()
            .SetKey<SmelledFressnesWK>();

        builder.AddWorldSensor<PlayerSentRadiusSensor>()
            .SetKey<PlayerSmelledRadius>();

        builder.AddTargetSensor<FindHideSpotSensor>()
            .SetTarget<HideTargetPositionTK>();

        builder.AddTargetSensor<FindPeakSpotSensor>()
            .SetTarget<PeekAtPlayerPositionTK>();



    }

    private void buildActions(GoapSetBuilder builder)
    {

        //Wandre action
        builder.AddAction<WanderActionM>()
             .SetTarget<WanderingTargetPositionTK>()
             .AddEffect<WanderingStateWK>(EffectType.Increase)
             .SetBaseCost(injector.config1.wanderingBaseCost)
             .SetInRange(injector.config1.wanderingSetinRange);

         builder.AddAction<WandreNearPlayersSmellAction>()
             .SetTarget<SmellTargetPostionTK>()
             .AddEffect<PlayerDistanceWK>(EffectType.Decrease)
             .AddEffect<WanderingStateWK>(EffectType.Increase)
             .SetBaseCost(injector.config1.goToSentminCost)
             .AddCondition<SmelledFressnesWK>(CrashKonijn.Goap.Resolver.Comparison.SmallerThanOrEqual, injector.config1.smellFressness* 50);
        //Move actions
        builder.AddAction<GoToPlayerLastPosAction>()
          .SetTarget<ChaseTargetPositionTK>()
          .AddEffect<LastKnownPlayerPositionWK>(EffectType.Decrease)
          .AddEffect<PlayerDistanceWK>(EffectType.Decrease)
          .SetBaseCost(injector.config1.chaseCost)
          .SetInRange(injector.config1.chaseRange);

        //Hurt player
          builder.AddAction<MeleeAction>()
          .SetTarget<ChaseTargetPositionTK>()
          .AddEffect<playerHealthWK>(EffectType.Decrease)
          .AddCondition<PlayerDistanceWK>(CrashKonijn.Goap.Resolver.Comparison.SmallerThanOrEqual, (int)injector.config1.meleeRange)
          .SetBaseCost((int)injector.config1.meleeCost)
          .SetInRange(injector.config1.meleeRange);

        //lose attention
        builder.AddAction<HideFromPlayer>()
            .SetTarget<HideTargetPositionTK>()
            .AddEffect<PlayerAwarenessWK>(EffectType.Decrease)
            .AddCondition<PlayerDistanceWK>(CrashKonijn.Goap.Resolver.Comparison.SmallerThanOrEqual, injector.config1.stalkDitsanceMinDistance)
            .SetBaseCost(injector.config1.hideCost)
            .SetInRange(injector.config1.peekRange);

        //Get attention
        builder.AddAction<PeekAtPlayerAction>()
             .SetTarget<PeekAtPlayerPositionTK>()
             .AddEffect<PlayerAwarenessWK>(EffectType.Increase)
             .AddCondition<PlayerDistanceWK>(CrashKonijn.Goap.Resolver.Comparison.SmallerThanOrEqual, injector.config1.stalkDitsanceMinDistance)
             .SetBaseCost(injector.config1.peekCost)
             .SetInRange(injector.config1.peekRange);

        builder.AddAction<ScreamAction>()
             .AddEffect<PlayerAwarenessWK>(EffectType.Increase).SetBaseCost(injector.config1.screamActionCost)
             .SetBaseCost(injector.config1.screamActionCost)
             .AddCondition<PlayerDistanceWK>(CrashKonijn.Goap.Resolver.Comparison.SmallerThanOrEqual, injector.config1.screamHearingRange);
    }

    private void buildGoals(GoapSetBuilder builder)
    {

        builder.AddGoal<WanderGoalM>()
            .AddCondition<WanderingStateWK>(CrashKonijn.Goap.Resolver.Comparison.GreaterThanOrEqual, injector.config1.targetPosStop); 

        builder.AddGoal<HurtPlayerGoal>()
            .AddCondition<playerHealthWK>(CrashKonijn.Goap.Resolver.Comparison.SmallerThanOrEqual, 0);


        builder.AddGoal<StalkGoal>()
            .AddCondition<MonsterAggressionLevelWK>(CrashKonijn.Goap.Resolver.Comparison.GreaterThanOrEqual, injector.config1.stalkMaxAgressionLevel)
            .AddCondition<PlayerAwarenessWK>(CrashKonijn.Goap.Resolver.Comparison.GreaterThanOrEqual, injector.config1.stalkMinPlayerAwareness)
            .AddCondition<PlayerAwarenessWK>(CrashKonijn.Goap.Resolver.Comparison.SmallerThanOrEqual, injector.config1.stalkMaxPlayerAwareness);
    }

}


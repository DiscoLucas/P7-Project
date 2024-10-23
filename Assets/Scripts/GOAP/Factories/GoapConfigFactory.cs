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



    }

    private void buildActions(GoapSetBuilder builder)
    {
        // Action for melee attack
     /*   builder.AddAction<MeleeAction>()
            .SetTarget<ChaseTargetPositionTK>() // Now using chase target instead of PlayerTarget
            .AddEffect<PlayerDistanceWK>(EffectType.Decrease) // Decrease the distance to the player
            .AddEffect<MonsterAggressionLevelWK>(EffectType.Increase) // Increase the monster's aggression level
            .SetBaseCost((int)injector.config1.meleeCost)
            .SetInRange(injector.config1.AgentSensorRadius);

        // Action for stalking the player
        builder.AddAction<StalkAction>()
            .SetTarget<ChaseTargetPositionTK>() // Using chase target since it's relevant for stalking
            .AddEffect<PlayerDistanceWK>(EffectType.Decrease) // Decrease the distance to the player
            .AddEffect<MonsterAggressionLevelWK>(EffectType.Increase) // Increase the monster's aggression level
            .SetBaseCost(injector.config1.stalkActionCost)
            .SetInRange(injector.config1.stalkActionRange);

        // Action for screaming to scare the player
        builder.AddAction<ScreamAction>()
            .SetTarget<ScreamTargetPositionTK>() // Using scream-specific target
            .AddEffect<PlayerAwarenessWK>(EffectType.Decrease) // Decrease player's awareness of the monster
            .AddEffect<MonsterAggressionLevelWK>(EffectType.Increase) // Increase the monster's aggression level after screaming
            .SetBaseCost(injector.config1.screamActionCost)
            .SetInRange(injector.config1.screamActionRange);

        // Action to go out of sight of the player
        builder.AddAction<GoOutOfSightAction>()
            .SetTarget<HideTargetPositionTK>() // Use hiding target key
            .AddEffect<PlayerAwarenessWK>(EffectType.Decrease) // Decrease the player's awareness
            .AddEffect<HideSpotAvailableWK>(EffectType.Decrease) // Optionally indicate that the hiding spot is not available
            .SetBaseCost(injector.config1.hideCost)
            .SetInRange(injector.config1.hideRange);

        // Action for peeking at the player
        builder.AddAction<PeekAtPlayerAction>()
            .SetTarget<PeekAtPlayerPositionTK>() // Using peeking target
            .AddEffect<PlayerAwarenessWK>(EffectType.Decrease) // Decrease player's awareness
            .AddEffect<MonsterAggressionLevelWK>(EffectType.Increase) // Increase monster's aggression level slightly
            .SetBaseCost(injector.config1.peekCost)
            .SetInRange(injector.config1.peekRange);*/

        builder.AddAction<WanderActionM>()
            .SetTarget<WanderingTargetPositionTK>()
            .AddEffect<WanderingStateWK>(EffectType.Increase)
            .SetBaseCost(injector.config1.wanderingBaseCost)
            .SetInRange(injector.config1.wanderingSetinRange);

        builder.AddAction<GoToPlayerLastPosAction>()
            .SetTarget<ChaseTargetPositionTK>()
            .AddEffect<LastKnownPlayerPositionWK>(EffectType.Decrease)
            .AddEffect<PlayerDistanceWK>(EffectType.Decrease)
            .AddEffect<PlayerAwarenessWK>(EffectType.Increase)
            .SetBaseCost(injector.config1.chaseCost)
            .SetInRange(injector.config1.chaseRange);
    }

    private void buildGoals(GoapSetBuilder builder)
    {
        // Wander goal
        builder.AddGoal<WanderGoalM>()
            .AddCondition<WanderingStateWK>(CrashKonijn.Goap.Resolver.Comparison.GreaterThanOrEqual, injector.config1.targetPosStop);

        // Chase player away goal - chase until near the last known position
        builder.AddGoal<ChasePlayerAwayGoalM>()
            .AddCondition<LastKnownPlayerPositionWK>(CrashKonijn.Goap.Resolver.Comparison.SmallerThanOrEqual, (int)injector.config1.meleeRange);
    }

}

/*public class GoapConfigFactory : GoapSetFactoryBase
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
        // Use your existing WanderTargetSensorM and PlayerTargetSensor
        builder.AddTargetSensor<WanderTargetSensorM>()
            .SetTarget<WanderingTargetPositionTK>(); // Linked to Wandering

        builder.AddTargetSensor<PlayerTargetSensor>()
            .SetTarget<ChaseTargetPositionTK>(); // Set to chase the player when detected

        /*  

          // Adding an additional sensor for hiding spots, leveraging HideTargetPositionTK
          builder.AddTargetSensor<PlayerTargetSensor>() // You can reuse this sensor if it fits
              .SetTarget<HideTargetPositionTK>(); // Hide from the player

          // Adding world sensors for specific game conditions
          builder.AddWorldSensor<PlayerLookAtMonsterSensor>() // For player awareness of monster
              .SetKey<PlayerAwarenessWK>(); // Monitor player's awareness

          builder.AddWorldSensor<MonsterAggressionLevelSensor>()
              .SetKey<MonsterAggressionLevelWK>(); // Track monster's aggression level

          builder.AddWorldSensor<PlayerDistanceSensor>() // Track distance between player and monster
              .SetKey<PlayerDistanceWK>(); // Control decisions based on distance

    }





    private void buildActions(GoapSetBuilder builder)
    {
        // Action for melee attack
        builder.AddAction<MeleeAction>()
            .SetTarget<ChaseTargetPositionTK>() // Now using chase target instead of PlayerTarget
            .AddEffect<PlayerDistanceWK>(EffectType.Decrease) // Decrease the distance to the player
            .AddEffect<MonsterAggressionLevelWK>(EffectType.Increase) // Increase the monster's aggression level
            .SetBaseCost((int)injector.config1.meleeCost)
            .SetInRange(injector.config1.AgentSensorRadius);

        // Action for stalking the player
        builder.AddAction<StalkAction>()
            .SetTarget<ChaseTargetPositionTK>() // Using chase target since it's relevant for stalking
            .AddEffect<PlayerDistanceWK>(EffectType.Decrease) // Decrease the distance to the player
            .AddEffect<MonsterAggressionLevelWK>(EffectType.Increase) // Increase the monster's aggression level
            .SetBaseCost(injector.config1.stalkActionCost)
            .SetInRange(injector.config1.stalkActionRange);

        // Action for screaming to scare the player
        builder.AddAction<ScreamAction>()
            .SetTarget<ScreamTargetPositionTK>() // Using scream-specific target
            .AddEffect<PlayerAwarenessWK>(EffectType.Decrease) // Decrease player's awareness of the monster
            .AddEffect<MonsterAggressionLevelWK>(EffectType.Increase) // Increase the monster's aggression level after screaming
            .SetBaseCost(injector.config1.screamActionCost)
            .SetInRange(injector.config1.screamActionRange);

        // Action to go out of sight of the player
        builder.AddAction<GoOutOfSightAction>()
            .SetTarget<HideTargetPositionTK>() // Use hiding target key
            .AddEffect<PlayerAwarenessWK>(EffectType.Decrease) // Decrease the player's awareness
            .AddEffect<HideSpotAvailableWK>(EffectType.Decrease) // Optionally indicate that the hiding spot is not available
            .SetBaseCost(injector.config1.hideCost)
            .SetInRange(injector.config1.hideRange);

        // Action for peeking at the player
        builder.AddAction<PeekAtPlayerAction>()
            .SetTarget<PeekAtPlayerPositionTK>() // Using peeking target
            .AddEffect<PlayerAwarenessWK>(EffectType.Decrease) // Decrease player's awareness
            .AddEffect<MonsterAggressionLevelWK>(EffectType.Increase) // Increase monster's aggression level slightly
            .SetBaseCost(injector.config1.peekCost)
            .SetInRange(injector.config1.peekRange);

        builder.AddAction<WanderActionM>()
        .SetTarget<WanderingTargetPositionTK>() // Set appropriate target key for wandering
        .SetBaseCost(injector.config1.wanderActionCost)
        .SetInRange(injector.config1.wanderActionRange);
    }


    private void buildGoals(GoapSetBuilder builder)
    {
        // Wander Goal
        builder.AddGoal<WanderGoalM>()
            .AddCondition<WanderingStateWK>(CrashKonijn.Goap.Resolver.Comparison.GreaterThanOrEqual, injector.config1.targetPosStop);

        // Chase Player Away Goal
        builder.AddGoal<ChasePlayerAwayGoalM>()
            .AddCondition<LastKnownPlayerPositionWK>(CrashKonijn.Goap.Resolver.Comparison.GreaterThan, 0);
        /*
            // Stalk Player Goal
            builder.AddGoal<StalkGoal>()
                .AddCondition<LastKnownPlayerPositionWK>(CrashKonijn.Goap.Resolver.Comparison.SmallerThan, injector.config1.stalkDistance);

            // Hide Goal
            builder.AddGoal<HideGoal>()
                .AddCondition<PlayerAwarenessWK>(CrashKonijn.Goap.Resolver.Comparison.SmallerThanOrEqual, 0);


            // Hurt Player Goal
            builder.AddGoal<HurtPlayerGoal>()
                .AddCondition<MonsterAggressionLevelWK>(CrashKonijn.Goap.Resolver.Comparison.GreaterThanOrEqual, injector.config1.aggressionThreshold);
 
    }
}
*/
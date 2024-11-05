using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Classes;
using Assets.Scripts.GOAP;
using Assets.Scripts.GOAP.Sensors;

public class WandreNearPlayersSmellAction : WanderActionM
{

    public override float GetCost(IMonoAgent agent, IComponentReference references)
    {
        var playerSensor = references.GetCachedComponentInChildren<PlayerAwarenessSensor>();
        bool playerInSight = playerSensor != null && playerSensor.playerinsight;
        var playerSmellSensor = references.GetCachedComponentInChildren<PlayerSensor>();
        float fresnessOfSmell = playerSmellSensor != null ? playerSmellSensor.getSentFreshness() : 0f;
        int toOldFresness = config.smellFressness *50;
        if (playerInSight)
        {
            return config.goToSenMaxCost;
        }
        else
        {
            float freshnessFactor = Mathf.Clamp01(fresnessOfSmell / toOldFresness);
            return Mathf.Lerp(config.goToSentminCost, config.goToSenMaxCost, freshnessFactor);
        }
    }


    public override ActionRunState Perform(IMonoAgent agent, CommonDataM data, ActionContext context)
    {
        var playerSensor = agent.GetComponentInChildren<PlayerAwarenessSensor>();

        if (playerSensor.playerinsight)
        {

            return ActionRunState.Stop;
        }


        return base.Perform(agent, data, context);
    }
}

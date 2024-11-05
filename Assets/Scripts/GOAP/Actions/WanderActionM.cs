using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Classes;
using Assets.Scripts.GOAP;
using Assets.Scripts.GOAP.Sensors;
using UnityEngine.AI;

public class WanderActionM : ActionBase<CommonDataM>, IInjectableObj
{
    public MonsterConfig config;
    float maxTimer = 6;

    public override float GetCost(IMonoAgent agent, IComponentReference references)
    {
        float freshness = references.GetCachedComponentInChildren<PlayerSensor>().getSentFreshness();
        return Mathf.Lerp(config.wanderingCostRange.x, config.wanderingCostRange.y, freshness / config.smellFressness);
    }

    public override void Created()
    {
    
    }

    public override void End(IMonoAgent agent, CommonDataM data)
    {
        
    }

    public void Inject(DependencyInjector injector)
    {
        config = injector.config1 as MonsterConfig;
    }

    public override ActionRunState Perform(IMonoAgent agent, CommonDataM data, ActionContext context)
    {
        data.timer -= context.DeltaTime;
        NavMeshAgent navAgent = agent.GetComponent<AgentMoveBehavior>().navMeshAgent;
        bool reachedEnd = false;

        if (navAgent != null)
        {
            float dist = Vector3.Distance(navAgent.transform.position, navAgent.pathEndPosition);
            reachedEnd = (dist < config.minWalkingDistance); 
        }

        if (data.timer >= 0 && !reachedEnd)
        {
            return ActionRunState.Continue;
        }
        return ActionRunState.Stop;
    }

    public override void Start(IMonoAgent agent, CommonDataM data)
    {

        data.timer = Random.Range(1, maxTimer);
    }
}

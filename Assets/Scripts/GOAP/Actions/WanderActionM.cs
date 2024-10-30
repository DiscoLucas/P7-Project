using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Classes;
using Assets.Scripts.GOAP;

public class WanderActionM : ActionBase<CommonDataM>, IInjectableObj
{
    MonsterConfig config;
    float maxTimer = 6;
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
        if (data.timer >= 0) {
            return ActionRunState.Continue;
        }
        return ActionRunState.Stop;
    }

    public override void Start(IMonoAgent agent, CommonDataM data)
    {

        data.timer = Random.Range(1, maxTimer);
    }
}

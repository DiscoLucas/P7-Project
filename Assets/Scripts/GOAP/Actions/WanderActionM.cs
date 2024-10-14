using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Classes;

public class WanderActionM : ActionBase<CommonDataM>
{
    public float maximumTime = 2;
    public override void Created()
    {
    
    }

    public override void End(IMonoAgent agent, CommonDataM data)
    {
        
    }

    public override ActionRunState Perform(IMonoAgent agent, CommonDataM data, ActionContext context)
    {
        data.timer = context.DeltaTime;
        if (data.timer <= 0) {
            return ActionRunState.Continue;
        }
        return ActionRunState.Stop;
    }

    public override void Start(IMonoAgent agent, CommonDataM data)
    {
        data.timer = Random.Range(0, maximumTime);
    }
}

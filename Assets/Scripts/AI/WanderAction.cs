using UnityEngine;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

public class WanderAction : ActionBase<WanderAction.Data>
{
    // called when the action is created
    public override void Created()
    {
        throw new System.NotImplementedException();
    }
    
    // Called when the action is started for a specific agent.
    public override void Start(IMonoAgent agent, Data data)
    {
        // When the agent is at the target, wait a random amount of time before moving to the next target.
        data.Timer = Random.Range(0.3f, 1.5f);
    }

    // Called every frame while the action is active. It's only active when the agent is in range of the target.
    public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
    {
        data.Timer -= context.DeltaTime;

        // if the timer is higher than 0, continue.
        if (data.Timer > 0) return ActionRunState.Continue;
        
        // The action is done, return stop. This will trigger the resolver for the next action.
        return ActionRunState.Stop;
    }

    public override void End(IMonoAgent agent, Data data)
    {
    }

    public class Data : IActionData
    {
        public ITarget Target { get; set; }
        public float Timer { get; set; }
    }
}

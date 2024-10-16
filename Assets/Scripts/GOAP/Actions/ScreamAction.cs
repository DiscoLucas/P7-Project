using System.Collections;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.GOAP.Actions
{
    public class ScreamAction : ActionBase<CommonDataM>
    {
        public override void Created() { }

        public override void Start(IMonoAgent agent, CommonDataM data)
        {
            Debug.Log("Scream started " + Time.timeSinceLevelLoad);
            data.timer = Random.Range(1f, 2f); // Scream lasts for 1-2 seconds
                                               // Set in range to false to stop movement
            (agent as AgentBehaviour)?.Events.TargetChanged(data.Target, false);
        }

        public override ActionRunState Perform(IMonoAgent agent, CommonDataM data, ActionContext context)
        {
            data.timer -= context.DeltaTime;
            if (data.timer <= 0)
            {
                return ActionRunState.Stop;
            }
            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, CommonDataM data)
        {
            Debug.Log("Scream ended " + Time.timeSinceLevelLoad);
            // Restore in range state if needed
            (agent as AgentBehaviour)?.Events.TargetChanged(data.Target, true);
        }
    }


    public class ScreamData : IActionData
    {
        public TransformTarget Target { get; set; }
        ITarget IActionData.Target
        {
            get => Target;
            set => Target = value as TransformTarget; // Ensure the target is set correctly
        }
    }
}

using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Enums;

namespace Assets.Scripts.GOAP.Actions
{
    public class StalkAction : ActionBase<StalkData>, IInjectableObj
    {
        private MonsterConfig config;
        private Transform target;

        public void Inject(DependencyInjector injector)
        {
            config = injector.config1 as MonsterConfig;
        }

        public override void Created()
        {
            // Initialize the stalk action
        }

        public override void Start(IMonoAgent agent, StalkData data)
        {
            target = data.Target?.Transform; // Use Transform instead of Position
            Debug.Log("Monster starts stalking the player.");
        }

        public override ActionRunState Perform(IMonoAgent agent, StalkData data, ActionContext context)
        {
            // Logic to stalk the player
            if (target != null)
            {
                // Move towards the player while being stealthy
                Debug.Log("Monster stalking the player...");
                // Here you can add the movement logic to approach the target
                return ActionRunState.Continue;
            }

            return ActionRunState.Stop; // Stop stalking if no target
        }

        public override void End(IMonoAgent agent, StalkData data)
        {
            Debug.Log("Monster stops stalking.");
        }
    }

    public class StalkData : IActionData
    {
        public TransformTarget Target { get; set; }

        ITarget IActionData.Target
        {
            get => Target;
            set => Target = value as TransformTarget; // Ensure the target is set correctly
        }
    }
}

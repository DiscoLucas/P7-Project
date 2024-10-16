using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Classes;
using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;

namespace Assets.Scripts.GOAP.Actions
{
    public class ScreamAction : ActionBase<ScreamData>, IInjectableObj
    {
        private MonsterConfig config;

        public void Inject(DependencyInjector injector)
        {
            config = injector.config1 as MonsterConfig;
        }

        public override void Created()
        {
            // Initialize the action if necessary
        }

        public override void Start(IMonoAgent agent, ScreamData data)
        {
            // Logic to perform when the scream action starts
            Debug.Log("Monster screams to attract player!");
            // Optionally add logic for sound effects or triggering a scream animation
        }

        public override ActionRunState Perform(IMonoAgent agent, ScreamData data, ActionContext context)
        {
            // Logic to continue the scream action if needed
            // This could involve timing or conditions to stop the scream
            return ActionRunState.Stop; // Ends the scream action after one execution
        }

        public override void End(IMonoAgent agent, ScreamData data)
        {
            // Logic to clean up after the scream action ends
        }
    }

    // Custom data class for scream action
    public class ScreamData : IActionData, ITarget
    {
        public TransformTarget Target { get; set; } // Target to scream at (the player)

        // Implement ITarget
        public Vector3 Position => Target.Position; // Returns the target position

        // Implement IActionData
        ITarget IActionData.Target
        {
            get => Target;
            set => Target = value as TransformTarget; // Ensure the target is set correctly
        }
    }
}

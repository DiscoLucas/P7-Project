using System.Collections;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using JSAM;

namespace Assets.Scripts.GOAP.Actions
{
    public class ScreamAction : ActionBase<CommonDataM>
    {
        public override void Created() { }

        public override void Start(IMonoAgent agent, CommonDataM data)
        {

        }

        public override ActionRunState Perform(IMonoAgent agent, CommonDataM data, ActionContext context)
        {

            //AudioManager.PlaySound("TEST");
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, CommonDataM data)
        {

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

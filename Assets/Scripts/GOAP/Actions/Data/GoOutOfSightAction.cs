using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Actions.Data
{
    public class GoOutOfSightAction : ActionBase<CommonDataM>
    {
        public override void Created()
        {

        }

        public override void End(IMonoAgent agent, CommonDataM data)
        {

        }

        public override ActionRunState Perform(IMonoAgent agent, CommonDataM data, ActionContext context)
        {
            // Logic to move to a hidden position
            return ActionRunState.Stop;
        }

        public override void Start(IMonoAgent agent, CommonDataM data)
        {
        }
    }
}
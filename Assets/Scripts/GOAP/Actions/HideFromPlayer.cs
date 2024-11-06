using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Actions
{
    public class HideFromPlayer : PlayerVisibilityBasedAction
    {
        public override void Start(IMonoAgent agent, CommonDataM data)
        {
            base.Start(agent, data);
            agent.GetComponent<AgentSpeedBehavior>().changeSpeed(BotState.STALK, true);
        }
    }

}
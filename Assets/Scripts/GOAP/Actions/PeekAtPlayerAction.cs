using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Actions
{
    public class PeekAtPlayerAction : PlayerVisibilityBasedAction
    {
        public override int calculateBase(int playerAwareness, float midpoint)
        {
            return (int)Mathf.Lerp(config.peakCostRange.x, config.peakCostRange.y, 1 - ((playerAwareness - config.stalkMinPlayerAwareness) / (midpoint - config.stalkMinPlayerAwareness)));
        }

        public override void Start(IMonoAgent agent, CommonDataM data)
        {
            base.Start(agent, data);
            agent.GetComponent<AgentSpeedBehavior>().changeSpeed(BotState.STALK, false);
        }
    }

}
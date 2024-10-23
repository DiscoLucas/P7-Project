using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Actions
{
    public class GoToPlayerLastPosAction : ActionBase<CommonDataM>, IInjectableObj
    {
        MonsterConfig monsterConfig;
        public override void Created()
        {

        }

        public override void End(IMonoAgent agent, CommonDataM data)
        {

        }

        public void Inject(DependencyInjector injector)
        {
            monsterConfig = injector.config1 as MonsterConfig;
        }

        public override ActionRunState Perform(IMonoAgent agent, CommonDataM data, ActionContext context)
        {
            // Check if the target was set correctly
            if (data.Target == null)
            {
                Debug.LogError("Data target is null!");
                return ActionRunState.Stop;  // Stop the action if no target is present
            }
            data.timer = context.DeltaTime;
            Debug.Log("Agent is null: " +(agent == null));
            Debug.Log("Data target is null: " + (data.Target == null));
            
            Vector3 agentPos = agent.transform.position;
            Vector3 postionPlayer =new Vector3(data.Target.Position.x,agentPos.y, data.Target.Position.z);
            bool timerOut = (data.timer <= 0);
            bool canAttack = (Vector3.Distance(postionPlayer, agentPos) >= monsterConfig.meleeRange);
            Debug.Log("pPos: " + postionPlayer + " aPos: " + agentPos + " timeout: " + timerOut + " canattack: " + canAttack);
            if (timerOut || canAttack ) 
            {
                return ActionRunState.Continue;
            }
            return ActionRunState.Stop;
        }

        public override void Start(IMonoAgent agent, CommonDataM data)
        {
            if (data.Target == null)
            {
                Debug.LogError("Data target is null in Start method.");
            }
            else
            {
                Debug.Log("Target acquired: " + data.Target.Position);
            }

            data.timer = 10;
        }
    }
}
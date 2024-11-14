using Assets.Scripts.GOAP.Behaviors;
using Assets.Scripts.GOAP.Sensors;
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
        MonsterBrain brain;
        bool newPosSpottede = false;
        float freshness; 



        public override void Created()
        {
            newPosSpottede = false;
        }

        public override void End(IMonoAgent agent, CommonDataM data)
        {

        }

        public void Inject(DependencyInjector injector)
        {
            monsterConfig = injector.config1 as MonsterConfig;
            brain = injector.brain;
            brain.awarenessSensor.playerSpottede.AddListener(changeTargetPos);
        }

        void changeTargetPos(bool isspottede) {
            if (isspottede) {
                brain.p_sensor.updatePlayerPos();
                newPosSpottede=true;
            }
        }
        public override ActionRunState Perform(IMonoAgent agent, CommonDataM data, ActionContext context)
        {
            if (newPosSpottede) {
                return ActionRunState.Stop;
            }
                

            // Check if the target was set correctly
            if (data.Target == null)
            {
                return ActionRunState.Stop;
            }
            data.timer = context.DeltaTime;
            
            Vector3 agentPos = agent.transform.position;
            Vector3 postionPlayer =new Vector3(data.Target.Position.x, agentPos.y , data.Target.Position.z);
            bool timerOut = (data.timer <= 0);
            bool canAttack = (Vector3.Distance(postionPlayer, agentPos) >= monsterConfig.meleeRange);
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

            data.timer = 10;
        }
    }
}
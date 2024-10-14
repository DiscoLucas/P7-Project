using CrashKonijn.Goap.Behaviours;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Behaviors
{
    /// <summary>
    /// Set the agent goap-settings
    /// </summary>
    [RequireComponent(typeof(AgentBehaviour))]
    public class GoapBinderConfig : MonoBehaviour
    {
        [SerializeField] private GoapRunnerBehaviour goapRunner;
        private void Awake()
        {
            AgentBehaviour agent = GetComponent<AgentBehaviour>();
            agent.GoapSet = goapRunner.GetGoapSet("Monsterset");
        }
    }
}
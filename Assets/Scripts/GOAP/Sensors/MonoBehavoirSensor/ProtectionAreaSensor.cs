using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Sensors
{
    public class ProtectionAreaSensor : MonoBehaviour, IInjectableObj
    {
        public delegate void PlayerEnterEvent(Transform player);
        public delegate void PlayerExitEvent(Vector3 lastKnownPosition);
        public GameObject player;
        public event PlayerEnterEvent OnPlayerEnter;
        public event PlayerExitEvent OnPlayerExit;
        public float protectionAreaRadius;
        private MonsterConfig config;
        bool haveEnterede = false;
        public void Inject(DependencyInjector injector)
        {
            config = injector.config1 as MonsterConfig;
            protectionAreaRadius = config.protectionAreaRadius; 
        }

        private void Awake()
        {
            
        }

        public void Start() { 
            player = GameObject.FindGameObjectsWithTag("Player")[0];
        }

        private void FixedUpdate()
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if ((!haveEnterede) && (dist <= protectionAreaRadius))
            {

                OnPlayerEnter?.Invoke(player.transform);
                
                haveEnterede =true;

            }
            else if(haveEnterede && (dist > protectionAreaRadius)) {

                OnPlayerExit?.Invoke(player.transform.position);

                haveEnterede = false ;
            }
        }
    }
}

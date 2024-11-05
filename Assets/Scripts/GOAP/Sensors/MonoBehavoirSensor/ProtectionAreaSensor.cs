using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Sensors
{
    [RequireComponent(typeof(SphereCollider))]
    public class ProtectionAreaSensor : MonoBehaviour, IInjectableObj
    {
        public SphereCollider Collider;
        public delegate void PlayerEnterEvent(Transform player);
        public delegate void PlayerExitEvent(Vector3 lastKnownPosition);

        public event PlayerEnterEvent OnPlayerEnter;
        public event PlayerExitEvent OnPlayerExit;

        private MonsterConfig config; 

        public void Inject(DependencyInjector injector)
        {
            config = injector.config1 as MonsterConfig; 
            Collider.radius = config.protectionAreaRadius; 
        }

        private void Awake()
        {
            Collider = GetComponent<SphereCollider>();
            Collider.isTrigger = true; 
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                OnPlayerEnter?.Invoke(player.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                OnPlayerExit?.Invoke(other.transform.position);
            }
        }
    }
}

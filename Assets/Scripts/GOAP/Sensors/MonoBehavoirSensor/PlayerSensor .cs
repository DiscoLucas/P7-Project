using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Sensors
{

    [RequireComponent(typeof(SphereCollider))]
    public class PlayerSensor : MonoBehaviour
    {
        public SphereCollider Collider;
        public delegate void PlayerEnterEvent(Transform player);
        public delegate void PlayerExitEvent(Vector3 lastKnownPosition);
        public Transform playerLastPos;
        public Transform playersRealPostion;
        public event PlayerEnterEvent OnPlayerEnter;
        public event PlayerExitEvent OnPlayerExit;

        private void Awake()
        {
            Collider = GetComponent<SphereCollider>();
            
        }

        public int ditanceToPlayer() {
            return Mathf.RoundToInt((Vector3.Distance(transform.position, playersRealPostion.position)));
        }


        private void Start()
        {
            playerLastPos.parent = null;
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
                OnPlayerExit?.Invoke(player.transform.position);
            }
        }
    }
}
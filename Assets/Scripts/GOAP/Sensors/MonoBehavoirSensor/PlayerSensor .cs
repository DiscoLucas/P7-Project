using Assets.Scripts.GOAP.Behaviors;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GOAP.Sensors
{
    [DefaultExecutionOrder(3)]
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerSensor : MonoBehaviour
    {
        public SphereCollider Collider;
        public delegate void PlayerEnterEvent(Transform player);
        public delegate void PlayerExitEvent(Vector3 lastKnownPosition);
        public Transform playerLastPos, smellTargetLastPos, hidespot, peakSpot;
        public Transform playersRealPostion;
        public event PlayerEnterEvent OnPlayerEnter;
        public event PlayerExitEvent OnPlayerExit;
        public SessionLogTracker sessionLogTracker;
        public MonsterBrain brain;
        [SerializeField]
        float playerSmellFreshness = float.MaxValue, distanceToSmellPoint = 0;
        bool playerHaveBeenSmelled = false;

        private void Awake()
        {
            Collider = GetComponent<SphereCollider>();
            
        }

        private void FixedUpdate()
        {
            if (sessionLogTracker == null) {
                getSessionLogTrakor();
            }
            if (playerHaveBeenSmelled) {
                playerSmellFreshness += 1;

            }
                
        }

        public int ditanceToPlayer() {
            return Mathf.RoundToInt((Vector3.Distance(transform.position, playerLastPos.position)));
        }

        public void updatePlayerPos() {
            playerLastPos.position = new Vector3(playersRealPostion.position.x, playerLastPos.position.y, playersRealPostion.position.z);
        }
        void getSessionLogTrakor() {
            sessionLogTracker = SessionLogTracker.Instance;
            sessionLogTracker.playerPostionsSummeries.AddListener(playerPostionHaveBeenSummeries);
        }
        private void Start()
        {
            playerLastPos.parent = null;
            smellTargetLastPos.parent = null;
            hidespot.parent = null;
            peakSpot.parent = null;
            playersRealPostion = GameManager.Instance.playerObject.transform;
            getSessionLogTrakor();
        }

        public void playerPostionHaveBeenSummeries(Vector3 pos, float dist) { 
            playerHaveBeenSmelled = true;
            playerSmellFreshness = 0;
            distanceToSmellPoint = dist;
            smellTargetLastPos.position = pos;
        }

        public float getSentFreshness() { 
            return playerSmellFreshness;
        }

        public Transform getSentSmelledPoint() { 
            return smellTargetLastPos;
        }

        public float getSentSmelledDistance() {
            return distanceToSmellPoint;
        }

       /*  private void OnTriggerEnter(Collider other)
        {
           if (other.TryGetComponent(out PlayerMovement player))
            {
                OnPlayerEnter?.Invoke(playerLastPos.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                OnPlayerExit?.Invoke(playerLastPos.transform.position);
            }
        }*/
    }
}
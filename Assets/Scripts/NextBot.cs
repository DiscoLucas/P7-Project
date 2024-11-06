using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class NextBot : MonoBehaviour
{
    
    
    public NavMeshAgent agent;
    GameObject player;
    Vector3 playerPosition;
    Vector3 randomHidePoint;
    public float minDistanceToPlayer = 20f;
    public float searchRadius = 100f;

    [Header("Bot behavior")]
    public BotState State;// { get; set; }
    public string StateDisplay;
    public static event Action<BotState> OnBeforeStateChange;
    public static event Action<BotState> OnAfterStateChange;

    [Header("Physical Properties")]
    public float chaseSpeed = 5f;
    public float walkSpeed = 2f;
    public float runSpeed = 10f; // used for running away from player. Doesn't need to be balanced.
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        playerPosition = player.transform.position;
        // set initial state
        State = BotState.STALK;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
        
        switch (State)
        {
            case BotState.CHASE:
                ChasePlayer();
                break;
            case BotState.STALK:
                StalkPlayer();
                break;
            case BotState.IDLE:
                break;
            default:
                break;
        }
        StateDisplay = State.ToString();
    }
    /// <summary>
    /// Changes the state of the bot. If the new state is the same as the current state, the state will not change.
    /// </summary>
    /// <param name="newState"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    void ChangeStateOnce(BotState newState)
    {
        // guard to prevent duplicate state changes
        if (State == newState) return;

        OnBeforeStateChange?.Invoke(newState);
        State = newState;
        Debug.Log($"Bot state changed to {newState}");

        switch (newState)
        {
            case BotState.HIDE:
                Hide();
                break;
            case BotState.STALK:
                StalkPlayer();
                break;
            case BotState.IDLE:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        StateDisplay = newState.ToString();
        OnAfterStateChange?.Invoke(newState);
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(playerPosition);
    }

    void StalkPlayer()
    {
        agent.speed = walkSpeed;
        agent.SetDestination(playerPosition);
        
        if (IsPlayerVisible())
        {
            int coinFlip = Random.Range(0, 2);
            Debug.Log(coinFlip);
            if (coinFlip == 0) State = BotState.CHASE;
            else if (coinFlip == 1) ChangeStateOnce(BotState.HIDE);
        }
    }

    void Hide()
    {
        
        agent.speed = runSpeed;
        agent.SetDestination(GetRandomNavMeshPoint());
        if (agent.remainingDistance < 1f)
        {
            State = BotState.STALK;
        }
    }

    bool IsPlayerVisible()
    {
        // TODO: change to look for player camera
        RaycastHit hit;
        Vector3 direction = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            Debug.DrawRay(transform.position, direction, Color.red);
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, direction, Color.green); 
                return true;
            }
        }
        return false;
    }

    bool IsSeenByPlayer() 
    {   

        //implemnt later
        return false;
    }

    Vector3 GetRandomNavMeshPoint()
    {
        Vector3 randomPoint = Vector3.zero;
        int maxTries = 30;
        // TODO: implement error handling for when no point is found

        for (int i = 0; i < maxTries; i++)
        {
            // get a random point
            Vector3 randomDirection = Random.insideUnitSphere * searchRadius;
            randomDirection += transform.position;

            // check if it's on the navmesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, searchRadius, NavMesh.AllAreas))
            {
                float distanceFromPlayer = Vector3.Distance(hit.position, playerPosition);
                
                if (distanceFromPlayer >= minDistanceToPlayer)
                {
                    randomPoint = hit.position;
                    break;
                }
            }
        }
        return randomPoint;
    }

}
[Serializable]
public enum BotState
{
    CHASE,
    HIDE,
    STALK,
    IDLE
}

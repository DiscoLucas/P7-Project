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
    public BotState State { get; private set; }
    public string StateDisplay;
    public static event Action<BotState> OnBeforeStateChange;
    public static event Action<BotState> OnAfterStateChange;

    [Header("Physical Properties")]
    public float chaseSpeed = 5f;
    public float walkSpeed = 2f;
    public float runSpeed = 10f; // used for running away from player. Doesn't need to be balanced.

    public enum BotState
    {
        CHASE,
        HIDE,
        STALK,
        IDLE
    }

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
            case BotState.HIDE:
                Hide();
                break;
            case BotState.STALK:
                StalkPlayer();
                break;
            case BotState.IDLE:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(State), State, null);
        }
        StateDisplay = State.ToString();
    }

    void ChangeState(BotState newState)
    {
        // guard to prevent duplicate state changes
        //if (State == newState) return;

        OnBeforeStateChange?.Invoke(newState);
        State = newState;
        Debug.Log($"Bot state changed to {newState}");

        switch (newState)
        {
            case BotState.CHASE:
                ChasePlayer();
                break;
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
            int coinFlip = Random.Range(0, 1);
            if (coinFlip == 0) State = BotState.CHASE;
            else State = BotState.HIDE;
        }
    }

    void Hide()
    {
        agent.speed = runSpeed;
        agent.SetDestination(GetRandomNavMeshPoint());
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

public class DumbBot : MonoBehaviour
{
    StateMachine fsm;
    public NavMeshAgent agent;

    public string stateDisplayText = "";

    [Header("Speed Settings")]
    public float walkSpeed = 2f;
    public float chaseSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Timer Settings")]
    public float chaseTime = 30f;
    private float RandomIdleTime() { return Random.Range(1f, 5f);}

    public float attackRange = 1f;
    public float minDistanceToPlayer = 20f;
    public float searchRadius = 100f;

    public string newState;

    GameObject player;
    Vector3 playerPosition;
    public Transform home;
    float distanceToPlayer => Vector3.Distance(playerPosition, transform.position);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        playerPosition = player.transform.position;

        fsm = new StateMachine();
        
        // configure states
        fsm.AddState("Stalk",
            onLogic: state => agent.SetDestination(playerPosition),
            onEnter: state => agent.speed = walkSpeed);
        fsm.AddState("Chase",
            onLogic: state => agent.SetDestination(playerPosition),
            onEnter: state => agent.speed = chaseSpeed);
        fsm.AddState("Idle",
            onEnter: state => StartCoroutine(TriggerAfterDelay("StartStalking", RandomIdleTime())));
        fsm.AddState("RunAway",
            onEnter: state => { 
                agent.SetDestination(GetRandomNavMeshPoint());
                agent.speed = runSpeed;
                Debug.Log("Remaining distance: " + agent.remainingDistance);
            },
            onExit: state => Debug.Log("Remaining distance after running: " + agent.remainingDistance)
            );
        fsm.AddState("EvaluateState",  
        onEnter: state => {
            newState = ChooseRandomState();
            Debug.Log("New state is: " + newState);
            //Debug.Break();
        });
        fsm.AddState("PeekPlayer");
        fsm.AddState("Attack",
            onEnter: state => Debug.Log("*Teleports behind you* nothing personal kid."));
        fsm.AddState("GoHome",
            onEnter: state => agent.SetDestination(home.position));

        fsm.SetStartState("Stalk");

        // configure transitions
        fsm.AddTriggerTransition("PlayerVisible", "Stalk", "EvaluateState"); // TODO: fix EvaluateState. It works sometimes, but its REALLY fucky.
        fsm.AddTransition("EvaluateState", "RunAway", t => newState == "RunAway");
        fsm.AddTransition("EvaluateState", "Chase", t => newState == "Chase");
        //fsm.AddTransition("Chase", "Attack", t => distanceToPlayer <= attackRange); // attack player
        fsm.AddTransition(new TransitionAfter("Chase", "RunAway", chaseTime)); // run away for a certain amount of time 
        fsm.AddTransition("RunAway", "Idle", t => !float.IsInfinity(agent.remainingDistance) && agent.remainingDistance < 1f); // stop running
        fsm.AddTriggerTransition("StartStalking", "Idle", "Stalk");
        
        fsm.Init();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(agent.remainingDistance);
        fsm.OnLogic();
        StateBase<string> state = fsm.ActiveState;
        string currentState = fsm.ActiveStateName;

        playerPosition = player.transform.position;

        if (currentState == "Stalk") // this should be a way to trigger the chase state without polling.
            if (IsPlayerVisible())
                fsm.Trigger("PlayerVisible");

        stateDisplayText = fsm.GetActiveHierarchyPath();
        //Debug.Log(newState);
    }
    string ChooseRandomState()
    {
        int coinFlip = Random.Range(0, 2);
        Debug.Log("coinflip resulted in: " + coinFlip);
        if (coinFlip == 0) return "Chase";
        else return "RunAway";
    }

    bool IsPlayerVisible()
    {
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

    Vector3 GetRandomNavMeshPoint()
    {
        Vector3 randomPoint = Vector3.zero;
        int maxTries = 100;
        bool validPointFound = false;

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

                if (distanceFromPlayer >= minDistanceToPlayer && agent.pathStatus != NavMeshPathStatus.PathInvalid)
                {
                    randomPoint = hit.position;
                    validPointFound = true;
                    break;
                }
            }

            if (i == maxTries - 1)
            {
                Debug.LogWarning("No valid position found on the NavMesh.");
                
            }
        }

        if (!validPointFound)
        {
            randomPoint = home.position;;
        }

        return randomPoint;
    }
    private IEnumerator TriggerAfterDelay(string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);
        fsm.Trigger(triggerName); // Trigger the transition after the delay
    }

}

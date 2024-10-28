using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

public class DumbBot : MonoBehaviour
{
    StateMachine fsm;
    public NavMeshAgent agent;

    public float walkSpeed = 2f;
    public float chaseSpeed = 5f;
    public float runSpeed = 10f;

    public float chaseTime = 30f;
    private float chaseTimer;
    public float attackRange = 1f;
    public float minDistanceToPlayer = 20f;
    public float searchRadius = 100f;

    GameObject player;
    Vector3 playerPosition;
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
        fsm.AddState("Idle");
        fsm.AddState("RunAway",
            onEnter: state => { 
                agent.SetDestination(GetRandomNavMeshPoint());
                agent.speed = runSpeed;
            });
        fsm.AddState("PeekPlayer");
        fsm.AddState("Attack",
            onEnter: state => Debug.Log("*Teleports behind you* nothing personal kid."));

        fsm.SetStartState("Stalk");

        // configure transitions
        fsm.AddTriggerTransition("PlayerVisible", "Stalk", ChooseRandomState()); // TODO: fix the outcome from being deterministic
        fsm.AddTransition("Chase", "Attack", t => distanceToPlayer <= attackRange); // attack player
        fsm.AddTransition(new TransitionAfter("Chase", "RunAway", chaseTime)); // run away for a certain amount of time 
        fsm.AddTransition("RunAway", "Idle", t => agent.remainingDistance < 1f); // stop running
        
        fsm.Init();
    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnLogic();
        StateBase<string> state = fsm.ActiveState;
        string currentState = fsm.ActiveStateName;

        playerPosition = player.transform.position;

        if (currentState == "Stalk") // this should be a way to trigger the chase state without polling.
            if (IsPlayerVisible())
                fsm.Trigger("PlayerVisible");
    }
    string ChooseRandomState()
    {
        int coinFlip = Random.Range(0, 2);
        Debug.Log("coinflip resulted in: " + coinFlip);
        if (coinFlip == 0) return "Chase";
        else return "RunAway";
    }
    void Stalk()
    {
       agent.speed = walkSpeed;
       agent.SetDestination(playerPosition);
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

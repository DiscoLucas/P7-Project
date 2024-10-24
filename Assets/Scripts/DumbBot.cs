using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

public class DumbBot : MonoBehaviour
{
    StateMachine fsm;
    NavMeshAgent agent;

    public float walkSpeed = 2f;
    public float chaseSpeed = 5f;
    public float runSpeed = 10f;

    public float chaseTime = 30f;
    private float chaseTimer;
    public float attackRange = 1f;

    GameObject player;
    Vector3 playerPosition;
    float distanceToPlayer => Vector3.Distance(playerPosition, transform.position);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");

        fsm = new StateMachine();
        
        // configure states
        fsm.AddState("Stalk");
        fsm.AddState("Chase");
        fsm.AddState("Idle");
        fsm.AddState("RunAway");
        fsm.AddState("PeekPlayer");
        fsm.AddState("Attack");

        fsm.SetStartState("Stalk");

        // configure transitions
        fsm.AddTwoWayTransition("Chase", "Attack", t => distanceToPlayer <= attackRange);
        new TransitionAfter("Chase", "Hide", chaseTime);
        
        fsm.Init();
    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnLogic();
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
}

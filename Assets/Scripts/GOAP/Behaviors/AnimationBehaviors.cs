using Assets.Scripts.GOAP;
using JSAM;
using UnityEngine;
using UnityEngine.AI;

public class AnimationBehaviors : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent;
    [SerializeField]
    Animator animator;
    [SerializeField]
    DumbBot nextbot;
    [Header("Animation Variables")]
    [SerializeField]
    AgentSoundBehaviors soundBehaviors;
    [SerializeField]
    string speedVariable = "Speed",screamTriggerVariable = "Scream", attackTriggerVariable = "Attack" , idealVariable = "IdealState",screamStateVariable = "mainScream";
    float maxValue = 0f;
    float oldSpeed = 0f;
    [SerializeField]
    float smoothFactor = 7;
    [SerializeField]
    float oscillationSpeed = 2.0f;
    [SerializeField]
    bool attackActionCurrent = false, screamState = false;
    void Start()
    {
        agent = transform.parent.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        nextbot = transform.parent.GetComponent<DumbBot>();
        if (nextbot == null)
        {
            maxValue = transform.parent.GetComponentInChildren<DependencyInjector>().config1.baseChaseSpeed;
            soundBehaviors = transform.parent.GetComponentInChildren<AgentSoundBehaviors>();
        }
        else { 
            maxValue = nextbot.runSpeed;
        }

    }

    private void FixedUpdate()
    {

        float currentSpeed = (agent.velocity.magnitude / maxValue) * 100;
        float smoothSpeed = Mathf.Lerp(oldSpeed, currentSpeed, Time.fixedDeltaTime * smoothFactor);
        oldSpeed = smoothSpeed;
        float idealState = (Mathf.Sin(Time.fixedTime * oscillationSpeed) + 1) / 2;
        animator.SetFloat(idealVariable, idealState);
        animator.SetFloat(speedVariable, smoothSpeed);
    }
    public void startScream() {
        soundBehaviors.playerScream();
    }

    public void startScreamAction() {  
        animator.SetInteger(screamStateVariable, screamState? 1: 0);
        animator.SetTrigger(screamTriggerVariable);
        screamState = (Random.Range(0,10) > 5);



    }

    public void startAttack() {
        animator.SetTrigger(attackTriggerVariable);
        attackActionCurrent = true;
    }
    public void onCollisionEnterAttack() {
        Debug.LogError("Hans hænder var kastede");
    }
    public void endAttack()
    {
        attackActionCurrent = false;
    }

    public bool canEndAttack() {
        return attackActionCurrent;
    }
}

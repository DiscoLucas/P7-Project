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
    [Header("Monster FoorStep Sound & Variables")]
    [SerializeField]
    SoundFileObject monsterFootSteps;
    [SerializeField]
    float maxFootstepRate = 0.5f;
    [SerializeField]
    float footstepTimer = 0;

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
            soundBehaviors = transform.parent.GetComponentInChildren<AgentSoundBehaviors>();
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
        float footstepInterval = Mathf.Lerp(2f, maxFootstepRate, currentSpeed / 100f);
        footstepTimer += Time.fixedDeltaTime;
        if(footstepTimer >= footstepInterval && currentSpeed>0)
        {
            AudioManager.PlaySound(monsterFootSteps);
            footstepTimer = 0;
        }

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
        GameManager.Instance.ChangeState(GameState.GameOver);

    }
    public void endAttack()
    {
        attackActionCurrent = false;
    }

    public bool canEndAttack() {
        return attackActionCurrent;
    }
}

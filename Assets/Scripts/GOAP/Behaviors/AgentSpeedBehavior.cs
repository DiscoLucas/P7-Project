using CrashKonijn.Goap.Classes.Injectors;
using UnityEngine;
using UnityEngine.AI;

public class AgentSpeedBehavior : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public MonsterConfig config;
    public AgressionSensor agressionSensor;

    void Start()
    {
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void changeSpeed(BotState botState, bool isHiding)
    {
        int aggressionLevel = agressionSensor.aggressionLevel;
        float speed = calculateSpeed(botState, aggressionLevel, isHiding, !isHiding);
        changeSpeed(speed);
    }

    public void changeSpeed(BotState botState, int aggressionLevel)
    {
        float speed = calculateSpeed(botState, aggressionLevel,false,false);
        changeSpeed(speed);
    }

    public void changeSpeed(float agentSpeed) { 
        navMeshAgent.speed = agentSpeed;
    }

    public float calculateSpeed(BotState botState, int aggressionLevel, bool isHiding, bool isPeeking)
    {
        float baseSpeed = 0f;
        float aggressionMultiplier = 1f + (aggressionLevel / 100f) * (config.maxAggressionMultiplier - 1f);
        switch (botState)
        {
            case BotState.IDLE:
                baseSpeed = config.baseIdleSpeed;
                break;
            case BotState.STALK:
                baseSpeed = config.baseStalkSpeed;
                break;
            case BotState.CHASE:
                baseSpeed = config.baseChaseSpeed;
                break;
        }
        if (isHiding)
        {
            baseSpeed *= config.hideSpeedModifier;
        }
        else if (isPeeking)
        {
            baseSpeed *= config.peekSpeedModifier;
        }

        float finalSpeed = baseSpeed * aggressionMultiplier;

        return finalSpeed;
    }
}

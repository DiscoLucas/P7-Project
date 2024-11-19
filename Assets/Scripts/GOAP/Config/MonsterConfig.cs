using UnityEngine;

/// <summary>
/// Configuration settings for the monster's behavior and abilities.
/// </summary>
[CreateAssetMenu(fileName = "MonsterConfig", menuName = "Agent Config/MonsterConfig", order = 1)]
public class MonsterConfig : ScriptableObject
{
    [Header("#- Agent settings -#")]
    [Header("Sensor settings")]
    public float AgentSensorRadius = 50f;
    [Header("Player")]
    public LayerMask playerLayerMask;

    [Header("Protection area")]
    public int protectionAreaRadius = 5;

    [Header("Agent movement")]
    public float minWalkingDistance = 0.01f;
    public int targetPosStop = 1;
    public float baseIdleSpeed = 2f;
    public float baseStalkSpeed = 3f;
    public float baseChaseSpeed = 6f;
    public float hideSpeedModifier = 0.5f;
    public float peekSpeedModifier = 1.2f;
    public float maxAggressionMultiplier = 1.5f;

    [Header("Agression level")]
    public int agressionLevelBeginChase = 75;
    public int stalkMaxAgressionLevel = 50;
    public float lowAwarenessCostMultiplier = 0.5f;
    public float highAwarenessCostMultiplier = 1;

    [Header("#- Wandering -#")]
    public float wanderingSetinRange = 10f;

    [Header("Smell player")]
    public float smelledSearchAreaMultiplyer = 2;
    public int smellFressness = 3;

    [Header("#- Stalk -#")]
    public int stalkDitsanceMinDistance = 30;
    public int stalkMinPlayerAwareness = 50;
    public int stalkMaxPlayerAwareness = 70;
    [Header("Hide")]
    public float peekRange = 20;
    [Header("Peak")]
    public float hideRange = 20;

    [Header("Screaming")]
    public int screamHearingRange = 13;
    [Tooltip("In seconds")]
    public int screamInterval = 5;

    [Header("#- Chase -#")]
    public float chaseRange = 20f;
    public float distanceToMonsterForStartChaseAlways = 5;

    [Header("Melee")]
    public float meleeRange = 5f;
    public float attackDelay = 2.5f;


    [Header("#- Cost Settings -#")]
    [Header("Hide")]
    public int hideCost = 12;
    public Vector2 hideCostRange = new Vector2(12, 40);
    [Header("Peak")]
    public int peekCost = 5;
    public Vector2 peakCostRange = new Vector2(12, 40);
    [Header("Scream")]
    public int screamActionCostbase = 5;
    public Vector2 screamActionCostRange = new Vector2(12, 100);
    [Header("Wandering")]
    public int wanderingBaseCost = 25;
    public Vector2 wanderingCostRange = new Vector2(2, 30);
    [Header("Chase")]
    public int chaseCost = 10;
    public Vector2 chaseCostRange = new Vector2(15, 45);
    [Header("Melee")]
    public float meleeCost = 10f;
    public Vector2 meleeCostRange = new Vector2(0, 100);
    [Header("Wandering go to sent")]
    public int goToSentminCost = 2;
    public int goToSenMaxCost = 40;




}

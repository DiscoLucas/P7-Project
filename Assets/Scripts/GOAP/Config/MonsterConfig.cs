using UnityEngine;

/// <summary>
/// Configuration settings for the monster's behavior and abilities.
/// </summary>
[CreateAssetMenu(fileName = "MonsterConfig", menuName = "Agent Config/MonsterConfig", order = 1)]
public class MonsterConfig : ScriptableObject
{
    [Header("General Settings")]
    public float smelledSearchAreaMultiplyer = 2;
    public int smellFressness = 3;
    public int stalkDitsanceMinDistance = 30;
    public int stalkMaxAgressionLevel = 50;
    public int stalkMaxPlayerAwareness = 70;
    public int stalkMinPlayerAwareness = 50;
    public int agressionLevelBeginChase = 75;
    public float minWalkingDistance = 0.01f;
    public float AgentSensorRadius = 50f;
    public LayerMask playerLayerMask;
    public int protectionAreaRadius = 5;

    [Header("Combat Settings")]
    public float meleeRange = 5f;
    public float attackDelay = 2.5f;

    [Header("Chase Settings")]
    public float chaseSpeed = 6f;
    public float chaseRange = 20f;
    public int targetPosStop = 1;

    [Header("Wandering Settings")]
    public float wanderingSetinRange = 10f;
    public float startingAggressionLevel = 0.5f;

    [Header("Awareness Settings")]
    public int stalkDistance = 10;
    public int aggressionThreshold = 14;
    public float stalkActionRange = 12;
    public int screamHearingRange = 13;
    public float peekRange = 20;
    public float hideRange = 20;

    [Header("Cost Settings")]
    public int hideCost = 12;
    public Vector2 hideCostRange = new Vector2(12, 40);
    public int peekCost = 5;
    public Vector2 peakCostRange = new Vector2(12, 40);
    public int screamActionCostbase = 5;
    public Vector2 screamActionCostRange = new Vector2(12, 100);
    public int wanderingBaseCost = 25;
    public Vector2 wanderingCostRange = new Vector2(2, 30);
    public int chaseCost = 10;
    public Vector2 chaseCostRange = new Vector2(15, 45);
    public float meleeCost = 10f;
    public Vector2 meleeCostRange = new Vector2(0, 100);
    public int goToSentminCost = 2;
    public int goToSenMaxCost = 40;
    public float lowAwarenessCostMultiplier = 0.5f;
    public float highAwarenessCostMultiplier = 1;
}

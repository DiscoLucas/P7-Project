using UnityEngine;

/// <summary>
/// Configuration settings for the monster's behavior and abilities.
/// </summary>
[CreateAssetMenu(fileName = "MonsterConfig", menuName = "Agent Config/MonsterConfig", order = 1)]
public class MonsterConfig : ScriptableObject
{
    public float smelledSearchAreaMultiplyer = 2;
    public int smellFressness = 10;
    public int stalkDitsanceMinDistance;
    public int stalkMaxAgressionLevel = 50;
    public int stalkMaxPlayerAwareness = 80;
    public int stalkMinPlayerAwareness = 40;
    public int agressionLevelBeginChase = 75;

    public float minWalkingDistance = 0.25f;

    public float AgentSensorRadius = 25f;

    public LayerMask playerLayerMask;


    public int protectionAreaRadius = 5;

    public float meleeRange = 5f;

    public float attackDelay = 2f;

    public float chaseSpeed = 6f;
    public float chaseRange = 20f;

    public int targetPosStop = 1;



    public float wanderingSetinRange = 10f;

    public float startingAggressionLevel = 0.5f;



    

    public int stalkDistance = 10;

    public int aggressionThreshold = 14;

    public float stalkActionRange = 12;
    public int screamHearingRange = 13;
    public float peekRange = 9;
    public float hideRange = 20;


    public int hideCost = 12;
    public Vector2 hideCostRange = new Vector2(12,40);
    public int peekCost = 5;
    public Vector2 peakCostRange = new Vector2(12, 40);

    public int screamActionCostbase = 5;
    public Vector2 screamActionCostRange = new Vector2(2, 100);

    public int wanderingBaseCost = 5;
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

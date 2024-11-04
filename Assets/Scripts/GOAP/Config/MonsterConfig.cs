using UnityEngine;

/// <summary>
/// Configuration settings for the monster's behavior and abilities.
/// </summary>
[CreateAssetMenu(fileName = "MonsterConfig", menuName = "Agent Config/MonsterConfig", order = 1)]
public class MonsterConfig : ScriptableObject
{
    public float smelledSearchAreaMultiplyer = 2;
    public int goToSentminCost = 2;
    public int goToSenMaxCost = 40;
    public int smellFressness = 10;
    public int stalkDitsanceMinDistance;
    public int stalkMaxAgressionLevel = 50;
    public int stalkMaxPlayerAwareness = 80;
    public int stalkMinPlayerAwareness = 40;
    public int agressionLevelBeginChase = 75;


    [Tooltip("Radius to detect the player.")]
    public float AgentSensorRadius = 25f;

    [Tooltip("Layer mask to identify the player.")]
    public LayerMask playerLayerMask;

    [Tooltip("Radius around the protection point for detecting player proximity.")]
    public int protectionAreaRadius = 5;


    [Tooltip("Range to switch to melee action.")]
    public float meleeRange = 5f;

    [Tooltip("Delay between attacks in seconds.")]
    public float attackDelay = 2f;

    [Tooltip("Cost associated with performing a melee action.")]
    public float meleeCost = 10f;


    [Tooltip("Speed of the monster while chasing the player.")]
    public float chaseSpeed = 6f;

    [Tooltip("Distance at which the monster will start chasing the player.")]
    public float chaseRange = 20f;

    [Tooltip("Cost associated with chasing actions.")]
    public int chaseCost = 10;

    [Tooltip("Position to stop while wandering.")]
    public int targetPosStop = 1;

    [Tooltip("Base cost for wandering actions.")]
    public int wanderingBaseCost = 5;

    [Tooltip("Range at which the wandering action activates.")]
    public float wanderingSetinRange = 10f;

    [Tooltip("Starting aggression level of the monster, influencing its behavior.")]
    public float startingAggressionLevel = 0.5f;

    [Tooltip("Cost associated with stalking action.")]
    public int stalkActionCost = 10;

    [Tooltip("Cost associated with scream action.")]
    public int screamActionCost = 5;

    public int stalkDistance = 10;

    public int aggressionThreshold = 14;

    public float stalkActionRange = 12;
    public int screamHearingRange = 13;
    public int hideCost = 12;
    public float hideRange = 20;
    public int peekCost = 5;
    public float peekRange = 9;


}

using UnityEngine;

[CreateAssetMenu(fileName = "MonsterConfig", menuName = "Agent Config/MonsterConfig", order = 1)]
public class MonsterConfig : ScriptableObject
{
    // Sensor and Detection Settings
    public float AgentSensorRadius = 25;      // Radius to detect the player
    public LayerMask playerLayerMask;         // Layer to detect the player
    public int protectionAreaRadius = 5;      // Radius around the protection point

    // Melee Attack Settings
    public float meleeRange = 5;              // Range to switch to melee action
    public float attackDelay = 2;             // Delay between attacks
    public float meleeCost = 10;              // Cost associated with melee action

    // Chasing and Movement Settings
    public float chaseSpeed = 6;              // Speed during chase
    public float chaseRange = 20;             // Distance at which chasing begins
    public int chaseCost = 10;

    // Wander and Exploration Settings
    public int targetPosStop = 1;             // Position to stop while wandering
    public int wanderingBaseCost = 5;         // Base cost for wandering actions
    public float wanderingSetinRange = 10;    // Range at which the wandering action activates

    // Additional Behavior Settings
    public float idleTime = 3;                // Time spent idling between actions
    public float patrolRadius = 15;           // Patrol radius for wandering

    // Starting aggression level
    public float startingAggressionLevel = 0.5f; // Adjust as needed
    public float screamFrequency = 5.0f; // Time between screams

    public int stalkActionCost = 10; // Cost for stalking action
    public int screamActionCost = 5;  // Cost for scream action
}

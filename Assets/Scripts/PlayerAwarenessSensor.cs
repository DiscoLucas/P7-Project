using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAwarenessSensor : MonoBehaviour
{
    public float playerAwarenessLevel;
    public GameObject player;
    public Camera playerCamera;
    public float fieldOfViewOfPlayer;
    [SerializeField]
    int firstTimeplayerLookAtMonsterBonus = 10;
    [SerializeField]
    float minTimeToLookAtAgent = 1.5f;
    [SerializeField]
    float minAwarenessIncrease = 1;
    [SerializeField]
    float minAwarenessDecrease = 0.25f;
    [SerializeField]
    float lookAtTimeMinIncrease = 0.5f, lookAtTimeMinDecrease = 0.1f;
    [SerializeField]
    bool playerHasSeenTheAgentBefore = false;
    [SerializeField]
    public bool monsterInSight = false;
    [SerializeField]
    bool tutorialDone = true;
    [SerializeField]
    private float timeLookingAtMonster = 0f;
    [SerializeField]
    int minDistanceToPlayer = 1, maxDistanceToPlayer = 100, movementUnit = 5;
    int fixedUpdateCounter = 0;
    public int monsterViewAttentions = 4;
    public UnityEvent<bool> playerSpottede;
    public int maxRayDsitance = 500;
    public LayerMask raysLayermask;
    string playerTag;
    public int viewportMultipliere = 2, maxAwernessLevel = 100;
    // Variables for hiding spot calculation
    public float stepAngle = 5f; // Step angle for raycasting in the FOV

    void Start()
    {
        if(playerCamera == null)
            playerCamera = Camera.main;

        playerTag = player.name;
        fieldOfViewOfPlayer = playerCamera.fieldOfView * 2;
    }

    void Update()
    {
        if (!tutorialDone)
            return;

        CheckIfPlayerCanSeeMonster(); // Check if the player is currently looking at the monster
    }

    private void CheckIfPlayerCanSeeMonster()
    {
        // Check if the player is looking at the monster and update awareness
        Vector3 directionToMonster = transform.position - playerCamera.transform.position;
        float angleToMonster = Vector3.Angle(playerCamera.transform.forward, directionToMonster);

        if (angleToMonster < fieldOfViewOfPlayer / viewportMultipliere) // Within player's FOV
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, directionToMonster, out hit))
            {
                if (hit.collider.gameObject == gameObject.transform.parent.gameObject) // Ray hit the monster
                {
                    monsterInSight = true;
                    timeLookingAtMonster += lookAtTimeMinIncrease * Time.fixedDeltaTime;

                    if (timeLookingAtMonster >= minTimeToLookAtAgent)
                    {
                        IncreaseAwareness();
                    }
                }
                else 
                {
                    monsterInSight = false;
                    DecreaseAwareness();
                    timeLookingAtMonster -= lookAtTimeMinDecrease * Time.fixedDeltaTime;
                }
            }
        }
        else 
        {
            monsterInSight = false;
            DecreaseAwareness();
            timeLookingAtMonster -= lookAtTimeMinDecrease * Time.fixedDeltaTime;
        }

        timeLookingAtMonster = Mathf.Max(timeLookingAtMonster, 0f); // Ensure timeLookingAtMonster doesn't go below 0
    }

    private void IncreaseAwareness()
    {
        
        if (!playerHasSeenTheAgentBefore)
        {
            playerAwarenessLevel += firstTimeplayerLookAtMonsterBonus;
            playerHasSeenTheAgentBefore = true;
        }
        else
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float clampedDistance = Mathf.Clamp(distanceToPlayer, minDistanceToPlayer, maxDistanceToPlayer);
            float logDistanceFactor = Mathf.Log10(clampedDistance + 1);
            float awarenessIncrease = (minAwarenessIncrease + (10f / logDistanceFactor)) * Time.fixedDeltaTime;
            playerAwarenessLevel += awarenessIncrease;
            
        }

        playerAwarenessLevel = Mathf.Min(playerAwarenessLevel, maxAwernessLevel);
    }

    private void DecreaseAwareness()
    {
        if (playerHasSeenTheAgentBefore)
        {
            playerAwarenessLevel = Mathf.Max(firstTimeplayerLookAtMonsterBonus, playerAwarenessLevel - minAwarenessDecrease * Time.fixedDeltaTime);
        }
    }


    private bool CheckIfMonsterCanSeePlayer()
    {

        Vector3 directionToMonster = playerCamera.transform.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, directionToMonster, out hit, maxRayDsitance,raysLayermask))
        {
            return (hit.collider.gameObject == player);
        }


        return false; 
    }
    public bool playerinsight;

    private void FixedUpdate()
    {
        fixedUpdateCounter++;
        if (fixedUpdateCounter >= monsterViewAttentions)
        {
            fixedUpdateCounter = 0;
           playerinsight = CheckIfMonsterCanSeePlayer();
            playerSpottede.Invoke(playerinsight);
        }
    }

    void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = playerinsight ? Color.green : Color.red;
        Vector3 directionToPlayer = playerCamera.transform.position - transform.position;
        Gizmos.DrawRay(transform.position, directionToPlayer);
        Vector3 dir = (playerCamera.transform.position - transform.position).normalized *maxRayDsitance;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + transform.right*2, dir);
    }

    public int GetPlayerAwarenessLevel()
    {
        return Mathf.RoundToInt(playerAwarenessLevel);
    }
}

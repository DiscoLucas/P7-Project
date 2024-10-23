using UnityEditor;
using UnityEngine;

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
    bool monsterInSight = false;
    [SerializeField]
    bool tutorialDone = true;
    [SerializeField]
    private float timeLookingAtMonster = 0f;
    [SerializeField]
    int minDistanceToPlayer = 1, maxDistanceToPlayer = 100;

    void Start()
    {
        playerCamera = Camera.main;
        fieldOfViewOfPlayer = playerCamera.fieldOfView*2;
        // Initialize if needed
    }

    void Update()
    {
        if (!tutorialDone)
            return;

        CheckIfPlayerCanSeeMonster();
    }

    private void CheckIfPlayerCanSeeMonster()
    {
        Vector3 directionToPlayer = playerCamera.transform.position - transform.position;
        Vector3 directionToMonster = transform.position - playerCamera.transform.position; // From player to monster
        float angleToMonster = Vector3.Angle(playerCamera.transform.forward, directionToMonster); // Angle from where player is looking to the monste

        // Check if the player is within the field of view
        if (angleToMonster < fieldOfViewOfPlayer / 2f)
        {

            RaycastHit hit;
            // Cast a ray from the monster to the player to see if there's anything blocking the view
            if (Physics.Raycast(transform.position, directionToPlayer, out hit))
            {
                if (hit.collider.gameObject == player)
                {
                    // Player is looking at the monster without obstruction
                    monsterInSight = true;
                    timeLookingAtMonster += lookAtTimeMinIncrease*Time.fixedDeltaTime;

                    if (timeLookingAtMonster >= minTimeToLookAtAgent)
                    {
                        IncreaseAwareness();
                    }
                }
                else
                {
                    // Something is blocking the player's view
                    monsterInSight = false;
                    DecreaseAwareness();
                    timeLookingAtMonster -= lookAtTimeMinDecrease * Time.fixedDeltaTime;
                }
            }
        }
        else
        {
            // Player is not looking in the direction of the monster
            monsterInSight = false;
            DecreaseAwareness();
            timeLookingAtMonster -= lookAtTimeMinDecrease * Time.fixedDeltaTime;
        }
        timeLookingAtMonster = Mathf.Max(timeLookingAtMonster, 0f);
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
        playerAwarenessLevel = Mathf.Min(playerAwarenessLevel, 100);
    }


    private void DecreaseAwareness()
    {
        if (playerHasSeenTheAgentBefore)
        {
            playerAwarenessLevel = Mathf.Max(firstTimeplayerLookAtMonsterBonus, playerAwarenessLevel - minAwarenessIncrease * Time.fixedDeltaTime);
        }
    }

    void OnDrawGizmos()
    {
        if (player == null) return;
            
        if(monsterInSight)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Vector3 directionToPlayer = playerCamera.transform.position - transform.position;
        Gizmos.DrawRay(transform.position, directionToPlayer);

    }

    public int getPlayerAwarenessLevel() {
        return Mathf.RoundToInt(playerAwarenessLevel);
    }

}

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
    int minDistanceToPlayer = 1, maxDistanceToPlayer = 100, movementUnit = 5;

    // Variables for hiding spot calculation
    public float maxRayDistance = 50f; // Max distance to check for a hiding spot
    public float stepAngle = 5f; // Step angle for raycasting in the FOV

    void Start()
    {
        playerCamera = Camera.main;
        fieldOfViewOfPlayer = playerCamera.fieldOfView * 2;
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
        Vector3 directionToMonster = transform.position - playerCamera.transform.position;
        float angleToMonster = Vector3.Angle(playerCamera.transform.forward, directionToMonster);

        if (angleToMonster < fieldOfViewOfPlayer / 2f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit))
            {
                if (hit.collider.gameObject == player)
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

    public Vector3 FindHidingSpot()
    {
        Vector3 hidingSpot = Vector3.zero;
        bool spotFound = false;
        for (float angle = -fieldOfViewOfPlayer / 2; angle <= fieldOfViewOfPlayer / 2; angle += stepAngle)
        {
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * playerCamera.transform.forward;

            if (Physics.Raycast(playerCamera.transform.position, direction, out RaycastHit hit, maxRayDistance))
            {
                if (hit.collider != null && hit.collider.gameObject != player && hit.collider.gameObject != gameObject)
                {
                    Vector3 directionToHit = hit.point - transform.position;
                    if (!Physics.Raycast(transform.position, directionToHit, maxRayDistance))
                    {
                        hidingSpot = hit.point;
                        spotFound = true;
                        break;
                    }
                }
            }
        }

        if (!spotFound)
        {
            hidingSpot = transform.position + (Random.Range(0, 2) == 0 ? -player.transform.right : player.transform.right) * movementUnit; 
        }

        return hidingSpot;
    }

    void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = monsterInSight ? Color.green : Color.red;
        Vector3 directionToPlayer = playerCamera.transform.position - transform.position;
        Gizmos.DrawRay(transform.position, directionToPlayer);
    }

    public int GetPlayerAwarenessLevel()
    {
        return Mathf.RoundToInt(playerAwarenessLevel);
    }
}

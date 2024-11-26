using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
[DefaultExecutionOrder(3)]
public class AgressionSensor : MonoBehaviour
{
    public int aggressionLevel = 0;
    public PlayerAwarenessSensor playerAwarenessSensor;
    public float timeWhenStart;
    [Tooltip("In Minutes")]
    public float maxTime = 10;
    public int updateRate = 4, currentTick;
    private NavMeshPath path;
    public Transform protectionArea;
    public int maxAggressionLevel = 100;
    public float lastDistanceToProtectionPoint = float.MaxValue;
    public int distanceEffect = 10;
    public MonsterConfig monsterConfig;

    private float boostEndTime = 0;
    private float activeBoostAmount = 0f;

    public float awarenessWeight = 0.3f;
    public float distanceWeight = 0.2f;
    public float timeWeight = 0.5f;

    private void Start()
    {
        path = new NavMeshPath();
        maxTime *= 60;
        timeWhenStart = Time.realtimeSinceStartup;
        
        
    }

    private void FixedUpdate()
    {
        currentTick += 1;
        if (updateRate <= currentTick)
        {
            currentTick = 0;
            CalcAggressionLevel();
        }
    }

    public void CalcAggressionLevel()
    {
        Vector3 start = protectionArea.position;
        Vector3 playerPosition = playerAwarenessSensor.player.transform.position;

        NavMesh.CalculatePath(start, playerPosition, NavMesh.AllAreas, path);
        float distanceToProtectionPoint = 0;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            distanceToProtectionPoint += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        if (distanceToProtectionPoint == 0)
            distanceToProtectionPoint = lastDistanceToProtectionPoint;

        lastDistanceToProtectionPoint = distanceToProtectionPoint;
        float elapsedTime = Time.realtimeSinceStartup - timeWhenStart;
        float distanceToMonster = Vector3.Distance(transform.position, playerPosition);
        if (distanceToMonster <= monsterConfig.distanceToMonsterForStartChaseAlways)
        {
            aggressionLevel = maxAggressionLevel;
            return;
        }

        // Adjusted factors with weighting
        float awarenessFactor = (Mathf.Max(1, playerAwarenessSensor.playerAwarenessLevel) / playerAwarenessSensor.maxAwernessLevel) * awarenessWeight;
        float distanceFactor = (distanceEffect / (distanceToProtectionPoint + 1)) * distanceWeight;
        float timeFactor = (elapsedTime / maxTime) * timeWeight;

        // Calculate final aggression based on weighted factors
        float calculatedAggression = (awarenessFactor + distanceFactor + timeFactor) * maxAggressionLevel;

        // Apply boost if active, decaying over time
        float boostFactor = Mathf.Clamp01((boostEndTime - Time.realtimeSinceStartup) / (boostEndTime - timeWhenStart));
        aggressionLevel = Mathf.Min(maxAggressionLevel, Mathf.RoundToInt(calculatedAggression + (activeBoostAmount * boostFactor)));
    }

    public void BoostAggression(int boostAmount, float duration)
    {
        activeBoostAmount = boostAmount;
        boostEndTime = Time.realtimeSinceStartup + duration;
    }
}

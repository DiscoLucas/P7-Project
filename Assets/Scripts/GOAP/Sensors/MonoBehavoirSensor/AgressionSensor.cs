using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class AgressionSensor : MonoBehaviour
{
    public int aggressionLevel = 0;
    public PlayerAwarenessSensor playerAwarenessSensor;
    public float timewhenStart;
    [Tooltip("In Minutes")]
    public float maxtime = 10;
    public int updateRate = 4, currentTick;
    private NavMeshPath path;
    public Transform protectionsArea;
    public int maxAgressionLevel = 100;
    public float lastDistanceToProtectionPoint = float.MaxValue;
    public int distanceEffect = 10;
    private void Start()
    {
        path = new NavMeshPath();
        maxtime *= 60;
        timewhenStart = Time.realtimeSinceStartup;
    }

    private void FixedUpdate()
    {
        currentTick += 1;
        if (updateRate <= currentTick) {
            currentTick = 0;
            calcAgressionLevel();
        }
    }

    public void calcAgressionLevel() {
        Vector3 start = protectionsArea.position;
        Vector3 end = playerAwarenessSensor.player.transform.position;
        NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path);
        float distanceToProtectionPoint = 0;
        for (int i = 0; i < path.corners.Length - 1; i++) {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            distanceToProtectionPoint += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        if(distanceToProtectionPoint == 0)
            distanceToProtectionPoint = lastDistanceToProtectionPoint;

        lastDistanceToProtectionPoint = distanceToProtectionPoint;
        float elsapedTime = Time.realtimeSinceStartup - timewhenStart;
        float al =
            (Mathf.Max(1, playerAwarenessSensor.playerAwarenessLevel) / playerAwarenessSensor.maxAwernessLevel)
            * (distanceEffect / (distanceToProtectionPoint + 1))
            * (elsapedTime / maxtime)
            * maxAgressionLevel;
        //Debug.Log("Agression level: " + al + " distance: " + distanceToProtectionPoint + " time: " + elsapedTime);
        aggressionLevel = Mathf.Min(maxAgressionLevel, Mathf.RoundToInt(al));
    }
}

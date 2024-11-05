using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerPositionMapTracker : MonoBehaviour
{
    public GameObject player;
    public Transform agentPos;
    public PlayerPositionMap playerPositionMap; // Reference to the map
    public float logInterval = 0.5f;

    [Tooltip("Maximum recent positions to track for gameplay")]
    public int maxRecentPositions = 20;
    bool dynamicBuffer = true;
    private float timer;

    [Tooltip("List of recent positions the player has visited for active game usage")]
    public List<Vector3> recentPlayerPositions = new List<Vector3>();
    public Color gizmoColor, smellAreaGizmoColor;
    public bool enableLoggin = false;
    public int fixedUpdateCounterUpdatePos = 0, fixedUpdateCounterSummerize = 0, updateRate = 4, summariePostionTime = 2;

    public Vector3 summriePostion = Vector3.zero;
    public float averageDistance = 0;
    public bool haveSummedPostion;
    public UnityEvent<Vector3, float> playerPostionsSummeries;
    private void Start()
    {
        playerPositionMap.enableLogging = enableLoggin;
        summariePostionTime *= 50;
        if (dynamicBuffer) {
            maxRecentPositions = summariePostionTime/updateRate;
        }
    }




    private void FixedUpdate()
    {
        fixedUpdateCounterUpdatePos++;
        fixedUpdateCounterSummerize++;
        if (fixedUpdateCounterSummerize >= summariePostionTime) {
            Vector3 avgPos = Vector3.zero;
            haveSummedPostion = true;


            float dist = 0;
            for (int i = 0; i < recentPlayerPositions.Count; i++) {
                avgPos += recentPlayerPositions[i];
            }

            avgPos /= recentPlayerPositions.Count;
            summriePostion = avgPos;

            foreach (var pos in recentPlayerPositions)
            {
                dist += Vector3.Distance(summriePostion, pos);
            }

            dist /= recentPlayerPositions.Count;

            averageDistance = dist;
            fixedUpdateCounterSummerize = 0;
            playerPostionsSummeries.Invoke(avgPos, averageDistance);
        }
        if (fixedUpdateCounterUpdatePos >= updateRate)
        {
            fixedUpdateCounterUpdatePos = 0;
            Vector3 npos = new Vector3(
               player.transform.position.x,
               player.transform.position.y,
               player.transform.position.z
            );
            addPosition(npos);
        }

        
    }

    public void addPosition(Vector3 position)
    {

        if (recentPlayerPositions.Count >= maxRecentPositions)
        {
            recentPlayerPositions.RemoveAt(0); 
        }

        recentPlayerPositions.Add(position);
        playerPositionMap.addPosition(position);

    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < recentPlayerPositions.Count; i++)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(recentPlayerPositions[i], Vector3.one);
        }
        Gizmos.color = smellAreaGizmoColor;
        Gizmos.DrawSphere(summriePostion, averageDistance);
    }


}

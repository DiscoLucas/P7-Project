using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SessionLog", menuName = "Tracking/SessionAnalytics")]
public class SessionLog : ScriptableObject
{
    [Tooltip("Log all player positions for debugging or analytics")]
    public bool enableLogging = false;

    [Tooltip("Detailed log of all player positions if logging is enabled")]
    public List<Vector3> allPlayerLoggedPositions = new List<Vector3>();

    [Tooltip("Detailed log of all monster positions if logging is enabled")]
    public List<Vector3> allMonsterLoggedPositions = new List<Vector3>();

    public bool playerDied = false;
    public bool gameWasCompletede = false;

    private DateTime startTime;
    private DateTime endTime;

    [Tooltip("Total time played in seconds")]
    public float timePlayed = 0f;

    public void startSession()
    {
        startTime = DateTime.Now;
        Debug.Log("Session started at: " + startTime);
    }

    public void endSession()
    {
        endTime = DateTime.Now;
        timePlayed = (float)(endTime - startTime).TotalSeconds;
        Debug.Log("Session ended at: " + endTime);
        Debug.Log("Total time played: " + timePlayed + " seconds");
    }

    public void updatePlayerDied()
    {
        playerDied = true;
    }

    public void gameCompletede()
    {
        gameWasCompletede = true;
    }

    public void addPosition(Vector3 playerPosition, Vector3 monsterPosition)
    {
        if (enableLogging)
        {
            allPlayerLoggedPositions.Add(playerPosition);
            allMonsterLoggedPositions.Add(monsterPosition);
        }
    }
}

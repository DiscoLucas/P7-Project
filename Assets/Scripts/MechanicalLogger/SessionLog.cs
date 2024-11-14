using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SessionLog
{
    public SessionLog(string name) {
        this.name = name;
    }
    public string name;

    [Tooltip("Detailed log of all player positions if logging is enabled")]
    public List<Vector3> allPlayerLoggedPositions = new List<Vector3>();

    [Tooltip("Detailed log of all monster positions if logging is enabled")]
    public List<Vector3> allMonsterLoggedPositions = new List<Vector3>();

    public int timesDied;
    public bool gameWasCompletede = false;

    private DateTime startTime;
    private DateTime endTime;

    [Tooltip("Total time played in seconds")]
    public float timePlayed = 0f;
    public List<int> deathIndexs;

    public void startSession()
    {
        startTime = DateTime.Now;
        Debug.Log("Session started at: " + startTime);
    }

    public void endSession()
    {
        endTime = DateTime.Now;
        float timePlayed = (float)(endTime - startTime).TotalSeconds;
        int minutes = (int)(timePlayed / 60);
        int seconds = (int)(timePlayed % 60);

        string timePlayedString = string.Format("{0}:{1:D2}", minutes, seconds);
        Debug.Log("Session ended at: " + endTime);
        Debug.Log("Total time played: " + timePlayedString );
    }

    public void updatePlayerDied()
    {
        deathIndexs.Add(allMonsterLoggedPositions.Count - 1);
        timesDied += 1;
        SessionLogTracker.Instance.state.sessionStarted = false;
    }

    public void gameCompletede()
    {
        gameWasCompletede = true;
    }

    public void addPosition(Vector3 playerPosition, Vector3 monsterPosition)
    {
        allPlayerLoggedPositions.Add(playerPosition);
        allMonsterLoggedPositions.Add(monsterPosition);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SessionLog
{

    public string name;

    [Tooltip("Detailed log of all player positions if logging is enabled")]
    public List<Vector3> allPlayerLoggedPositions = new List<Vector3>();

    [Tooltip("Detailed log of all monster positions if logging is enabled")]
    public List<Vector3> allMonsterLoggedPositions = new List<Vector3>();

    public List<string> addtionalLogginInfomation = new List<string>();

    public int timesDied;
    public bool gameWasCompletede = false;

    
    private DateTime startTime;
    private DateTime endTime;

    [Tooltip("Total time played in seconds")]
    public string timePlayed = "None";
    public List<int> deathIndexs = new List<int>();

    public SessionLog(string name)
    {
        this.name = name;
        
    }

    public void startSession()
    {
        startTime = DateTime.Now;
        Debug.Log("Session started at: " + startTime);
    }

    public void endSession()
    {
        
        string timePlayedString = timeFromStart();
        
        Debug.Log("Total time played: " + timePlayedString );
        this.timePlayed = timePlayedString;
    }

    public string timeFromStart()
    {
        endTime = DateTime.Now;
        double timePlayed = (endTime - startTime).TotalSeconds;
        int minutes = (int)(timePlayed / 60);
        int seconds = (int)(timePlayed % 60);
        int milliseconds = (int)((timePlayed - Math.Floor(timePlayed)) * 1000);

        string timePlayedString = string.Format("{0}:{1:D2}:{2:D3}", minutes, seconds, milliseconds);
        Debug.Log("Session at: " + endTime);
        return timePlayedString;
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
        if (allPlayerLoggedPositions.Count < 1) {
            deathIndexs.Add(0);
        }
        string additionalInfo = timeFromStart() + ";";
        if (GameManager.Instance != null)
            additionalInfo += GameManager.Instance.State.ToString();
        addtionalLogginInfomation.Add(additionalInfo);
        allPlayerLoggedPositions.Add(playerPosition);
        allMonsterLoggedPositions.Add(monsterPosition);

    }
}

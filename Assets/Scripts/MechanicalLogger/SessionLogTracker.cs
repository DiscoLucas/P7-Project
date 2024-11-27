using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
//using System.IO.Ports;

public class SessionLogTracker: SingletonPersistent<SessionLogTracker>
{
    public GameObject player;
    public Transform agentPos;
    public SessionLog sessionLog = null;
    //public SerialPort serialPort;
    public float logInterval = 0.5f;
    
    private string loggerFolder = "Logs";
    [Tooltip("Maximum recent positions to track for gameplay")]
    public int maxRecentPositions = 20;
    bool dynamicBuffer = true;
    private float timer;

    [Tooltip("List of recent positions the player has visited for active game usage")]
    public List<Vector3> recentPlayerPositions = new List<Vector3>();
    public Color gizmoColor, smellAreaGizmoColor;
    public int fixedUpdateCounterUpdatePos = 0, fixedUpdateCounterSummerize = 0, updateRate = 4, summariePostionTime = 2;

    public Vector3 summriePostion = Vector3.zero;
    public float averageDistance = 0;
    public bool haveSummedPostion;
    public UnityEvent<Vector3, float> playerPostionsSummeries;
    public SessionLogState state;

    string portName = "COM3";
    int baudRate = 9600;
    public void setGameVariables() { 
        player = GameManager.Instance.playerObject;
        agentPos = GameManager.Instance.monsterObject.transform;
        state = GameObject.FindAnyObjectByType<SessionLogState>();

    }


    private void Start()
    {
        sessionLog = null;
        if(GameManager.Instance != null)
            GameManager.Instance.onGameStartEvent.AddListener(setGameVariables);
        summariePostionTime *= 50;
        if (dynamicBuffer) {
            maxRecentPositions = summariePostionTime/updateRate;
        }

    }
    public void onSessionStarted() {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        if (agentPos == null)
            agentPos = GameObject.FindGameObjectWithTag("Monner").transform;

    }


    private void FixedUpdate()
    {
        if (state != null) {
            if (state.sessionStarted) { 
                if (!state.onSessionStart) {
                    onSessionStarted();
                    state.onSessionStart = true;
                }
                fixedUpdateCounterUpdatePos++;
                fixedUpdateCounterSummerize++;
                if (fixedUpdateCounterSummerize >= summariePostionTime)
                {
                    Vector3 avgPos = Vector3.zero;
                    haveSummedPostion = true;

                    

                    float dist = 0;
                    for (int i = 0; i < recentPlayerPositions.Count; i++)
                    {
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
                    Vector3 player_npos = new Vector3(
                       player.transform.position.x,
                       player.transform.position.y,
                       player.transform.position.z
                    );

                    Vector3 monster_npos = new Vector3(
                       agentPos.position.x,
                       agentPos.position.y,
                       agentPos.transform.position.z
                    );
                    addPosition(player_npos, monster_npos);
                }
            }
        } 
    }

    public void addPosition(Vector3 player_npos, Vector3 monster_npos)
    {
        
        if (recentPlayerPositions.Count >= maxRecentPositions)
        {
            recentPlayerPositions.RemoveAt(0); 
        }

        recentPlayerPositions.Add(player_npos);
        if (sessionLog != null)
            sessionLog.addPosition(player_npos, monster_npos);
        else
            Debug.LogWarning("The Session log have not been createde");
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

    [ContextMenu("End Session And SaveAs CSV")]
    public void EndSessionAndSaveAsCSV()
    {
        Debug.Log("Session log is null: " + (sessionLog == null));

        if (sessionLog != null)
        {
            
            state.sessionStarted = false;
            sessionLog.endSession();
            ExportSessionLogToCSV(sessionLog);
            sessionLog = null;
        }
    }



    public void startLoggin()
    {
        state.sessionStarted = true;

    }

    public void ExportSessionLogToCSV(SessionLog sessionLog)
    {
        if (sessionLog == null)
        {
            Debug.LogError("SessionLog is null. Cannot export.");
            return;
        }

        // Define the output folder and file path
        string basePath = Path.GetFullPath(Path.Combine(Application.dataPath, "..")); // Save next to the executable
        string folderPath = Path.Combine(basePath, loggerFolder);
        string filePath = Path.Combine(folderPath, sessionLog.name + ".csv");

        try
        {
            // Ensure the directory exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            Debug.Log($"Exporting session log to: {filePath}");

            // Write session log to the CSV file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write session information
                writer.WriteLine($"Session Name: {sessionLog.name}");
                writer.WriteLine($"Total Time Played (seconds): {sessionLog.timePlayed}");
                writer.WriteLine($"Player Died: {sessionLog.timesDied}");
                writer.WriteLine($"Game Completed: {sessionLog.gameWasCompletede}");
                writer.WriteLine("----");
                writer.WriteLine("Player Position X;Player Position Y;Player Position Z;Monster Position X;Monster Position Y;Monster Position Z;CurrentDeath");

                int currentDeathIndex = 0;

                for (int i = 0; i < sessionLog.allPlayerLoggedPositions.Count; i++)
                {
                    // Update the current death index
                    if (currentDeathIndex < sessionLog.deathIndexs.Count - 1 && sessionLog.deathIndexs[currentDeathIndex + 1] == i)
                    {
                        currentDeathIndex++;
                    }

                    // Write player and monster positions
                    Vector3 playerPos = sessionLog.allPlayerLoggedPositions[i];
                    Vector3 monsterPos = sessionLog.allMonsterLoggedPositions[i];
                    int currentDeath = sessionLog.deathIndexs.Count > currentDeathIndex ? sessionLog.deathIndexs[currentDeathIndex] : -1;

                    string line = $"{playerPos.x};{playerPos.y};{playerPos.z};{monsterPos.x};{monsterPos.y};{monsterPos.z};{currentDeath}";
                    writer.WriteLine(line);
                }
            }

            Debug.Log($"SessionLog successfully exported to CSV at: {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to export SessionLog to CSV. Error: {ex.Message}");
        }
    }

    public string newSessionlogName = "NewSessionLog";
    public string sessionLogFolderName = "SessionLogs";
    public SessionLog createSessionLog()
    {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        SessionLog newMap = new SessionLog($"{newSessionlogName}_{timestamp}");
        Debug.Log("New session log created in memory with name: " + newMap.name);

        return newMap;
    }

    protected override void onDuplicateInstanceDestroyed()
    {
        SessionLogTracker.Instance.recentPlayerPositions.Clear();

    }



    public string CreateFilePath(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename) || filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new ArgumentException("Invalid filename provided.");
        }

        // Navigate to the directory containing the executable
        string basePath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

        // Combine with the desired folder
        string folderPath = Path.Combine(basePath, sessionLogFolderName);

        // Ensure the directory exists
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Construct the full file path
        string filePath = Path.Combine(folderPath, $"{filename}.csv");

        Debug.Log($"Generated File Path: {filePath}");
        return filePath;
    }



}

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SessionLogImporter
{
  /*  public static SessionLog LoadSessionLogFromCSV(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("CSV file not found: " + filePath);
            return null;
        }

        SessionLog newLog = ScriptableObject.CreateInstance<SessionLog>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            // Read session information
            newLog.name = reader.ReadLine()?.Split(':')[1].Trim();
            newLog.timePlayed = float.Parse(reader.ReadLine()?.Split(':')[1].Trim());
            //newLog.playerDied = bool.Parse(reader.ReadLine()?.Split(':')[1].Trim());
            newLog.gameWasCompletede = bool.Parse(reader.ReadLine()?.Split(':')[1].Trim());

            reader.ReadLine(); // Skip empty line
            reader.ReadLine(); // Skip headers

            // Read positions
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(';');
                if (values.Length >= 6)
                {
                    Vector3 playerPos = new Vector3(
                        float.Parse(values[0]),
                        float.Parse(values[1]),
                        float.Parse(values[2])
                    );

                    Vector3 monsterPos = new Vector3(
                        float.Parse(values[3]),
                        float.Parse(values[4]),
                        float.Parse(values[5])
                    );

                    newLog.allPlayerLoggedPositions.Add(playerPos);
                    newLog.allMonsterLoggedPositions.Add(monsterPos);
                }
            }
        }

        Debug.Log("SessionLog loaded from CSV: " + newLog.name);
        return newLog;
    }*/
}


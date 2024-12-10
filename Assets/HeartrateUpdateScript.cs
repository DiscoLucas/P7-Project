using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class HeartrateUpdateScript : MonoBehaviour
{
    public string sessionLogFolderPath = @"C:\Users\Christian\Downloads\Sessionlog-20241210T124909Z-001\Sessionlog";
    private string debugLogPath;

    private void Start()
    {
        // Set up debug log file
        debugLogPath = Path.Combine(sessionLogFolderPath, "deb.log");
        File.WriteAllText(debugLogPath, "Debug Log:\n"); // Clear or create the log file

        // Get all participant folders
        var participantFolders = Directory.GetDirectories(sessionLogFolderPath);

        foreach (var folder in participantFolders)
        {
            // Get `out.csv` and session log file
            string[] csvFiles = Directory.GetFiles(folder, "*.csv");
            string outCsvFile = csvFiles.FirstOrDefault(file => file.Contains("out"));
            string sessionLogFile = csvFiles.FirstOrDefault(file => !file.Contains("_baseline") && !file.Contains("_gameData") && !file.Contains("out"));

            if (outCsvFile == null || sessionLogFile == null)
            {
                LogDebug($"Skipping folder {folder} due to missing files.");
                continue;
            }

            // Parse files
            List<LogEntry> sessionLogs = ParseSessionLog(sessionLogFile);
            List<HeartRateEntry> heartRateEntries = ParseHeartRate(outCsvFile);

            if (sessionLogs.Count == 0 || heartRateEntries.Count == 0)
            {
                LogDebug($"No valid data in folder {folder}");
                continue;
            }

            // Perform visibility checks and update heart rate data
            UpdateHeartRateWithVisibility(heartRateEntries, sessionLogs);

            // Save updated heart rate data to spothr.csv
            SaveUpdatedHeartRate(folder, heartRateEntries);
        }
    }

    private List<LogEntry> ParseSessionLog(string sessionLogFile)
    {
        var logs = new List<LogEntry>();
        var lines = File.ReadAllLines(sessionLogFile);
        foreach (var line in lines)
        {
            if (!line.StartsWith("0:")) continue; // Skip header or irrelevant lines
            var parts = line.Split(';');
            try
            {
                var log = new LogEntry
                {
                    Time = ParseTime(parts[0]),
                    PlayerPosition = new Vector3(float.Parse(parts[2]), float.Parse(parts[3]), float.Parse(parts[4])),
                    MonsterPosition = new Vector3(float.Parse(parts[5]), float.Parse(parts[6]), float.Parse(parts[7]))
                };
                logs.Add(log);
            }
            catch (Exception ex)
            {
                LogDebug($"Failed to parse session log entry '{line}': {ex.Message}");
            }
        }
        return logs;
    }

    private List<HeartRateEntry> ParseHeartRate(string outCsvFile)
    {
        var entries = new List<HeartRateEntry>();
        var lines = File.ReadAllLines(outCsvFile).SkipWhile(line => !line.StartsWith("# Cleaned Data")).Skip(2); // Skip to cleaned data section
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            try
            {
                var entry = new HeartRateEntry
                {
                    Timestamp = ParseTime(parts[0]),
                    HeartRate = float.Parse(parts[2])
                };
                entries.Add(entry);
            }
            catch (Exception ex)
            {
                LogDebug($"Failed to parse heart rate entry '{line}': {ex.Message}");
            }
        }
        return entries;
    }

    private void UpdateHeartRateWithVisibility(List<HeartRateEntry> heartRates, List<LogEntry> logs)
    {
        foreach (var heartRate in heartRates)
        {
            try
            {
                // Find the closest log entry based on timestamp
                var closestLog = logs.OrderBy(log => Math.Abs((log.Time - heartRate.Timestamp).TotalMilliseconds)).First();

                // Ray origin and direction
                Vector3 rayOrigin = closestLog.PlayerPosition;
                Vector3 direction = closestLog.MonsterPosition - closestLog.PlayerPosition;
                float distance = direction.magnitude;

                // Debugging ray properties
                LogDebug($"Ray Origin: {rayOrigin}, Direction: {direction}, Distance: {distance}");

                // Check visibility using Physics.Raycast
                bool isVisible = !Physics.Raycast(rayOrigin, direction.normalized, distance);
                heartRate.MonsterVisible = isVisible ? 1 : 0;

                // Log the result for debugging
                LogDebug($"Timestamp: {heartRate.Timestamp}, Monster Visible: {heartRate.MonsterVisible}");
            }
            catch (Exception ex)
            {
                LogDebug($"Error updating visibility for heart rate entry at {heartRate.Timestamp}: {ex.Message}");
            }
        }
    }


    private void SaveUpdatedHeartRate(string folder, List<HeartRateEntry> heartRates)
    {
        string outputFilePath = Path.Combine(folder, "spothr.csv");
        var lines = new List<string>
        {
            "# Cleaned Data",
            "Timestamp,HeartRate,MonsterVisible"
        };

        lines.AddRange(heartRates.Select(hr => $"{FormatTimeSpan(hr.Timestamp)},{hr.HeartRate},{hr.MonsterVisible}"));

        File.WriteAllLines(outputFilePath, lines);

        LogDebug($"Saved updated heart rate data to {outputFilePath}");
    }

    private void LogDebug(string message)
    {
        Debug.Log(message);
        File.AppendAllText(debugLogPath, $"{message}\n");
    }

    private string FormatTimeSpan(TimeSpan timeSpan)
    {
        // Custom format for TimeSpan
        return $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Milliseconds:D3}";
    }

    private TimeSpan ParseTime(string timestamp)
    {
        try
        {
            var parts = timestamp.Split(':').Select(p => p.Trim().Split('.').FirstOrDefault()).ToArray();

            // Handle hours, minutes, seconds, and milliseconds
            int minutes = int.Parse(parts[0]);
            int seconds = int.Parse(parts[1]);
            int milliseconds = parts.Length > 2 ? int.Parse(parts[2]) : 0;
            TimeSpan timeSpan = new TimeSpan(0, 0, minutes, seconds, milliseconds);
            return timeSpan;
        }
        catch (Exception ex)
        {
            LogDebug($"Failed to parse timestamp '{timestamp}': {ex.Message}");
            return TimeSpan.Zero; // Return a default value to avoid breaking execution
        }
    }

    private class LogEntry
    {
        public TimeSpan Time { get; set; }
        public Vector3 PlayerPosition { get; set; }
        public Vector3 MonsterPosition { get; set; }
    }

    private class HeartRateEntry
    {
        public TimeSpan Timestamp { get; set; }
        public float HeartRate { get; set; }
        public int MonsterVisible { get; set; }
    }
}

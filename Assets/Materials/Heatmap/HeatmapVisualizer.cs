using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class ParticipantPosition
{
    public string Name; // The name of the session folder
    public List<Vector3> PlayerPositions = new List<Vector3>();
    public List<Vector3> MonsterPositions = new List<Vector3>();
}

[RequireComponent(typeof(LineRenderer))]
public class HeatmapVisualizer : MonoBehaviour
{
    public string sessionLogFolderPath = @"C:\Users\Christian\Downloads\Sessionlog-20241210T124909Z-001\Sessionlog";
    public string groupFilePath = @"C:\Users\Christian\Downloads\Sessionlog-20241210T124909Z-001\Group.csv";
    public string screenshotsFolder = @"C:\Users\Christian\Downloads\HeatmapScreenshots";
    public float minLineWidth = 0.1f;
    public float maxLineWidth = 0.5f;
    public float roundingSensitivity = 1.0f;
    public float colorSensitivity = 1.0f;

    public List<ParticipantPosition> Participants = new List<ParticipantPosition>();
    private LineRenderer templateLineRenderer;

    public void Start()
    {
        templateLineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(GenerateHeatmaps());
        StartCoroutine(GenerateGroupPaths());
    }

    public IEnumerator GenerateHeatmaps()
    {
        Participants = LoadSessionLogs();
        int totalParticipants = Participants.Count;

        Directory.CreateDirectory(screenshotsFolder);

        for (int i = 0; i < totalParticipants; i++)
        {
            var participant = Participants[i];
            GameObject go = CreateHeatmapForParticipant(participant, true);
            yield return null;
            TakeScreenshot(participant.Name + "_Player_Heatmap.png");
            yield return null;
            go.SetActive(false);
            yield return null;
            go = CreateHeatmapForParticipant(participant, false);
            yield return null;
            TakeScreenshot(participant.Name + "_Monster_Heatmap.png");
            yield return null;
            go.SetActive(false);
            yield return null;
            Debug.Log($"Generating heatmaps: {i + 1}/{totalParticipants} participants processed.");
        }

        Debug.Log("Heatmap generation complete!");
    }

    private void TakeScreenshot(string fileName)
    {
        string filePath = Path.Combine(screenshotsFolder, fileName);
        ScreenCapture.CaptureScreenshot(filePath);
        Debug.Log($"Screenshot saved to: {filePath}");
    }

    private GameObject CreateHeatmapForParticipant(ParticipantPosition participant, bool isPlayer)
    {
        string objectName = isPlayer ? participant.Name : $"{participant.Name}_Monster";
        GameObject participantObject = new GameObject(objectName);
        participantObject.transform.parent = this.transform;
        LineRenderer participantLineRenderer = participantObject.AddComponent<LineRenderer>();
        CopyLineRendererSettings(templateLineRenderer, participantLineRenderer);
        List<Vector3> positions = isPlayer ? participant.PlayerPositions : participant.MonsterPositions;
        Dictionary<Vector3, int> positionVisitCount = new Dictionary<Vector3, int>();
        foreach (Vector3 position in positions)
        {
            Vector3 roundedPosition = new Vector3(
                Mathf.Round(position.x / roundingSensitivity) * roundingSensitivity,
                Mathf.Round(position.y / roundingSensitivity) * roundingSensitivity,
                Mathf.Round(position.z / roundingSensitivity) * roundingSensitivity
            );

            if (positionVisitCount.ContainsKey(roundedPosition))
            {
                positionVisitCount[roundedPosition]++;
            }
            else
            {
                positionVisitCount[roundedPosition] = 1;
            }
        }

        participantLineRenderer.positionCount = positionVisitCount.Count;
        int index = 0;
        float maxVal = Mathf.Max(positionVisitCount.Values.ToArray());
        foreach (var entry in positionVisitCount)
        {
            participantLineRenderer.SetPosition(index, entry.Key);

            float normalizedIntensity = Mathf.InverseLerp(1, 100, entry.Value / maxVal) * colorSensitivity;
            float lineWidth = Mathf.Lerp(minLineWidth, maxLineWidth, normalizedIntensity);

            participantLineRenderer.startWidth = lineWidth;
            participantLineRenderer.endWidth = lineWidth;

            Color color = participantLineRenderer.colorGradient.Evaluate(normalizedIntensity);
            participantLineRenderer.startColor = color;
            participantLineRenderer.endColor = color;

            index++;
        }
        return participantObject;
    }

    public IEnumerator GenerateGroupPaths()
    {
        Dictionary<string, string> groupMappings = LoadGroupMappings();
        var groupAPlayers = Participants.Where(p => groupMappings.TryGetValue(p.Name, out var group) && group == "Group A").ToList();
        var groupBPlayers = Participants.Where(p => groupMappings.TryGetValue(p.Name, out var group) && group == "Group B").ToList();
        Directory.CreateDirectory(screenshotsFolder);

        //player 
        string name = "GroupA_Player";
        GameObject go = CreateAveragePath(name, groupAPlayers.SelectMany(p => p.PlayerPositions).ToList());
        yield return null;
        TakeScreenshot(name + "_Path.png");
        yield return null;
        go.SetActive(false);
        yield return null;

        name = "GroupB_Player";
        go = CreateAveragePath(name, groupBPlayers.SelectMany(p => p.PlayerPositions).ToList());
        yield return null;
        TakeScreenshot(name + "_Path.png");
        yield return null;
        go.SetActive(false);
        yield return null;

        name = "GroupA_Monster";
        go = CreateAveragePath(name, groupAPlayers.SelectMany(p => p.MonsterPositions).ToList());
        yield return null;
        TakeScreenshot(name + "_Path.png");
        yield return null;
        go.SetActive(false);
        yield return null;

        name = "GroupB_Monster";
        go = CreateAveragePath(name, groupBPlayers.SelectMany(p => p.MonsterPositions).ToList());
        yield return null;
        TakeScreenshot(name + "_Path.png");
        yield return null;
        go.SetActive(false);
        yield return null;
    }

    private GameObject CreateAveragePath(string name, List<Vector3> positions)
    {
        if (positions.Count == 0)
        {
            Debug.LogWarning("No positions provided for path creation.");
            return null;
        }
        GameObject groupObject = new GameObject(name);
        groupObject.transform.parent = this.transform;
        LineRenderer lineRenderer = groupObject.AddComponent<LineRenderer>();
        CopyLineRendererSettings(templateLineRenderer, lineRenderer);
        Dictionary<Vector3, int> positionVisitCount = CountPositionVisits(positions);
        int maxFrequency = positionVisitCount.Values.Any() ? positionVisitCount.Values.Max() : 1;
        List<Vector3> sortedPositions = SortPositionsByProximity(positions);
        lineRenderer.positionCount = sortedPositions.Count;
        lineRenderer.SetPositions(sortedPositions.ToArray());
        for (int i = 0; i < sortedPositions.Count; i++)
        {
            Vector3 position = sortedPositions[i];
            int frequency = positionVisitCount[position];
            float normalizedIntensity = Mathf.InverseLerp(1, 100, frequency / maxFrequency) * colorSensitivity;

            float lineWidth = Mathf.Lerp(minLineWidth, maxLineWidth, normalizedIntensity / 100);
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;

            Color color = lineRenderer.colorGradient.Evaluate(normalizedIntensity);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }

        return groupObject;
    }

    private List<Vector3> SortPositionsByProximity(List<Vector3> positions)
    {
        List<Vector3> sortedPositions = new List<Vector3>();
        HashSet<Vector3> remainingPositions = new HashSet<Vector3>(positions);
        Vector3 currentPosition = positions.First();
        sortedPositions.Add(currentPosition);
        remainingPositions.Remove(currentPosition);
        while (remainingPositions.Count > 0)
        {
            Vector3 closestPosition = remainingPositions.OrderBy(p => Vector3.Distance(currentPosition, p)).First();
            sortedPositions.Add(closestPosition);
            remainingPositions.Remove(closestPosition);
            currentPosition = closestPosition;
        }

        return sortedPositions;
    }

    private Dictionary<Vector3, int> CountPositionVisits(List<Vector3> positions)
    {
        Dictionary<Vector3, int> positionVisitCount = new Dictionary<Vector3, int>();
        foreach (Vector3 position in positions)
        {
            if (positionVisitCount.ContainsKey(position))
            {
                positionVisitCount[position]++;
            }
            else
            {
                positionVisitCount[position] = 1;
            }
        }

        return positionVisitCount;
    }

    private Dictionary<string, string> LoadGroupMappings()
    {
        Dictionary<string, string> mappings = new Dictionary<string, string>();
        string[] lines = File.ReadAllLines(groupFilePath);

        foreach (string line in lines.Skip(1))
        {
            string[] columns = line.Split(',');
            if (columns.Length >= 2)
            {
                mappings[columns[0]] = columns[1];
            }
        }

        return mappings;
    }

    private void CopyLineRendererSettings(LineRenderer source, LineRenderer target)
    {
        target.colorGradient = source.colorGradient;
        target.startWidth = source.startWidth;
        target.endWidth = source.endWidth;
        target.material = source.material;
        target.numCapVertices = source.numCapVertices;
        target.numCornerVertices = source.numCornerVertices;
        target.useWorldSpace = source.useWorldSpace;
        target.loop = source.loop;
    }

    private List<ParticipantPosition> LoadSessionLogs()
    {
        List<ParticipantPosition> participantPositions = new List<ParticipantPosition>();

        string[] testDirectories = Directory.GetDirectories(sessionLogFolderPath, "Testie*");
        Debug.Log($"Found {testDirectories.Length} test directories.");

        foreach (string dir in testDirectories)
        {
            string[] csvFiles = Directory.GetFiles(dir, "*.csv");
            string relevantFile = csvFiles.FirstOrDefault(file => !file.Contains("_baseline") && !file.Contains("_gameData") && !file.Contains("out"));

            if (!string.IsNullOrEmpty(relevantFile))
            {
                ParticipantPosition participant = ParseCsv(relevantFile);
                participant.Name = new DirectoryInfo(dir).Name;
                participantPositions.Add(participant);
            }
        }

        return participantPositions;
    }

    private ParticipantPosition ParseCsv(string filePath)
    {
        ParticipantPosition participant = new ParticipantPosition();

        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines.Skip(6))
        {
            string[] columns = line.Split(';');
            if (columns.Length < 9) continue;

            float playerX = ParseWithSmallestValue(columns[2]);
            float playerY = ParseWithSmallestValue(columns[3]);
            float playerZ = ParseWithSmallestValue(columns[4]);

            float monsterX = ParseWithSmallestValue(columns[5]);
            float monsterY = ParseWithSmallestValue(columns[6]);
            float monsterZ = ParseWithSmallestValue(columns[7]);

            participant.PlayerPositions.Add(new Vector3(playerX, playerY, playerZ));
            participant.MonsterPositions.Add(new Vector3(monsterX, monsterY, monsterZ));
        }

        return participant;
    }

    private float ParseWithSmallestValue(string value)
    {
        float parsedInvariant = float.MaxValue;
        float parsedDanish = float.MaxValue;

        if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float resultInvariant))
        {
            parsedInvariant = resultInvariant;
        }

        if (float.TryParse(value, NumberStyles.Float, new CultureInfo("da-DK"), out float resultDanish))
        {
            parsedDanish = resultDanish;
        }

        if (parsedInvariant != float.MaxValue && parsedDanish != float.MaxValue)
        {
            return Mathf.Abs(parsedInvariant) < Mathf.Abs(parsedDanish) ? parsedInvariant : resultDanish;
        }
        else if (parsedInvariant != float.MaxValue)
        {
            return parsedInvariant;
        }
        else if (parsedDanish != float.MaxValue)
        {
            return parsedDanish;
        }

        Debug.LogError($"Failed to parse value: {value}");
        return 0;
    }

}
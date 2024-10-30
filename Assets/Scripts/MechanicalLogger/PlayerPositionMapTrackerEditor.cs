using System;
using UnityEditor;
using UnityEngine;
using System.IO; // Import this to handle directory operations

[CustomEditor(typeof(PlayerPositionMapTracker))]
public class PlayerPositionMapTrackerEditor : Editor
{
    public string loggerFolder = "Logs", filename = "PlayerPositionMap";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerPositionMapTracker tracker = (PlayerPositionMapTracker)target;

        if (GUILayout.Button("Create New Player Position Map"))
        {

            PlayerPositionMap newMap = ScriptableObject.CreateInstance<PlayerPositionMap>();

            

            
            AssetDatabase.CreateAsset(newMap, createFilepath());
            AssetDatabase.SaveAssets();
            tracker.playerPositionMap = newMap;

            EditorUtility.SetDirty(tracker);
        }
    }

    public string createFilepath() {

        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string folderPath = Path.Combine("Assets", loggerFolder);
        string filePath = Path.Combine(folderPath, $"{filename}_{timestamp}.asset");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }


        return filePath;
    }
}

using System;
using UnityEditor;
using UnityEngine;
using System.IO; // Import this to handle directory operations

[CustomEditor(typeof(SessionLogTracker))]
public class PlayerPositionMapTrackerEditor : Editor
{
    

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SessionLogTracker tracker = (SessionLogTracker)target;

        if (GUILayout.Button("Create New Player Position Map"))
        {
            tracker.sessionLog = tracker.createSessionLog();
            EditorUtility.SetDirty(tracker);
        }
    }


}

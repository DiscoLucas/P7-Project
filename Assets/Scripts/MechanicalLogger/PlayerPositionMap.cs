using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPositionMap", menuName = "Tracking/Player Position Map")]
public class PlayerPositionMap : ScriptableObject
{

    [Tooltip("Log all player positions for debugging or analytics")]
    public bool enableLogging = false;

    [Tooltip("Detailed log of all player positions if logging is enabled")]
    public List<Vector3> allLoggedPositions = new List<Vector3>();

    public void addPosition(Vector3 position)
    {
        if (enableLogging)
        {
            allLoggedPositions.Add(position);
        }
    }
}

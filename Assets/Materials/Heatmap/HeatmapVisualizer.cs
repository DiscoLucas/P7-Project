using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HeatmapVisualizer : MonoBehaviour
{
    public SessionLog map;
    public bool isPlayer = true;
    public float minLineWidth = 0.1f; 
    public float maxLineWidth = 0.5f; 

    public Gradient colorGradient;

    public float roundingSensitivity = 1.0f;

    private LineRenderer lineRenderer;
    private Dictionary<Vector3, int> positionVisitCount = new Dictionary<Vector3, int>();

    [ContextMenu("Generate Heatmap")]
    public void GenerateHeatmap()
    {
        if (map == null)
        {
            return;
        }

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        positionVisitCount.Clear();
        List<Vector3> postions;
        if (isPlayer)
            postions = map.allPlayerLoggedPositions;
        else
            postions = map.allMonsterLoggedPositions;
        foreach (Vector3 position in postions)
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

        lineRenderer.positionCount = positionVisitCount.Count;

        int index = 0;
        foreach (var entry in positionVisitCount)
        {
            lineRenderer.SetPosition(index, entry.Key);
            float normalizedIntensity = Mathf.InverseLerp(1, Mathf.Max(positionVisitCount.Values.ToArray()), entry.Value);
            lineRenderer.colorGradient = colorGradient;
            float lineWidth = Mathf.Lerp(minLineWidth, maxLineWidth, normalizedIntensity);
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            Color color = colorGradient.Evaluate(normalizedIntensity);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color; 

            index++;
        }
    }
}

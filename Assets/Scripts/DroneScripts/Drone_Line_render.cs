using System.Collections.Generic;
using UnityEngine;

public class Drone_Line_render : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    public float distanceThreshold = 0.5f; // Minimum distance between points

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        // Add points to the line when the drone has moved a certain distance
        if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], transform.position) > distanceThreshold)
        {
            points.Add(transform.position);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, transform.position);
        }
    }
}

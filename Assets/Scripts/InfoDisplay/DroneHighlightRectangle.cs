using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DroneHighlightRectangle : MonoBehaviour
{
    public float width = 0.5f;
    public float height = 0.3f;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = 5;
        lineRenderer.widthMultiplier = 0.8f; // line thickness

        // Define the rectangle corners
        Vector3[] corners = new Vector3[] {
            new Vector3(-width/2, height/2, 0),
            new Vector3(width/2, height/2, 0),
            new Vector3(width/2, -height/2, 0),
            new Vector3(-width/2, -height/2, 0),
            new Vector3(-width/2, height/2, 0)
        };

        lineRenderer.SetPositions(corners);
    }

    void Update()
    {
        // Get the main camera (the player)
        Camera cam = Camera.main;
        if (cam == null) return;

        // Make the rectangle always face the camera without tilting
        Vector3 lookDirection = cam.transform.position - transform.position;
        lookDirection.y = 0; // optional: prevents tilting up/down

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
            transform.rotation = lookRotation;
        }
    }
}

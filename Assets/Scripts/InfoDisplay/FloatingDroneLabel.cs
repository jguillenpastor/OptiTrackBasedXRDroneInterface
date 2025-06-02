using UnityEngine;
using TMPro;
using System.Net.Http;

public class FloatingDroneLabel : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public Transform targetCamera;
    public float baseFontSize = 0.05f;
    public float minDistance = 0.5f;
    public float maxDistance = 5f;
    public float scaleFactor = 0.1f;
    VRPNPoseSubscriber vrpnSubscriber;

    void Start()
    {
        vrpnSubscriber = FindObjectOfType<VRPNPoseSubscriber>();

        if (textMesh == null)
        {
            textMesh = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (targetCamera == null && Camera.main != null)
        {
            targetCamera = Camera.main.transform;
        }

        if (textMesh != null)
        {
            textMesh.alignment = TextAlignmentOptions.Center; // center the text alignment
        }
    }

    void Update()
    {
        SetText(vrpnSubscriber.GetDronePosition(), vrpnSubscriber.GetDroneVelocity());

        if (targetCamera == null || textMesh == null) return;

        // Adjust font size based on distance to camera
        float distance = Vector3.Distance(transform.position, targetCamera.position);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        float newSize = baseFontSize + (distance * scaleFactor);
        textMesh.fontSize = newSize;
    }

    // Used to update the label content from another script
    public void SetText(Vector3 position, Vector3 velocity)
    {
        string content = "";

        string line1 = $"Speed X: {velocity.x:F3} m/s \n";
        string line2 = $"Speed Y: {velocity.y:F3} m/s \n";
        string line3 = $"Speed Z: {velocity.z:F3} m/s \n";
        string line4 = $"Height: {position.y:F2} m \n";

        content = line1 + line2 + line3 + line4;

        if (textMesh != null)
        {
            textMesh.text = content;
        }
    }
}

using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using UnityEngine;
using TMPro; // Import TextMeshPro

public class DroneSubscriber : MonoBehaviour
{
    private ROSConnection ros;
    public TMP_Text statusText; // Reference to TextMeshPro UI element
    private string messages = ""; // Message accumulator
    private bool hasNewMessage = false; // Flag to detect new messages
    private readonly object lockObj = new object(); // For thread-safe access

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<StringMsg>("/unity_bridge/MRdrone/pose", StatusCallback); // Subscribe to ROS topic
    }

    void StatusCallback(StringMsg msg)
    {
        Debug.Log("Drone status: " + msg.data);

        // Lock to avoid race conditions with multithreaded callbacks
        lock (lockObj)
        {
            messages += "→ " + msg.data + "\n"; // Append received message
            hasNewMessage = true; // Set flag to update UI
        }
    }

    void Update()
    {
        // Only update the UI if there's a new message
        if (hasNewMessage)
        {
            lock (lockObj)
            {
                if (statusText != null)
                {
                    statusText.text = messages; // Display accumulated messages
                }
                hasNewMessage = false; // Reset flag
            }
        }
    }
}

using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class WaypointSubscriber : MonoBehaviour
{
    ROSConnection ros;
    public GameObject waypointPrefab; // Prefab of the sphere assigned in Unity
    private GameObject waypointParent; // Container for all waypoints
    private List<Vector3> waypointsToProcess = new List<Vector3>(); // Queue of pending waypoints
    private bool hasNewWaypoints = false; // Flag to indicate new waypoints received
    VRPNPoseSubscriber vrpnSubscriber;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<PoseArrayMsg>("/waypoints", WaypointsCallback);
        vrpnSubscriber = FindObjectOfType<VRPNPoseSubscriber>();
    }

    void WaypointsCallback(PoseArrayMsg msg)
    {
        // Debug.Log("Received " + msg.poses.Length + " waypoints.");

        // Clear the current list before adding new waypoints
        waypointsToProcess.Clear();

        for (int i = 0; i < msg.poses.Length; i++)
        {
            Vector3 position = new Vector3(
                -(float)msg.poses[i].position.y,
                (float)msg.poses[i].position.z,
                (float)msg.poses[i].position.x
            );
            waypointsToProcess.Add(position); // Add to processing queue
        }

        hasNewWaypoints = true; // Mark flag to process in next Update
    }

    void Update()
    {
        // Only create waypoints if new ones were received and headset is ready
        if (hasNewWaypoints && !vrpnSubscriber.Getonlyoneread())
        {
            CreateWaypointsInScene();
            hasNewWaypoints = false;
        }
    }

    void CreateWaypointsInScene()
    {
        // If a previous container exists, destroy it
        if (waypointParent != null)
        {
            Destroy(waypointParent);
        }

        waypointParent = new GameObject("Waypoints");

        // Adjust the waypoint position relative to the initial headset position
        for (int i = 0; i < waypointsToProcess.Count; i++)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = waypointsToProcess[i].x - vrpnSubscriber.GetHeadsetInitialPosition().x;
            newPosition.y = waypointsToProcess[i].y;
            newPosition.z = waypointsToProcess[i].z - vrpnSubscriber.GetHeadsetInitialPosition().z;

            // Instantiate the waypoint in the scene
            GameObject waypoint = Instantiate(waypointPrefab, newPosition, Quaternion.identity);
            waypoint.name = "wp" + i;
            waypoint.transform.parent = waypointParent.transform;
        }

        Debug.Log("Waypoints created in the scene.");

        // Unsubscribe after receiving and processing the waypoints
        ros.Unsubscribe("/waypoints");
    }
}

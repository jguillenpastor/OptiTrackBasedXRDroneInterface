using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using UnityEngine;

public class VRPNPoseSubscriber : MonoBehaviour
{
    private ROSConnection ros;

    private Vector3 dronePosition;
    private Quaternion droneRotation;
    private Vector3 droneVelocity;

    private Vector3 headsetPosition;
    private Vector3 headsetInitialPosition;
    private Quaternion headsetRotation;
    private DronePositionLogger dronePositionLogger;

    private bool onlyoneread = true; // Flag to ensure headset initial position is captured only once

    void Start()
    {
        Debug.Log("hello");
        ros = ROSConnection.GetOrCreateInstance();
        dronePositionLogger = GetComponent<DronePositionLogger>();

        // Subscribe to relevant ROS topics
        ros.Subscribe<PoseStampedMsg>("/unity_bridge/MRdrone/pose", DronePoseCallback);
        ros.Subscribe<PoseStampedMsg>("/unity_bridge/MRheadset/pose", HeadsetPoseCallback);
        ros.Subscribe<TwistStampedMsg>("/mavros/setpoint_velocity/cmd_vel", DroneVelocityCallback);
    }

    void DronePoseCallback(PoseStampedMsg msg)
    {
        // Convert ROS coordinates to Unity coordinates
        dronePosition = new Vector3(
            -(float)msg.pose.position.y,
            (float)msg.pose.position.z,
            (float)msg.pose.position.x
        );
        dronePositionLogger.rosPosition = dronePosition;

        droneRotation = new Quaternion(
            (float)msg.pose.orientation.x,
            (float)msg.pose.orientation.y,
            (float)msg.pose.orientation.z,
            (float)msg.pose.orientation.w
        );
    }

    void HeadsetPoseCallback(PoseStampedMsg msg)
    {
        // Capture headset initial position once
        if (onlyoneread)
        {
            Vector3 position = new Vector3(
                -(float)msg.pose.position.y,
                (float)msg.pose.position.z,
                (float)msg.pose.position.x
            );

            if (position != Vector3.zero)
            {
                Debug.Log("Initial position: " + position);
                headsetInitialPosition = position;
                dronePositionLogger.saveHeadsetInitialPos(headsetInitialPosition);
                onlyoneread = false;
            }
        }

        // Update current headset position and rotation
        headsetPosition = new Vector3(
            -(float)msg.pose.position.y,
            (float)msg.pose.position.z,
            (float)msg.pose.position.x
        );
        headsetRotation = new Quaternion(
            (float)msg.pose.orientation.x,
            (float)msg.pose.orientation.y,
            (float)msg.pose.orientation.z,
            (float)msg.pose.orientation.w
        );
    }

    void DroneVelocityCallback(TwistStampedMsg msg)
    {
        // Extract drone velocity from ROS message
        droneVelocity = new Vector3(
            (float)msg.twist.linear.y,
            (float)msg.twist.linear.x,
            (float)msg.twist.linear.z
        );
    }

    // Public methods to get drone position, rotation, and velocity
    public Vector3 GetDronePosition()
    {
        return dronePosition;
    }

    public Quaternion GetDroneRotation()
    {
        return droneRotation;
    }

    public Vector3 GetDroneVelocity()
    {
        return droneVelocity;
    }

    // Public methods to get headset position, initial position, and rotation
    public Vector3 GetHeadsetPosition()
    {
        return headsetPosition;
    }

    public Vector3 GetHeadsetInitialPosition()
    {
        return headsetInitialPosition;
    }

    public Quaternion GetHeadsetRotation()
    {
        return headsetRotation;
    }

    public bool Getonlyoneread()
    {
        return onlyoneread;
    }
}

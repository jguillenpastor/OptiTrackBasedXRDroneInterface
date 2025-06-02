using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using UnityEngine;

public class DronePublisher : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/drone/cmd_vel";

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistMsg>(topicName); // Register the publisher for Twist messages
    }

    void Update()
    {
        TwistMsg cmd = new TwistMsg();

        // Properly define all variables of the message
        cmd.linear = new Vector3Msg(1.0f, 0.0f, 0.0f); // Move only along the X axis
        cmd.angular = new Vector3Msg(0.0f, 0.0f, 0.0f); // No rotation

        ros.Publish(topicName, cmd); // Publish the message to the topic
    }
}

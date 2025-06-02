using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class DroneMovement : MonoBehaviour
{
    // Start is called before the first frame update
    VRPNPoseSubscriber vrpnSubscriber;
    public GameObject headset;

    void Start()
    {
        vrpnSubscriber = FindObjectOfType<VRPNPoseSubscriber>();
    }


    void Update()
    {
        // Update drone position relative to initial headset position
        Vector3 newPosition = transform.position;
        newPosition.x = vrpnSubscriber.GetDronePosition().x - vrpnSubscriber.GetHeadsetInitialPosition().x;
        newPosition.y = vrpnSubscriber.GetDronePosition().y;
        newPosition.z = vrpnSubscriber.GetDronePosition().z - vrpnSubscriber.GetHeadsetInitialPosition().z;
        transform.position = newPosition;


        //headset.transform.position = vrpnSubscriber.GetHeadsetPosition()- vrpnSubscriber.GetHeadsetInitialPosition();
        //Debug.Log("Current Pos:" + vrpnSubscriber.GetHeadsetPosition());
        //Debug.Log("Initial Pos:" + vrpnSubscriber.GetHeadsetInitialPosition());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : MonoBehaviour
{
    [SerializeField] public GameObject targets;             // Parent container for spawned targets
    [SerializeField] public GameObject targetSample;        // Prefab to instantiate
    [SerializeField] public GameObject targetCyinderPose;   // Reference transform for spawn position

    private int targetCounter = 1;

    void Start()
    {
        // (Optional) Initialization if needed
    }

    // Instantiates a new target at the specified pose
    void DuplicateTarget()
    {
        if (targetSample != null)
        {
            GameObject newObject = Instantiate(targetSample, targetCyinderPose.transform.position, targetCyinderPose.transform.rotation);
            newObject.name = "T" + targetCounter;
            newObject.transform.SetParent(targets.transform);
            newObject.SetActive(true);
            targetCounter++;
        }
    }

    // Deletes the most recently added target
    void DeleteTarget()
    {
        if (targets.transform.childCount != 0)
        {
            Destroy(targets.transform.GetChild(targetCounter - 2).gameObject);
            targetCounter--;
        }
    }

    void Update()
    {
        // Press A (Right Touch) to spawn a target
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            DuplicateTarget();
        }

        // Press B (Right Touch) to remove the last target
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            DeleteTarget();
        }
    }
}

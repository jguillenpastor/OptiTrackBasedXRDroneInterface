using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private GameObject Cylinder;

    private void Start()
    {
        Transform cylinderTransform = transform.Find("Cylinder");
        Cylinder = cylinderTransform.gameObject;
        Cylinder.gameObject.SetActive(false);

    }
    // This is called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        Cylinder.gameObject.SetActive(true);
        // Log when collision is detected
        Debug.Log("Trigger Entered with: " + other.gameObject.name);
    }
}
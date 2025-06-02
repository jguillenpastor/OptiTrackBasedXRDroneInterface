using UnityEngine;
using Oculus;

public class ScaleCylinder : MonoBehaviour
{
    public float growthSpeed = 1f;   // Speed at which the cylinder scales
    public float minScaleY = 0.2f;   // Minimum allowed height
    public float maxScaleY = 5f;     // (Optional) Maximum allowed height

    private float initialHeight;     // Current height of the cylinder

    void Start()
    {
        // Store the starting height of the cylinder
        initialHeight = transform.localScale.y;
    }

    void Update()
    {
        // Get input from the right thumbstick on Meta Quest
        Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // Ignore small input noise
        if (Mathf.Abs(thumbstickInput.y) > 0.1f)
        {
            // Prevent scaling below minimum unless input is increasing height
            if (initialHeight > minScaleY || thumbstickInput.y > 0)
            {
                // Calculate new height based on input and speed
                float growthAmount = initialHeight + thumbstickInput.y * growthSpeed * Time.deltaTime;
                initialHeight = growthAmount;

                // Apply the new height to the Y scale
                transform.localScale = new Vector3(transform.localScale.x, growthAmount, transform.localScale.z);
            }
        }
    }
}

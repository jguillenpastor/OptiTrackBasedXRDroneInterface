using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2.0f;

    void Update()
    {
        // Read joystick input from the right controller
        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        // Move the player object based on joystick input
        transform.position += new Vector3(joystickInput.x, 0, joystickInput.y) * speed * Time.deltaTime;

        // Check if button A was pressed
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            Debug.Log("Button A pressed");
        }
    }
}

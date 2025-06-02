using Unity.VisualScripting;
using UnityEngine;

namespace IndiePixel
{
    public class PID_Controller : MonoBehaviour
    {
        [Header("PID Settings")]
        private Vector3 targetPosition; // Position the drone should move to
        public GameObject targets; // GameObject containing waypoints as children
        private Vector3 target;
        private bool navigationMode = false; // Flag for autonomous navigation mode
        private Vector3 home; // Home position
        public GameObject homeGameobjetc;
        public int targetIndex = 0;
        public float kP = 1.5f;
        public float kI = 0.1f;
        public float kD = 0.3f;
        public float kPy = 1.5f;
        public float kIy = 0.1f;
        public float kDy = 0.3f;
        public float maxSpeed = 0.5f;
        public float error = 0.2f;
        public float dampingFactor = 0.9f; // The lower this is, the quicker it slows down

        private float positionErrorx;
        private float positionErrory;
        private float positionErrorz;
        private float positionIntegralx;
        private float positionIntegraly;
        private float positionIntegralz;
        private float lastPositionErrorx;
        private float lastPositionErrory;
        private float lastPositionErrorz;
        private float positionDerivativex;
        private float positionDerivativey;
        private float positionDerivativez;

        private float velx;
        private float vely;
        private float velz;

        private IP_Drone_Inputs droneInputs;
        private Rigidbody rb;

        void Start()
        {
            home = homeGameobjetc.transform.position;
            GoHome(); // Initialize by setting target to home
            droneInputs = GetComponent<IP_Drone_Inputs>();
            rb = GetComponent<Rigidbody>();
            lastPositionErrorx = transform.position.x;
            lastPositionErrory = transform.position.y;
            lastPositionErrorz = transform.position.z;
        }

        void FixedUpdate()
        {
            // Toggle navigation mode with controller button
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
            {
                navigationMode = !navigationMode;
            }

            if (navigationMode)
            {
                // If the drone is close enough to the current target, switch to the next
                if (Mathf.Abs(targetPosition.x - transform.position.x) < error &&
                    Mathf.Abs(targetPosition.y - transform.position.y) < error &&
                    Mathf.Abs(targetPosition.z - transform.position.z) < error)
                {
                    if (targetIndex <= targets.transform.childCount - 1)
                    {
                        SetTarget(targets.transform.GetChild(targetIndex).transform.position);
                        targetIndex++;
                    }
                    else
                    {
                        // Return to home position
                        GoHome();
                    }
                }
            }
            else
            {
                GoHome(); // Default behavior when not navigating
            }

            ApplyPIDControl(); // Main PID logic
        }

        void ApplyPIDControl()
        {
            float currentPositionx = transform.position.x;
            float currentPositiony = transform.position.y;
            float currentPositionz = transform.position.z;

            // Calculate position errors
            positionErrorx = targetPosition.x - currentPositionx;
            positionErrory = targetPosition.y - currentPositiony;
            positionErrorz = targetPosition.z - currentPositionz;

            // Accumulate integral error
            positionIntegralx += positionErrorx * Time.deltaTime;
            positionIntegraly += positionErrory * Time.deltaTime;
            positionIntegralz += positionErrorz * Time.deltaTime;

            // Compute derivative error
            positionDerivativex = (positionErrorx - lastPositionErrorx) / Time.deltaTime;
            lastPositionErrorx = positionErrorx;
            positionDerivativey = (positionErrory - lastPositionErrory) / Time.deltaTime;
            lastPositionErrory = positionErrory;
            positionDerivativez = (positionErrorz - lastPositionErrorz) / Time.deltaTime;
            lastPositionErrorz = positionErrorz;

            // PID output calculation for each axis
            velx = (kP * positionErrorx) + (kI * positionIntegralx) + (kD * positionDerivativex);
            vely = (kPy * positionErrory) + (kIy * positionIntegraly) + (kDy * positionDerivativey);
            velz = (kP * positionErrorz) + (kI * positionIntegralz) + (kD * positionDerivativez);

            // Clamp velocity to max speed
            velx = Mathf.Clamp(velx, -maxSpeed, maxSpeed);
            vely = Mathf.Clamp(vely, -maxSpeed, maxSpeed);
            velz = Mathf.Clamp(velz, -maxSpeed, maxSpeed);

            // Final PID output vector
            Vector3 pidOutput = new Vector3(velx, vely, velz);

            // Apply the calculated velocity to the drone
            droneInputs.SetVelocity(pidOutput);
        }

        public void SetTarget(Vector3 newTarget)
        {
            targetPosition = newTarget;
        }

        public void GoHome()
        {
            SetTarget(home);
            targetIndex = 0;
            navigationMode = false;
        }
    }
}

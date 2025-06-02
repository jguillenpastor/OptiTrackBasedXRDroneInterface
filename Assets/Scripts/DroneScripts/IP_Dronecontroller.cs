using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IndiePixel
{
    [RequireComponent(typeof(IP_Drone_Inputs))]
    public class IP_Dronecontroller : IP_Base_Rigidbody
    {
        [Header("Control Settings")]
        [SerializeField] private float minMaxPitch = 30f;
        [SerializeField] private float minMaxRoll = 30f;
        [SerializeField] private float verticalForceMultiplier = 10f;
        [SerializeField] private float horizontalForceMultiplier = 5f; // Multiplier for X and Z movement

        private IP_Drone_Inputs input;
        private List<IEngine> engines = new List<IEngine>();

        private float finalPitch;
        private float finalRoll;
        private float yaw;
        private float finalYaw;

        private float lerpSpeed = 2f;

        void Start()
        {
            input = GetComponent<IP_Drone_Inputs>();
            if (input == null)
            {
                Debug.LogError("Error: IP_Drone_Inputs not found on drone.");
            }
            engines = GetComponentsInChildren<IEngine>().ToList<IEngine>();
        }

        protected override void HandlePhysics()
        {
            HandleEngines();
            HandleControls();
        }

        protected virtual void HandleEngines()
        {
            foreach (IEngine engine in engines)
            {
                engine.UpdateEngine(rb, input);
            }
        }

        public float GetCurrentYaw()
        {
            float yaw = rb.rotation.eulerAngles.y;
            if (yaw > 180f)
            {
                yaw -= 360f; // Convert range to [-180, 180]
            }
            return yaw;
        }

        protected virtual void HandleControls()
        {
            // Convert velocity input into pitch and roll angles
            float pitch = Mathf.Clamp(input.VelocityCommand.z * minMaxPitch, -minMaxPitch, minMaxPitch);
            float roll = Mathf.Clamp(-input.VelocityCommand.x * minMaxRoll, -minMaxRoll, minMaxRoll);

            // Smooth transition of pitch and roll
            finalPitch = Mathf.Lerp(finalPitch, pitch, Time.deltaTime * lerpSpeed);
            finalRoll = Mathf.Lerp(finalRoll, roll, Time.deltaTime * lerpSpeed);

            Quaternion rot = Quaternion.Euler(finalPitch, GetCurrentYaw(), finalRoll);
            rb.MoveRotation(rot);

            // Apply vertical force to go up/down
            float verticalForce = (rb.mass * Physics.gravity.magnitude) + (input.VelocityCommand.y * verticalForceMultiplier);
            Vector3 forceY = Vector3.up * verticalForce;
            rb.AddForce(forceY, ForceMode.Force);

            // Ensure horizontal forces remain in XZ plane
            Vector3 forwardFlat = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            Vector3 rightFlat = new Vector3(transform.right.x, 0, transform.right.z).normalized;

            // Apply horizontal forces (X, Z) only
            Vector3 forwardForce = forwardFlat * input.VelocityCommand.z * horizontalForceMultiplier;
            Vector3 rightForce = rightFlat * input.VelocityCommand.x * horizontalForceMultiplier;

            rb.AddForce(forwardForce + rightForce, ForceMode.Force);
        }
    }
}

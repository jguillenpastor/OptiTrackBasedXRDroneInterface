using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel
{
    public class IP_Drone_Inputs : MonoBehaviour
    {
        #region Variables

        [Header("Manual Velocity Input")]
        [SerializeField] private float velocityX = 0f; // Velocity on X axis (forward/backward)
        [SerializeField] private float velocityY = 0f; // Velocity on Y axis (altitude)
        [SerializeField] private float velocityZ = 0f; // Velocity on Z axis (left/right)

        private Vector3 velocityCommand; // Velocity command in X, Y, Z

        public Vector3 VelocityCommand { get => velocityCommand; }

        #endregion

        #region MainMethods
        void Start()
        {
            // Assign the manual values to the velocity vector
            velocityCommand = new Vector3(velocityX, velocityY, velocityZ);
        }

        public void SetVelocity(Vector3 newVelocity)
        {
            // Debug.Log("New Velocity Applied to Motors: " + newVelocity);
            velocityCommand = newVelocity;
        }
        #endregion
    }
}

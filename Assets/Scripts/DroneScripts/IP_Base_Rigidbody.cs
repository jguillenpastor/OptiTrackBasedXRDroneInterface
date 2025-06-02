using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel
{
    [RequireComponent(typeof(Rigidbody))]
    public class IP_Base_Rigidbody : MonoBehaviour
    {
        #region Variables
        [SerializeField ]private float weightInKgs = 1f;

        protected Rigidbody rb;
        protected float startDrag;
        protected float startAngularDrag;

        #endregion
        #region Main Methods
        
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.mass = weightInKgs;
                startDrag = rb.drag;
                startAngularDrag = rb.angularDrag;
            }
        }

        void FixedUpdate()
        {
            if (rb == null)
            {
                return;
            }

            HandlePhysics();
        }
        #endregion
        protected virtual void HandlePhysics()
        {
        }
    }
}

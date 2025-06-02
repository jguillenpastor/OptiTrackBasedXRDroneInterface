using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel
{
    public class IP_Drone_Engine : MonoBehaviour
    {
        [SerializeField] private Transform propeller;
        [SerializeField] private float propRotSpeed = 300;

        void Update()
        {
            HandlePropellers();
        }

        void HandlePropellers()
        {
            if (propeller == null) return;
            propeller.Rotate(Vector3.up, propRotSpeed * Time.deltaTime);
        }
    }
}

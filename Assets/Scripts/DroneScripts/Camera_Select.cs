using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Select : MonoBehaviour
{
    public Camera[] cameras;  // Array of cameras in the scene
    private int cameraActual = 0;

    void Start()
    {
        // Deactivate all cameras except the first one
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == cameraActual);
        }
    }

    void Update()
    {
        // Switch camera when pressing numeric keys (1, 2, 3, etc.)
        for (int i = 0; i < cameras.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Cambiarcamera(i);
            }
        }
    }

    void Cambiarcamera(int indice)
    {
        if (indice < 0 || indice >= cameras.Length)
            return;

        // Deactivate the current camera and activate the new one
        cameras[cameraActual].gameObject.SetActive(false);
        cameras[indice].gameObject.SetActive(true);
        cameraActual = indice;
    }

    private void OnChangeCamera()
    {
        // Switch to the next camera in the array
        if (cameraActual >= cameras.Length - 1)
        {
            cameraActual = 0;
        }
        else
        {
            cameraActual++;
        }

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == cameraActual);
        }
    }
}

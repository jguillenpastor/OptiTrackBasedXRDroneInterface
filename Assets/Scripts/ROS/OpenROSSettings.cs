#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class OpenROSSettings : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Robotics/Open ROS Settings")]
    static void OpenSettings()
    {
        SettingsService.OpenProjectSettings("Project/Robotics/ROS Settings");
    }
#endif
}
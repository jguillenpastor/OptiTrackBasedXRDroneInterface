using System;
using System.IO;
using UnityEngine;

public class DronePositionLogger : MonoBehaviour
{
    public Transform droneTransform; // The drone's transform in Unity
    public Vector3 rosPosition; // ROS position (set from another script receiving ROS data)
    public Transform headsetPosition; // Headset position in Unity (set externally)

    private string droneunityFilePath;
    private string dronerosFilePath;
    private string headsetunityFilePath;
    private string headsetrosFilePath;
    private StreamWriter droneunityWriter;
    private StreamWriter dronerosWriter;
    private StreamWriter headsetunityWriter;
    private StreamWriter headsetrosWriter;

    private bool saveonce = true;

    void Start()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string folderPath = Application.persistentDataPath;
        Debug.Log(folderPath);

        // Generate file paths with timestamp
        droneunityFilePath = Path.Combine(folderPath, $"{timestamp}_drone_unity.csv");
        dronerosFilePath = Path.Combine(folderPath, $"{timestamp}_drone_ros.csv");
        headsetunityFilePath = Path.Combine(folderPath, $"{timestamp}_headset_unity.csv");
        headsetrosFilePath = Path.Combine(folderPath, $"{timestamp}_headset_ros.csv");

        // Create writers for each file
        droneunityWriter = new StreamWriter(droneunityFilePath, false);
        dronerosWriter = new StreamWriter(dronerosFilePath, false);
        headsetunityWriter = new StreamWriter(headsetunityFilePath, false);
        headsetrosWriter = new StreamWriter(headsetrosFilePath, false);

        // Write CSV headers
        droneunityWriter.WriteLine("time,x,y,z");
        dronerosWriter.WriteLine("time,x,y,z");
        headsetunityWriter.WriteLine("time,x,y,z");
        headsetrosWriter.WriteLine("time,x,y,z");
    }

    void Update()
    {
        string time = Time.time.ToString("F3");

        // Save drone position in Unity coordinates
        Vector3 droneunityPos = droneTransform.position;
        droneunityWriter.WriteLine($"{time},{droneunityPos.x},{droneunityPos.y},{droneunityPos.z}");
        droneunityWriter.Flush();

        // Save drone position in ROS coordinates
        dronerosWriter.WriteLine($"{time},{rosPosition.x},{rosPosition.y},{rosPosition.z}");
        dronerosWriter.Flush();

        // Save headset position in Unity coordinates (only once)
        Vector3 headsetunityPos = headsetPosition.position;
        if (headsetunityPos != Vector3.zero && saveonce)
        {
            headsetunityWriter.WriteLine($"{time},{headsetunityPos.x},{headsetunityPos.y},{headsetunityPos.z}");
            headsetunityWriter.Flush();
            saveonce = false;
        }
    }

    // Used to save the initial headset position in ROS coordinates
    public void saveHeadsetInitialPos(Vector3 headsetRosPos)
    {
        string time = Time.time.ToString("F3");

        headsetrosWriter.WriteLine($"{time},{headsetRosPos.x},{headsetRosPos.y},{headsetRosPos.z}");
        headsetrosWriter.Flush();
    }

    private void OnApplicationQuit()
    {
        CloseWriters();
    }

    private void OnDestroy()
    {
        CloseWriters();
    }

    // Close all file writers safely
    void CloseWriters()
    {
        if (droneunityWriter != null)
        {
            droneunityWriter.Close();
            droneunityWriter = null;
        }
        if (dronerosWriter != null)
        {
            dronerosWriter.Close();
            dronerosWriter = null;
        }
        if (headsetunityWriter != null)
        {
            headsetunityWriter.Close();
            headsetunityWriter = null;
        }
        if (headsetrosWriter != null)
        {
            headsetrosWriter.Close();
            headsetrosWriter = null;
        }
    }
}

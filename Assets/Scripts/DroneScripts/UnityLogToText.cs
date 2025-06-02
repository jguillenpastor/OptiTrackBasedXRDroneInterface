using UnityEngine;
using TMPro;

public class UnityLogToText : MonoBehaviour
{
    public TextMeshProUGUI logText;

    private string fullLog = "";
    private const int maxLines = 20; // Max number of lines to display in the log

    void OnEnable()
    {
        // Register to receive Unity log messages
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        // Unregister when the object is disabled
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Set log color based on log type
        string color = type switch
        {
            LogType.Error => "red",
            LogType.Warning => "yellow",
            LogType.Assert => "orange",
            LogType.Exception => "magenta",
            _ => "white"
        };

        // Format and append the new message
        string formatted = $"<color={color}>{logString}</color>\n";
        fullLog += formatted;

        // Trim log if it exceeds maxLines
        string[] lines = fullLog.Split('\n');
        if (lines.Length > maxLines)
        {
            fullLog = string.Join("\n", lines[^maxLines..]);
        }

        // Update UI text
        logText.text = fullLog;
    }
}

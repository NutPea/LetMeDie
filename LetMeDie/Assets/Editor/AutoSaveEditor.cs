#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class AutoSaveEditor
{
    private const double saveInterval = 900.0; // 15 Minuten
    private static double lastSaveTime;

    static AutoSaveEditor()
    {
        lastSaveTime = EditorApplication.timeSinceStartup;
        EditorApplication.update += Update;
    }

    private static void Update()
    {
        if (EditorApplication.timeSinceStartup - lastSaveTime > saveInterval)
        {
            AutoSave();
            lastSaveTime = EditorApplication.timeSinceStartup;
        }
    }

    private static void AutoSave()
    {
        string currentTime = DateTime.Now.ToString("HH:mm:ss");
        Debug.Log($"[{currentTime}] Everything got saved!");
        EditorApplication.ExecuteMenuItem("File/Save");
    }
}
#endif
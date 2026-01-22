using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class AutomatedBuild
{
    public static void BuildAndroid()
    {
        Debug.Log("BUILD STARTED");

        string[] scenes = new string[]
        {
            "Assets/Scenes/SampleScene.unity"
        };

        string buildPath = "builds/Android/build.apk";

        // Set required Android settings
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel29;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel34;

        //Log paths
        Debug.Log($"Building to: {buildPath}");
        Debug.Log($"Target Platform: {EditorUserBuildSettings.activeBuildTarget}");
        Debug.Log($"Android SDK Path: {EditorPrefs.GetString("AndroidSdkRoot")}");

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        //Retrace build steps
        foreach (BuildStep step in report.steps)
        {
            Debug.Log($"Step: {step.name} - Duration: {step.duration}");
            foreach (BuildStepMessage message in step.messages)
            {
                if (message.type == LogType.Error || message.type == LogType.Exception)
                {
                    Debug.LogError($"  ERROR: {message.content}");
                }
                else if (message.type == LogType.Warning)
                {
                    Debug.LogWarning($"  WARNING: {message.content}");
                }
            }
        }

        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log($"BUILD SUCCEEDED | Size: {report.summary.totalSize} bytes");
        }
        else
        {
            Debug.LogError("BUILD FAILED");
            EditorApplication.Exit(5);
        }
    }
}
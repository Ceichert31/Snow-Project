using UnityEditor;
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

        Debug.Log($"Building to: {buildPath}");

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log($"BUILD SUCCEEDED | Size: {report.summary.totalSize} bytes");
        }
        else
        {
            Debug.LogError("BUILD FAILED");
            EditorApplication.Exit(1);
        }
    }
}
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutomatedBuild
{
    public static void BuildAndroid()
    {
        string[] scenes = EditorBuildSettings
            .scenes.Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        string buildPath = "builds/Android/build.apk";

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.Android,
            options = BuildOptions.None,
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"BUILD SUCCEEDED | Size: {report.summary.totalSize} bytes");
        }
        else
        {
            Debug.LogError("BUILD FAILED");
            EditorApplication.Exit(-1);
        }
    }
}

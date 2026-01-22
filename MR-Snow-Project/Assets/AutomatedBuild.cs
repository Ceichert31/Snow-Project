using UnityEditor;
using System.IO;

public class AutomatedBuild
{
    /// <summary>
    /// Builds the android APK
    /// </summary>
    /// <remarks>
    /// Called by GitHub action runner
    /// </remarks>
    public static void BuildAndroid()
    {
        //Add all scenes we want to build
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };

        string outputPath = "builds/Android/build.apk"; 

        // Ensure output directory exists
        Directory.CreateDirectory("builds");

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = outputPath;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
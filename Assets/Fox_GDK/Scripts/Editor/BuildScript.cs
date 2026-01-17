using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UnityBuilderAction
{
    public static class BuildScript
    {
        public static void Build()
        {
            string[] scenes = EditorBuildSettings.scenes.Length > 0
                ? GetEnabledScenes()
                : new string[] { "Assets/Scenes/SampleScene.unity" };

            string buildPath = Path.Combine("build", EditorUserBuildSettings.activeBuildTarget.ToString());

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = buildPath,
                target = EditorUserBuildSettings.activeBuildTarget,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

            if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + report.summary.outputPath);
            }
            else
            {
                Debug.LogError("Build failed");
                EditorApplication.Exit(1);
            }
        }

        private static string[] GetEnabledScenes()
        {
            var scenes = new string[EditorBuildSettings.scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = EditorBuildSettings.scenes[i].path;
            }
            return scenes;
        }
    }
}
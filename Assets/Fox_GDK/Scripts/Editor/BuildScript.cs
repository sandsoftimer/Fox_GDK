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
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            
            string[] scenes = EditorBuildSettings.scenes.Length > 0
                ? GetEnabledScenes()
                : new string[] { "Assets/Scenes/SampleScene.unity" };

            string buildPath = GetBuildPath(buildTarget);
            
            Directory.CreateDirectory(Path.GetDirectoryName(buildPath));

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = buildPath,
                target = buildTarget,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

            if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + report.summary.outputPath);
            }
            else
            {
                Debug.LogError("Build failed: " + report.summary.result);
                EditorApplication.Exit(1);
            }
        }
        
        private static string GetBuildPath(BuildTarget target)
        {
            string basePath = Path.Combine("build", target.ToString());
            
            switch (target)
            {
                case BuildTarget.WebGL:
                    return basePath;
                case BuildTarget.Android:
                    return Path.Combine(basePath, "game.apk");
                default:
                    return basePath;
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
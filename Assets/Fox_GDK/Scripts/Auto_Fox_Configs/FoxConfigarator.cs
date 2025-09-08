#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FoxConfigarator : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        #region Debug log for anything happend on asset folder
        //foreach (string str in importedAssets)
        //{
        //    Debug.Log("Reimported Asset: " + str);
        //}
        //foreach (string str in deletedAssets)
        //{
        //    Debug.Log("Deleted Asset: " + str);
        //}

        //for (int i = 0; i < movedAssets.Length; i++)
        //{
        //    Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
        //}
        #endregion Debug log for anything happend on asset folder

#if !PUBLISHER_SDK_INSTALLED
        SetEditorBuildSettings();
#endif
        SetupProjectHierarchy();
        SetUnityLayers();
        SetFoxDefines();
        SetFoxScriptTemplates();
    }

    private static void SetFoxDefines()
    {
        var filePastLocation = Application.dataPath + "/csc.rsp";

        string[,] sdks = new string[,]
        {
            { "LionStudios","PUBLISHER_SDK_INSTALLED"},
            { "Plugins/Demigiant/DOTween","DOTWEEN_SDK_INSTALLED"},
        };

        if (File.Exists(filePastLocation))
            File.WriteAllText(filePastLocation, "");

        FileStream mcs = File.Open(filePastLocation, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw_defines = new StreamWriter(mcs);
        for (int i = 0; i < sdks.Length / 2; i++)
        {
            var sdkName = sdks[i, 0];
            var sdkDefine = sdks[i, 1];
            if (Directory.Exists(Application.dataPath + "/" + sdkName))
            {
                sw_defines.WriteLine("-define:" + sdkDefine);
            }
        }
        sw_defines.Close();
        mcs.Close();
    }

    public static bool IsPackageInstalled(string packageId)
    {
        if (!File.Exists("Packages/manifest.json"))
            return false;

        string jsonText = File.ReadAllText("Packages/manifest.json");
        return jsonText.Contains(packageId);
    }

    private static void SetupProjectHierarchy()
    {
        CreateDirectoryToProject("Artworks");
        CreateDirectoryToProject("Animations");
        CreateDirectoryToProject("Materials");
        CreateDirectoryToProject("Prefabs");
        CreateDirectoryToProject("Resources");
        CreateDirectoryToProject("Scripts/Editor");
        CreateDirectoryToProject("Scenes");
        CreateDirectoryToProject("Textures");
        SetFoxScriptTemplates();
    }

    public static void CreateDirectoryToProject(string directoryName)
    {
        var unityScriptTemplatesPath = Application.dataPath + "/" + directoryName + "/";

        if (!Directory.Exists(unityScriptTemplatesPath))
        {
            Directory.CreateDirectory(unityScriptTemplatesPath);
            Debug.LogError("Project Path <Color=yellow>/Assets/" + directoryName + "</Color> has been created. To modify/add double click me.");
        }
    }

    private static void SetFoxScriptTemplates()
    {
        CreateDirectoryToProject("ScriptTemplates");

        var fileCopyLocation = "";
        var filePastLocation = "";

        fileCopyLocation = Application.dataPath + "/Fox_GDK/Scripts/Auto_Fox_Configs/FoxScriptTemplates/82-Scripting__GameManager-GameManager.cs.txt";
        filePastLocation = Application.dataPath + "/ScriptTemplates/82-Scripting__GameManager-GameManager.cs.txt";
        if (!File.Exists(filePastLocation))
        {
            File.Copy(fileCopyLocation, filePastLocation);
        }

        fileCopyLocation = Application.dataPath + "/Fox_GDK/Scripts/Auto_Fox_Configs/FoxScriptTemplates/81-Scripting__BaseGameBehaviour-BaseGameBehaviour.cs.txt";
        filePastLocation = Application.dataPath + "/ScriptTemplates/81-Scripting__BaseGameBehaviour-BaseGameBehaviour.cs.txt";
        if (!File.Exists(filePastLocation))
        {
            File.Copy(fileCopyLocation, filePastLocation);
        }

        fileCopyLocation = Application.dataPath + "/Fox_GDK/Scripts/Auto_Fox_Configs/FoxScriptTemplates/81-Scripting__NewGameBehaviour-NewGameBehaviour.cs.txt";
        filePastLocation = Application.dataPath + "/ScriptTemplates/81-Scripting__NewGameBehaviour-NewGameBehaviour.cs.txt";
        if (!File.Exists(filePastLocation))
        {
            File.Copy(fileCopyLocation, filePastLocation);
        }

        //EditorApplication.OpenProject(Directory.GetCurrentDirectory());
    }

    private static void SetUnityLayers()
    {
        #region Setup Unity Layers & Tags
        SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProp = manager.FindProperty("layers");
        string[] layersName = ConstantManager.layerNames.Values.ToArray();

        bool layerMismatchfound = false;
        for (int i = 0; i < layersName.Length; i++)
        {
            SerializedProperty sp = layersProp.GetArrayElementAtIndex(i);
            if (!layersName[i].Equals(sp.stringValue))
            {
                layerMismatchfound = true;
                Debug.LogError("Unity Layer Names has been <Color=red>Repaired</Color>. To modify/add layers, see layerNames in <Color=Yellow>gameManager.constantManager.cs</Color>");
                break;
            }
        }

        if (layerMismatchfound)
        {
            for (int i = 8; i < layersName.Length; i++)
            {
                SerializedProperty layerSP = layersProp.GetArrayElementAtIndex(i);
                layerSP.stringValue = layersName[i];
                manager.ApplyModifiedProperties();
            }
        }
        #endregion Setup Unity Layers
    }

    private static void SetEditorBuildSettings()
    {
        #region Setup BuildSetting
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        string[] scenesPath = new string[sceneCount];
        List<string> newScenesPath = new List<string>();
        int bootSceneIndex = -1;
        for (int i = 0; i < sceneCount; i++)
        {
            scenesPath[i] = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            if (scenesPath[i].Contains("Fox_BootScene"))
            {
                bootSceneIndex = i;
            }
        }

        bool flag = false;
        if (bootSceneIndex == -1)
        {
            // Boot Scene not added yet.
            // Adding Boot Scene to BuildIndex
            Debug.LogError("<Color=red>Fox_BootScene was not found in BuildSettings.</Color> <Color=green>Re-Added at 0 index</Color>. To modify/add double click me.");
            flag = true;
            string bootScenePath = "Assets/Fox_GDK/Scenes/Fox_BootScene.unity";

            //string[] newScenesPath = new string[scenesPath.Length + 1];
            for (int i = 0; i < scenesPath.Length + 1; i++)
            {
                if (i == 0)
                    newScenesPath.Add(bootScenePath);
                else
                    newScenesPath.Add(scenesPath[i - 1]);
            }
        }
        else if (bootSceneIndex != 0)
        {
            // Boot Scene must be at 0 index.
            Debug.LogError("<Color=green>BuildSettings Updated.</Color> <Color=yellow>APBootScene must be at 0 index.</Color>");
            flag = true;

            List<string> tempScenesPath = new List<string>();
            tempScenesPath = scenesPath.ToList();

            newScenesPath.Add(scenesPath[bootSceneIndex]);
            for (int i = 0; i < tempScenesPath.Count; i++)
            {
                if (i != bootSceneIndex)
                    newScenesPath.Add(scenesPath[i]);
            }
        }

        if (flag)
        {
            EditorBuildSettingsScene[] editorBuildSettingsScene = new EditorBuildSettingsScene[newScenesPath.Count];
            for (int j = 0; j < newScenesPath.Count; j++)
            {
                EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene(newScenesPath[j], true);
                editorBuildSettingsScene[j] = sceneToAdd;
            }
            EditorBuildSettings.scenes = new EditorBuildSettingsScene[0];
            EditorBuildSettings.scenes = editorBuildSettingsScene;
        }
        #endregion Setup BuildSetting
    }

}
#endif
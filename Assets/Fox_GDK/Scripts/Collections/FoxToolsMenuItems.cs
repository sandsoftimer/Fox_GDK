#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class FoxToolsMenuItems
{
    [MenuItem("FoxTools/Delete Gameplay Data", false, 0)]
    public static void DeleteBinaryData()
    {
        PlayerPrefs.DeleteAll();
        string[] allfiles = Directory.GetFiles($"{Application.persistentDataPath}", "*.*", SearchOption.AllDirectories);
        for (int i = 0; i < allfiles.Length; i++)
        {
            File.Delete(allfiles[i]);
        }
        Debug.LogError($"All Gameplay Data Deleted <Color=yellow>Successfully</Color>");
    }

    [MenuItem("FoxTools/Extra/Generate Conflict")]
    public static void GenerateConflict()
    {
        string path = "ConflictForGreaterGood.txt";
        string data = string.Format("GitConflictGeneration: {0}\n{1} - Device Name: {2}\n{3}\n{4}", System.DateTime.Now, System.DateTime.Now.Ticks, SystemInfo.deviceName, SystemInfo.deviceModel, SystemInfo.deviceUniqueIdentifier);
        StreamWriter sw = File.CreateText(path);
        sw.WriteLine(data);
        sw.Close();
        Debug.LogErrorFormat(data);
    }

    public static void CreateScriptableObject(string path, Type obj)
    {
        ScriptableObject so = ScriptableObject.CreateInstance(obj);
        Selection.activeObject = CreateIfDoesntExists(path, so);
    }

    public static Object CreateIfDoesntExists(string path, Object o)
    {
        var ap = AssetDatabase.LoadAssetAtPath(path, o.GetType());
        if (ap == null)
        {
            AssetDatabase.CreateAsset(o, path);
            ap = AssetDatabase.LoadAssetAtPath(path, o.GetType());
            AssetDatabase.Refresh();
            return ap;
        }
        return ap;
    }

    #region PREFAB VARIENT CREATOR
    [MenuItem("FoxTools/UI Holder Variant", false, 10)]
    static void CreatePrefabVariant()
    {
        // Step 1: Select the base prefab in Project window
        Object selected = Selection.activeObject;
        string path = "Assets/Fox_GDK/Prefabs/UI/UI_Holder";

        if (File.Exists(path))
        {
            Debug.LogError($"Prefab missing: {path}");
            return;
        }

        // Step 2: Define target folder for prefab variants
        string targetFolder = "Assets/Fox_GDK/Prefabs/UI";
        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

        // Step 3: Generate a unique variant name
        string fileName = "UI_Holder";
        string variantPath = AssetDatabase.GenerateUniqueAssetPath(
            $"{targetFolder}/{fileName}_Variant.prefab"
        );

        // Step 4: Load prefab contents and create variant
        GameObject basePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Fox_GDK/Prefabs/UI/UI_Holder.prefab");
        GameObject instance = PrefabUtility.InstantiatePrefab(basePrefab) as GameObject;

        // Save as prefab variant
        GameObject variantPrefab = PrefabUtility.SaveAsPrefabAsset(instance, variantPath);

        // Step 5: Instantiate variant into hierarchy
        GameObject variantInstance = PrefabUtility.InstantiatePrefab(variantPrefab, Selection.activeObject.GameObject().transform) as GameObject;
        if (variantInstance != null)
        {
            Undo.RegisterCreatedObjectUndo(variantInstance, "Create Prefab Variant");
            Selection.activeGameObject = variantInstance;
        }

        // Cleanup
        Object.DestroyImmediate(instance);

        Debug.Log($"Prefab Variant created at: {variantPath} and instantiated in scene.");
    }


    // Add menu item when right-clicking in the Hierarchy
    [MenuItem("GameObject/FoxTools/UI Holder Variant", false, 10)]
    private static void CreateCustomObject(MenuCommand menuCommand)
    {
        CreatePrefabVariant();
    }
    #endregion PREFAB VARIENT CREATOR
}
#endif

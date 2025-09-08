#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class FoxEditorScriptMaker
{
    private static string selectedScriptPath = "";
    private static string selectedScriptName = "";

    [MenuItem("Assets/FoxTools/Add\\Update_FoxEditor_Outside")]
    private static void FoxEditorScript()
    {
        Type t = Type.GetType(Selection.activeObject.name);
        FieldInfo[] fieldInfos = t.GetFields();

        FoxConfigarator.CreateDirectoryToProject("Scripts/Editor");
        string path = Application.dataPath + "/Scripts/Editor/" + selectedScriptName + "Editor.cs";

        if (!File.Exists(path))
        {
            TextAsset asset = Resources.Load("NewFoxEditorScript.cs") as TextAsset;
            string scriptSkeleton = asset.text.Replace("#SCRIPTNAME#", selectedScriptName);

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                scriptSkeleton = scriptSkeleton.Replace(
                    "#endregion ALL_PUBLIC_PROPERTIES",
                    "private SerializedProperty " + fieldInfo.Name + ";" +
                    "\n\t#endregion ALL_PUBLIC_PROPERTIES");

                scriptSkeleton = scriptSkeleton.Replace(
                    "#endregion FINDER_ALL_PUBLIC_PROPERTIES",
                    fieldInfo.Name + " = serializedObject.FindProperty(\"" + fieldInfo.Name + "\");" +
                    "\n\t\t#endregion FINDER_ALL_PUBLIC_PROPERTIES");

                scriptSkeleton = scriptSkeleton.Replace(
                    "#endregion DrawProperty(propertyName)",
                    "DrawProperty(" + fieldInfo.Name + ");" +
                    "\n\t\t\t#endregion DrawProperty(propertyName)");
            }

            //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(path);
            writer.Write(scriptSkeleton);
            writer.Close();
            AssetDatabase.Refresh();
        }
        else
        {
            Update_Existing();
        }
    }

    private static void Update_Existing()
    {
        Type t = Type.GetType(Selection.activeObject.name);
        FieldInfo[] fieldInfos = t.GetFields();

        selectedScriptPath = Application.dataPath + "/Scripts/Editor/" + selectedScriptName + "Editor.cs";
        string scriptSkeleton = File.ReadAllText(selectedScriptPath);
        string pattern = string.Empty;
        string start = string.Empty;
        string end = string.Empty;

        start = "#region ALL_PUBLIC_PROPERTIES";
        end = "#endregion ALL_PUBLIC_PROPERTIES";
        pattern = Regex.Escape(start) + ".*" + Regex.Escape(end);
        scriptSkeleton = Regex.Replace(scriptSkeleton, pattern, "#region ALL_PUBLIC_PROPERTIES\n\t#endregion ALL_PUBLIC_PROPERTIES", RegexOptions.Singleline);

        start = "#region FINDER_ALL_PUBLIC_PROPERTIES";
        end = "#endregion FINDER_ALL_PUBLIC_PROPERTIES";
        pattern = Regex.Escape(start) + ".*" + Regex.Escape(end);
        scriptSkeleton = Regex.Replace(scriptSkeleton, pattern, "#region FINDER_ALL_PUBLIC_PROPERTIES\n\t\t#endregion FINDER_ALL_PUBLIC_PROPERTIES", RegexOptions.Singleline);

        start = "#region DrawProperty(propertyName)";
        end = "#endregion DrawProperty(propertyName)";
        pattern = Regex.Escape(start) + ".*" + Regex.Escape(end);
        scriptSkeleton = Regex.Replace(scriptSkeleton, pattern, "#region DrawProperty(propertyName)\n\t\t\t#endregion DrawProperty(propertyName)", RegexOptions.Singleline);

        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            scriptSkeleton = scriptSkeleton.Replace(
                "#endregion ALL_PUBLIC_PROPERTIES",
                "private SerializedProperty " + fieldInfo.Name + ";" +
                "\n\t#endregion ALL_PUBLIC_PROPERTIES");

            scriptSkeleton = scriptSkeleton.Replace(
                "#endregion FINDER_ALL_PUBLIC_PROPERTIES",
                fieldInfo.Name + " = serializedObject.FindProperty(\"" + fieldInfo.Name + "\");" +
                "\n\t\t#endregion FINDER_ALL_PUBLIC_PROPERTIES");

            scriptSkeleton = scriptSkeleton.Replace(
                "#endregion DrawProperty(propertyName)",
                "DrawProperty(" + fieldInfo.Name + ");" +
                "\n\t\t#endregion DrawProperty(propertyName)");
        }

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(selectedScriptPath);
        writer.Write(scriptSkeleton);
        writer.Close();
        AssetDatabase.Refresh();
    }

    // Note that we pass the same path, and also pass "true" to the second argument for validation.
    [MenuItem("Assets/FoxTools/Add\\Update_FoxEditor_Outside", true)]
    private static bool FoxEditorScriptValidation()
    {
        // This returns true when the selected object is an C# (the menu item will be disabled otherwise).
        selectedScriptName = Selection.activeObject.name;
        selectedScriptPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        return Selection.activeObject is MonoScript || Selection.activeObject is ScriptableObject;
    }
}
#endif
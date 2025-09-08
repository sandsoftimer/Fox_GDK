#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class FoxInsideEditorScriptMaker
{
    private static string selectedScriptPath = "";
    private static string selectedScriptName = "";

    [MenuItem("Assets/FoxTools/Add\\Update_FoxEditor_Inside")]
    private static void FoxInsideEditorScript()
    {
        Type t = Type.GetType(Selection.activeObject.name);
        FieldInfo[] fieldInfos = t.GetFields();

        string scriptSkeleton = File.ReadAllText(selectedScriptPath);

        if (!scriptSkeleton.Contains("#region FOX EDITOR MAKER"))
        {
            TextAsset asset = Resources.Load("NewFoxInsideEditorScript.cs") as TextAsset;
            scriptSkeleton += "\n";
            scriptSkeleton += asset.text.Replace("#SCRIPTNAME#", selectedScriptName);
            if (!scriptSkeleton.Contains("using UnityEditor;"))
                scriptSkeleton = "using UnityEditor;\n" + scriptSkeleton;
            if (!scriptSkeleton.Contains("using UnityEngine;"))
                scriptSkeleton = "using UnityEngine;\n" + scriptSkeleton;

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
        else
        {
            Update_Existing();
        }
    }

    static void Update_Existing()
    {
        Type t = Type.GetType(Selection.activeObject.name);
        FieldInfo[] fieldInfos = t.GetFields();

        string scriptSkeleton = File.ReadAllText(selectedScriptPath);
        string pattern = string.Empty;
        string start = string.Empty;
        string end = string.Empty;

        //string input = "Hello #World 123\nHello Universe 456\nHello Space 123";
        //start = "#World";
        //end = "456";

        //pattern = Regex.Escape(start) + ".*" + Regex.Escape(end);

        //if (Regex.IsMatch(input, pattern, RegexOptions.Singleline))
        //{
        //    Debug.LogError(scriptSkeleton);
        //}
        //else
        //{
        //    Debug.LogError("Pattern does not match.");
        //}

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
                "\n\t\t\t#endregion DrawProperty(propertyName)");
        }

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(selectedScriptPath);
        writer.Write(scriptSkeleton);
        writer.Close();
        AssetDatabase.Refresh();
    }

    // Note that we pass the same path, and also pass "true" to the second argument for validation.
    [MenuItem("Assets/FoxTools/Add\\Update_FoxEditor_Inside", true)]
    private static bool FoxInsideEditorScriptValidation()
    {
        // This returns true when the selected object is an C# (the menu item will be disabled otherwise).
        selectedScriptName = Selection.activeObject.name;
        selectedScriptPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        return Selection.activeObject is MonoScript || Selection.activeObject is ScriptableObject;
    }
}
#endif
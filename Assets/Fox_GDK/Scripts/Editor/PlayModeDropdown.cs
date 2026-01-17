#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

[InitializeOnLoad]
public static class PlayFromSceneZero
{
    private const string PreviousSceneKey = "PlayFromSceneZero_PreviousScene";

    public static Color ButtonColor = new Color(255, 0, 0);
    public static float GapFromPlayButton = 0f;

    static PlayFromSceneZero()
    {
        ToolbarCallback.OnToolbarGUI = OnToolbarGUI;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            string previousScene = EditorPrefs.GetString(PreviousSceneKey, "");
            if (!string.IsNullOrEmpty(previousScene))
            {
                EditorSceneManager.OpenScene(previousScene);
                EditorPrefs.DeleteKey(PreviousSceneKey);
            }
        }
    }

    private static void OnToolbarGUI()
    {
        if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode) return;

        GUILayout.BeginHorizontal();
        GUILayout.Space(GapFromPlayButton);

        var style = new GUIStyle("CommandLeft");
        var icon = EditorGUIUtility.IconContent("PlayButton");

        GUI.backgroundColor = ButtonColor;
        if (GUILayout.Button(icon, style))
        {
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorPrefs.SetString(PreviousSceneKey, currentScene);
                EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
                EditorApplication.isPlaying = true;
            }
        }
        GUI.backgroundColor = Color.white;
        GUILayout.EndHorizontal();
    }
}

static class ToolbarCallback
{
    static System.Type toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
    static System.Reflection.FieldInfo toolbarField = toolbarType.GetField("get", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

    public static System.Action OnToolbarGUI;

    static ToolbarCallback()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
        if (toolbarField == null) return;

        var toolbar = toolbarField.GetValue(null);
        if (toolbar != null)
        {
            EditorApplication.update -= Update;

            var root = toolbar.GetType().GetField("m_Root", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var element = root.GetValue(toolbar) as VisualElement;
            var zone = element.Q("ToolbarZonePlayMode");

            var container = new IMGUIContainer(() => OnToolbarGUI?.Invoke())
            {
                style = { flexDirection = FlexDirection.Row }
            };
            zone.Insert(0, container);
        }
    }
}
#endif
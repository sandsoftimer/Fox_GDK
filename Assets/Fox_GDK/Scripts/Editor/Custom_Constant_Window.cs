//using UnityEditor;
//using UnityEngine;

//public class Custom_Constant_Window : EditorWindow
//{
//    ConstantManager constantManager;
//    public ConstantManager targetObject
//    {
//        get
//        {
//            if (constantManager == null)
//            {
//                constantManager = Resources.Load("Constant Manager") as ConstantManager;
//            }
//            return constantManager;
//        }
//    }

//    [MenuItem("FoxTools/Extra/Game Constants")]
//    public static void ShowWindow()
//    {
//        GetWindow<Custom_Constant_Window>("Game Constants");
//    }

//    void OnGUI()
//    {
//        if (targetObject != null)
//        {
//            if (targetObject != null)
//            {
//                GUIStyle paddedStyle = new GUIStyle(GUI.skin.box);
//                paddedStyle.padding = new RectOffset(10, 10, 10, 10);
//                EditorGUILayout.BeginVertical(paddedStyle);

//                FoxEditor.Draw_This_TextField("GAME LOOPING INFO", FoxEditor.BackgroundStyle(new Color(0.5f, 0.75f, 0.0f), Color.black, TextAnchor.MiddleCenter), 20);

//                constantManager.SCENE_LOOPING_TYPE = (Fox_SceneLooping_Type)EditorGUILayout.EnumPopup(nameof(constantManager.SCENE_LOOPING_TYPE), constantManager.SCENE_LOOPING_TYPE);
//                constantManager.TOTAL_GAME_LEVELS = EditorGUILayout.IntField(nameof(constantManager.TOTAL_GAME_LEVELS), constantManager.TOTAL_GAME_LEVELS);
//                constantManager.SCENE_LOOPING_STARTING_INDEX = EditorGUILayout.IntField(nameof(constantManager.SCENE_LOOPING_STARTING_INDEX), constantManager.SCENE_LOOPING_STARTING_INDEX);

//                FoxEditor.Draw_This_TextField("TUTORIAL INFO", FoxEditor.BackgroundStyle(new Color(0.5f, 0.5f, 1.0f), Color.black, TextAnchor.MiddleCenter), 20);
//                constantManager.BASIC_TUTORIAL_LEVEL = EditorGUILayout.IntField(nameof(constantManager.BASIC_TUTORIAL_LEVEL), constantManager.BASIC_TUTORIAL_LEVEL);
//                constantManager.MARGE_TUTORIAL_LEVEL = EditorGUILayout.IntField(nameof(constantManager.MARGE_TUTORIAL_LEVEL), constantManager.MARGE_TUTORIAL_LEVEL);
//                constantManager.HIDDEN_HOLES_TUTORIAL_LEVEL = EditorGUILayout.IntField(nameof(constantManager.HIDDEN_HOLES_TUTORIAL_LEVEL), constantManager.HIDDEN_HOLES_TUTORIAL_LEVEL);

//                FoxEditor.Draw_This_TextField("FEATURE ITEM INFO", FoxEditor.BackgroundStyle(new Color(0f, 0f, 0.15f), Color.white, TextAnchor.MiddleCenter), 20);
//                constantManager.SQURE_HOLES_TUTORIAL_LEVEL = EditorGUILayout.IntField(nameof(constantManager.SQURE_HOLES_TUTORIAL_LEVEL), constantManager.SQURE_HOLES_TUTORIAL_LEVEL);

//                FoxEditor.Draw_This_TextField("BOOSTER UNLOCK INFO", FoxEditor.BackgroundStyle(new Color(1f, 0.647f, 0.0f), Color.black, TextAnchor.MiddleCenter), 20);

//                constantManager.HAMMER_UNLOCK_LEVEL_NUMBER = EditorGUILayout.IntField(nameof(constantManager.HAMMER_UNLOCK_LEVEL_NUMBER), constantManager.HAMMER_UNLOCK_LEVEL_NUMBER);
//                constantManager.MAGNET_UNLOCK_LEVEL_NUMBER = EditorGUILayout.IntField(nameof(constantManager.MAGNET_UNLOCK_LEVEL_NUMBER), constantManager.MAGNET_UNLOCK_LEVEL_NUMBER);
//                constantManager.DOCK_UNLOCK_LEVEL_NUMBER = EditorGUILayout.IntField(nameof(constantManager.DOCK_UNLOCK_LEVEL_NUMBER), constantManager.DOCK_UNLOCK_LEVEL_NUMBER);

//                EditorGUILayout.EndVertical();
//            }
//        }
//    }
//}

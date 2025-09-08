#if UNITY_EDITOR
using System;
using Unity.VisualScripting.ReorderableList;
using UnityEditor;
using UnityEngine;

public class FoxEditor : Editor
{
    protected void DrawProperty(SerializedProperty propertyName)
    {
        Space();
        EditorGUILayout.PropertyField(propertyName);
    }

    protected void DrawList(SerializedProperty propertyName)
    {
        Space();
        ReorderableListGUI.Title($"{nameof(propertyName)}");
        ReorderableListGUI.ListField(propertyName);
    }

    protected GUIStyle GetButtonStyle(EditorButtonStyle editorButtonStyle)
    {
        GUIStyle style = new GUIStyle(GUI.skin.button);
        if (editorButtonStyle != null)
        {
            GUI.backgroundColor = editorButtonStyle.buttonColor;
            style.normal.textColor = editorButtonStyle.buttonTextColor;
            style.fixedHeight = editorButtonStyle.buttonHeight;
        }
        return style;
    }

    protected void OnButtonPressed(string buttonText, Action action, EditorButtonStyle editorButtonStyle = null)
    {
        if (GUILayout.Button(buttonText, GetButtonStyle(editorButtonStyle)))
        {
            action.Invoke();
        }
    }

    protected void DrawHorizontalLine()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    protected void DrawHorizontalLineOnGUI(Rect rect)
    {
        EditorGUI.LabelField(rect, "", GUI.skin.horizontalSlider);
    }

    protected void Space(float spaceSize = 1, bool expand = false)
    {
        EditorGUILayout.Space(spaceSize, expand);
    }

    public static GUIStyle BackgroundStyle(Color backgroundColor, Color textColor, TextAnchor textAlignment)
    {
        GUIStyle style = new GUIStyle();
        style.alignment = textAlignment;
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, backgroundColor);
        texture.Apply();
        style.normal.background = texture;
        style.normal.textColor = textColor;
        return style;
    }

    public static void Draw_This_TextField(string name, GUIStyle gUIStyle, float space)
    {
        EditorGUILayout.Space(space);
        GUIStyle myTextFieldStyle = gUIStyle;
        EditorGUILayout.TextField(name, myTextFieldStyle);
    }
}
#endif
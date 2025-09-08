#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyProperty))]
public class ReadOnlyPropertyDrawer : PropertyDrawer
{
    /// <summary>
    /// Display attribute and his value in inspector depending on the type
    /// Fill attribute needed
    /// </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label);
        //switch (property.propertyType)
        //{
        //    case SerializedPropertyType.hand_Item_Curve:
        //    case SerializedPropertyType.ArraySize:
        //    case SerializedPropertyType.Bounds:
        //    case SerializedPropertyType.Character:
        //    case SerializedPropertyType.Color:
        //    case SerializedPropertyType.Generic:
        //    case SerializedPropertyType.Gradient:
        //    case SerializedPropertyType.Integer:
        //    case SerializedPropertyType.LayerMask:
        //    case SerializedPropertyType.Float:
        //    case SerializedPropertyType.Enum:
        //    case SerializedPropertyType.Boolean:
        //    case SerializedPropertyType.String:
        //    case SerializedPropertyType.Quaternion:
        //    case SerializedPropertyType.Vector2:
        //    case SerializedPropertyType.Vector3:
        //    case SerializedPropertyType.Vector4:
        //    case SerializedPropertyType.Rect:
        //    case SerializedPropertyType.ObjectReference:
        //        EditorGUI.PropertyField(position, property, label);
        //        break;
        //}
        GUI.enabled = true;
    }
}

#endif
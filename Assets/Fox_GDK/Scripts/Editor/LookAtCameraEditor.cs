#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LookAtCamera))]
public class LookAtCameraEditor : FoxEditor
{
    private LookAtCamera scriptReference;

    // All SerializedProperties
    #region ALL_PUBLIC_PROPERTIES
    private SerializedProperty offset;
    private SerializedProperty useCustomCamera;
    private SerializedProperty camera;
    #endregion ALL_PUBLIC_PROPERTIES

    bool drawProperties = true;
    public void OnEnable()
    {
        scriptReference = (LookAtCamera)target;
        #region FINDER_ALL_PUBLIC_PROPERTIES_FINDER
        offset = serializedObject.FindProperty("position");
        useCustomCamera = serializedObject.FindProperty("useCustomCamera");
        camera = serializedObject.FindProperty("_camera");
        #endregion FINDER_ALL_PUBLIC_PROPERTIES
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawProperty(offset);
        DrawProperty(useCustomCamera);
        if (scriptReference.useCustomCamera)
        {
            DrawProperty(camera);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif

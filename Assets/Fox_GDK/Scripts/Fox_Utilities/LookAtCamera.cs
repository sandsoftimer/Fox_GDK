using UnityEditor;
using UnityEngine;

public class LookAtCamera : FoxObject
{
    public Fox_Axis axis;
    public Vector3 offset = Vector3.zero;
    public bool useCustomCamera;
    public Camera _camera;

    public Camera sceneCam
    {
        get
        {
            if (_camera == null)
                _camera = mainCamera;
            return _camera;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(sceneCam.transform.position - offset, Vector3.up);
    }
}

#region FOX EDITOR MAKER
#if UNITY_EDITOR

[CustomEditor(typeof(LookAtCamera))]
public class LookAtCameraEditor : FoxEditor
{
    private LookAtCamera targetScript;

    // All SerializedProperties
    #region ALL_PUBLIC_PROPERTIES
    private SerializedProperty offset;
    private SerializedProperty useCustomCamera;
    private SerializedProperty _camera;
    private SerializedProperty foxManager;
    private SerializedProperty foxTools;
    private SerializedProperty gameplayData;
    private SerializedProperty mainCamera;
    private SerializedProperty gameState;
    #endregion ALL_PUBLIC_PROPERTIES

    bool drawProperties = true;
    public void OnEnable()
    {
        targetScript = (LookAtCamera)target;

        #region FINDER_ALL_PUBLIC_PROPERTIES
        offset = serializedObject.FindProperty("offset");
        useCustomCamera = serializedObject.FindProperty("useCustomCamera");
        _camera = serializedObject.FindProperty("_camera");
        foxManager = serializedObject.FindProperty("foxManager");
        foxTools = serializedObject.FindProperty("foxTools");
        gameplayData = serializedObject.FindProperty("gameplayData");
        mainCamera = serializedObject.FindProperty("mainCamera");
        gameState = serializedObject.FindProperty("gameState");
        #endregion FINDER_ALL_PUBLIC_PROPERTIES
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        DrawProperty(offset);
        DrawProperty(useCustomCamera);
        if (targetScript.useCustomCamera)
        {
            DrawProperty(_camera);
        }


        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion FOX EDITOR MAKER
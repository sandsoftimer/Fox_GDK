#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Storage))]
public class StorageEditor : FoxEditor
{
    private Storage scriptReference;

    // All SerializedProperties
    #region ALL_PUBLIC_PROPERTIES
    private SerializedProperty supportedStorageObjects;
    private SerializedProperty allowStacking;
    private SerializedProperty stackingGap;
    private SerializedProperty capacity;
    private SerializedProperty vanishAfterCollect;
    private SerializedProperty unlimitedCapacity;
    private SerializedProperty randomStackingRotation;
    private SerializedProperty totalTaken;
    private SerializedProperty resourceTaken;

    #endregion ALL_PUBLIC_PROPERTIES

    public void OnEnable()
    {
        scriptReference = (Storage)target;
        #region FINDER_ALL_PUBLIC_PROPERTIES_FINDER
        supportedStorageObjects = serializedObject.FindProperty("supportedStorageObjects");
        vanishAfterCollect = serializedObject.FindProperty("vanishAfterCollect");
        unlimitedCapacity = serializedObject.FindProperty("unlimitedCapacity");
        allowStacking = serializedObject.FindProperty("allowStacking");
        stackingGap = serializedObject.FindProperty("stackingGap");
        capacity = serializedObject.FindProperty("capacity");
        randomStackingRotation = serializedObject.FindProperty("randomStackingRotation");
        totalTaken = serializedObject.FindProperty("totalTaken");
        resourceTaken = serializedObject.FindProperty("resourceTaken");
        #endregion FINDER_ALL_PUBLIC_PROPERTIES
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawProperty(supportedStorageObjects);

        Space();
        DrawHorizontalLine();

        DrawProperty(unlimitedCapacity);
        if (!scriptReference.unlimitedCapacity)
            DrawProperty(capacity);

        DrawProperty(vanishAfterCollect);
        DrawProperty(randomStackingRotation);
        DrawProperty(totalTaken);
        DrawProperty(resourceTaken);
        if (!scriptReference.vanishAfterCollect)
        {
            DrawProperty(allowStacking);
            if (scriptReference.allowStacking)
                DrawProperty(stackingGap);
        }
        else
        {
            scriptReference.allowStacking = false;
        }


        serializedObject.ApplyModifiedProperties();
    }
}
#endif

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MoveableObject))]
public class MoveableObjectEditor : FoxEditor
{
    private MoveableObject scriptReference;

    // All SerializedProperties
    #region ALL_PUBLIC_PROPERTIES
    private SerializedProperty movingTransfrom;
    private SerializedProperty negetiveMove;
    private SerializedProperty positiveMove;
    private SerializedProperty moveSpeed;
    #endregion ALL_PUBLIC_PROPERTIES

    public void OnEnable()
    {
        scriptReference = (MoveableObject)target;
        #region FINDER_ALL_PUBLIC_PROPERTIES_FINDER
        movingTransfrom = serializedObject.FindProperty("movingTransfrom");
        negetiveMove = serializedObject.FindProperty("negetiveMove");
        positiveMove = serializedObject.FindProperty("positiveMove");
        moveSpeed = serializedObject.FindProperty("moveSpeed");
        #endregion FINDER_ALL_PUBLIC_PROPERTIES
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Space();

        DrawProperty(movingTransfrom);
        DrawHorizontalLine();

        DrawProperty(negetiveMove);
        DrawProperty(positiveMove);
        DrawProperty(moveSpeed);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif

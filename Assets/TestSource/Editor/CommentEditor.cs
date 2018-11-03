#if UNITY_EDITOR
using UnityEditor;

[CustomEditor (typeof(Comment))]
public class CommentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Comment comment = (Comment)target;
        serializedObject.Update();
        SerializedProperty commentProp = serializedObject.FindProperty("comment");

        EditorGUI.BeginChangeCheck();

        commentProp.stringValue = EditorGUILayout.TextArea(commentProp.stringValue);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
            Undo.RecordObject(target, "Comment Change");
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
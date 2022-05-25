using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerTextureApplication))]
[CanEditMultipleObjects]
public class LookAtPointEditor : Editor
{
    private PlayerTextureApplication _target;

    private SerializedProperty _spriteToUse;
    private SerializedProperty _originalSprite;
    private SerializedProperty _createdSprite;

    void OnEnable()
    {
        _spriteToUse = serializedObject.FindProperty("spriteToUse");
        _originalSprite = serializedObject.FindProperty("originalSprite");
        _createdSprite = serializedObject.FindProperty("createdSprite");
        _target = (PlayerTextureApplication) target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_spriteToUse);
        EditorGUILayout.PropertyField(_originalSprite);
        EditorGUILayout.PropertyField(_createdSprite);
        if (GUILayout.Button("Apply textures"))
        {
            _target.ApplyTextureOnPlayer("Textures/TextureApplicationOnPlayer/PreApplication");
        }

        serializedObject.ApplyModifiedProperties();
    }
}